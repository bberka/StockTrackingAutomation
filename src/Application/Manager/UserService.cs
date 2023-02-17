using Domain.Abstract;
using Domain.Entities;
using Domain.Models;
using EasMe;
using EasMe.Models;

namespace Application.Manager
{
    public interface IUserService
    {
        ResultData<User> Login(LoginModel model);
        Result Register(User user);
        Result UpdateUser(User user);
        List<User> GetValidUsers();
        Result DeleteUser(int id);
        User? GetUser(int id);
    }

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
        public List<User> GetValidUsers()
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

        public User? GetUser(int id)
        {
            return _unitOfWork.UserRepository.Find(id);
        }
    }
}
