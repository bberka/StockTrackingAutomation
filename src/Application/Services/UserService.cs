using Domain.Abstract;
using Domain.Entities;
using Domain.Models;
using EasMe;
using EasMe.Extensions;

namespace Application.Services
{


    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ResultData<User> Login(LoginModel model)
        {
            var user = _unitOfWork.UserRepository
                .GetFirstOrDefault(x => x.EmailAddress == model.EmailAddress 
                                        && x.IsValid == true 
                                        && !x.DeletedDate.HasValue);
            if (user is null)
            {
                return ResultData<User>.Error(1, "Hesap bulunamadı");
            }
            var hashed = Convert.ToBase64String(model.Password.MD5Hash());
            if (user.Password != hashed)
            {
                return ResultData<User>.Error(2, "Şifre yanlış");
            }
            if(user.RoleType == 0)
            {
                return ResultData<User>.Error(3, "Rol belirlenmemiş");
            }

            var ctx = HttpContextHelper.Current;
            var ip = ctx?.Connection.RemoteIpAddress.ToString();
            var useragent = ctx?.Request.GetUserAgent();
            if (ip is not null || useragent is not null)
            {
                user.LastLoginIp = ip;
                user.LastLoginUserAgent = useragent;
                user.LastLoginDate = DateTime.Now;
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Save();
            }
            
            return ResultData<User>.Success(user);
        }
        public Result Register(User user)
        {
            var existEmail = _unitOfWork.UserRepository.Any(x => x.EmailAddress == user.EmailAddress);
            if (existEmail)
            {
                return Result.Error(1, "Bu mail zaten var");
            }
            user.Password = Convert.ToBase64String(user.Password.MD5Hash());
            _unitOfWork.UserRepository.Add(user);
            var res = _unitOfWork.Save();
            if (!res) return Result.Error(2, "DbError");
            return Result.Success();
        }
        public Result UpdateUser(User user)
        {
            var current = _unitOfWork.UserRepository.Find(user.Id);
            if (current is null)
            {
                return Result.Error(1, "Kullanıcı bulunamadı");
            }
            current.IsValid = user.IsValid;
            current.EmailAddress = user.EmailAddress;
            current.Password = Convert.ToBase64String(user.Password.MD5Hash());
            current.RoleType = user.RoleType;
            _unitOfWork.UserRepository.Update(user);
            var res = _unitOfWork.Save();
            if (!res)
            {
                return Result.Error(2, "DbError");
            }
            return Result.Success();
        }
        public List<User> GetList()
        {
            return _unitOfWork.UserRepository.GetList(x => x.IsValid == true && !x.DeletedDate.HasValue);
        }
        
        public Result DeleteUser(int id)
        {
            var user = _unitOfWork.UserRepository.Find(id);
            if (user == null)
            {
                return Result.Error(1, "Kullanıcı bulunamadı");
            }
            if (user.DeletedDate.HasValue)
            {
                return Result.Error(2, "Kullanıcı zaten silinmiş");
            }
            user.DeletedDate = DateTime.Now;
            user.IsValid = false;
            _unitOfWork.UserRepository.Update(user);
            var res = _unitOfWork.Save();
            if (!res)
            {
                return Result.Error(3, "DbError");
            }
            return Result.Success();
        }

        public ResultData<User> GetUser(int id)
        {
            var user = _unitOfWork.UserRepository.Find(id);
            if (user is null)
            {
                return Result.Warn(1, "Kullanıcı bulunamadı");
            }

            if (!user.IsValid)
            {
                return Result.Error(2, "Kullanıcı geçersiz");
            }
            if (user.DeletedDate.HasValue)
            {
                return Result.Error(3, "Kullanıcı silinmiş");
            }
            return user;
        }

        public Result ChangePassword(int userId, ChangePasswordModel model)
        {
            var userResult = GetUser(userId);
            if (userResult.IsFailure)
            {
                return userResult.ToResult(100);
            }

            var user = userResult.Data;
            var encryptedPassword = model.OldPassword.MD5Hash().ToBase64String();
            if (string.CompareOrdinal(user?.Password, encryptedPassword) != 0)
            {
                return Result.Warn(1, "Şifre yanlış");
            }

            if (model.NewPassword != model.NewPasswordConfirm)
            {
                return Result.Warn(2, "Yeni şifreler aynı değil");
            }

            var newEncryptedPassword = model.NewPasswordConfirm.MD5Hash().ToBase64String();
            user.Password = newEncryptedPassword;
            user.PasswordLastUpdateDate = DateTime.Now;
            _unitOfWork.UserRepository.Update(user);
            return _unitOfWork.SaveResult(3);

        }
    }
}
