using Domain.Entities;
using Domain.Models;
using Domain.ValueObjects;
using EasMe;
using Infrastructure.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Manager
{
    public class UserMgr
    {
        private UserMgr() { }
        public static UserMgr This
        {
            get
            {
                Instance ??= new();
                return Instance;
            }
        }
        private static UserMgr? Instance;
        public ResultData<User> Login(LoginModel model)
        {
            var user = UserDAL.This.GetFirstOrDefault(x => x.EmailAddress == model.EmailAddress && x.IsValid == true && !x.DeletedDate.HasValue);
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
            var existEmail = UserDAL.This.Any(x => x.EmailAddress == user.EmailAddress);
            if (existEmail)
            {
                return Result.Error(1, "Bu mail zaten var");
            }
            user.Password = Convert.ToBase64String(user.Password.MD5Hash());
            var res = UserDAL.This.Add(user);
            if (!res) return Result.Error(2, "DbError");
            return Result.Success();
        }
        public Result UpdateUser(User user)
        {
            var current = UserDAL.This.Find(user.UserNo);
            if (current is null)
            {
                return Result.Error(1, "Kullanıcı bulunamadı");
            }
            current.IsValid = user.IsValid;
            current.EmailAddress = user.EmailAddress;
            current.Password = Convert.ToBase64String(user.Password.MD5Hash());
            current.RoleType = user.RoleType;
            var res = UserDAL.This.Update(user);
            if (!res)
            {
                return Result.Error(2, "DbError");
            }
            return Result.Success();
        }
        public List<User> GetValidUsers()
        {
            return UserDAL.This.GetList(x => x.IsValid == true && !x.DeletedDate.HasValue);
        }
        
        public Result DeleteUser(int id)
        {
            var user = UserDAL.This.Find(id);
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
            var res = UserDAL.This.Update(user);
            if (!res)
            {
                return Result.Error(3, "DbError");
            }
            return Result.Success();
        }

        public User GetUser(int id)
        {
            return UserDAL.This.Find(id);
        }
    }
}
