using Domain.Abstract;
using Domain.Entities;
using Domain.Models;
using EasMe;
using EasMe.Models;

namespace Application.Manager
{
    public interface IUserMgr
    {
        ResultData<User> Login(LoginModel model);
        Result Register(User user);
        Result UpdateUser(User user);
        List<User> GetValidUsers();
        Result DeleteUser(int id);
        User? GetUser(int id);
    }

    public class UserMgr : IUserMgr
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserMgr(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ResultData<User> Login(LoginModel model)
        {
            var user = _unitOfWork.Users.GetFirstOrDefault(x => x.EmailAddress == model.EmailAddress && x.IsValid == true && !x.DeletedDate.HasValue);
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
            var existEmail = _unitOfWork.Users.Any(x => x.EmailAddress == user.EmailAddress);
            if (existEmail)
            {
                return Result.Error(1, "Bu mail zaten var");
            }
            user.Password = Convert.ToBase64String(user.Password.MD5Hash());
            _unitOfWork.Users.Add(user);
            var res = _unitOfWork.Save();
            if (!res) return Result.Error(2, "DbError");
            return Result.Success();
        }
        public Result UpdateUser(User user)
        {
            var current = _unitOfWork.Users.Find(user.UserNo);
            if (current is null)
            {
                return Result.Error(1, "Kullanıcı bulunamadı");
            }
            current.IsValid = user.IsValid;
            current.EmailAddress = user.EmailAddress;
            current.Password = Convert.ToBase64String(user.Password.MD5Hash());
            current.RoleType = user.RoleType;
            _unitOfWork.Users.Update(user);
            var res = _unitOfWork.Save();
            if (!res)
            {
                return Result.Error(2, "DbError");
            }
            return Result.Success();
        }
        public List<User> GetValidUsers()
        {
            return _unitOfWork.Users.GetList(x => x.IsValid == true && !x.DeletedDate.HasValue);
        }
        
        public Result DeleteUser(int id)
        {
            var user = _unitOfWork.Users.Find(id);
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
            _unitOfWork.Users.Update(user);
            var res = _unitOfWork.Save();
            if (!res)
            {
                return Result.Error(3, "DbError");
            }
            return Result.Success();
        }

        public User? GetUser(int id)
        {
            return _unitOfWork.Users.Find(id);
        }
    }
}
