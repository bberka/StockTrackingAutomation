using Domain.Entities;
using Domain.Models;
using Domain.ValueObjects;
using EasMe;
using EasMe.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DAL
{
	public class UserDAL : EfEntityRepositoryBase<User,BusinessDbContext>, IEfEntityRepository<User>
	{
		private UserDAL() { }
		public static UserDAL This
		{
			get
			{
				Instance ??= new();
				return Instance;
			}
		}
		private static UserDAL? Instance;

		public ResultData<User> Login(LoginModel model)
		{
			var user = GetFirstOrDefault(x => x.EmailAddress == model.EmailAddress && x.IsValid == true && !x.DeletedDate.HasValue);
			if(user is null)
			{
				return ResultData<User>.Error(1, "Account not found");
			}
			var hashed = Convert.ToBase64String(model.Password.MD5Hash());
			if(user.Password != hashed)
			{
				return ResultData<User>.Error(2, "Account not found");
			}
			return ResultData<User>.Success(user);
		}
		public Result Register(User user)
		{
			var existEmail = Any(x => x.EmailAddress == user.EmailAddress);
			if(existEmail)
			{
				return Result.Error(1, "AlreadyExist:Email");
			}
			var res = Add(user);
			if (!res) return Result.Error(2, "DbError");
			return Result.Success();
		}
		public Result UpdateUser(User user)
		{
			var res = Update(user);
			if (!res) return Result.Error(3, "DbError");
			return Result.Success();
		}
		public List<User> GetValidUsers()
		{
			return GetList(x => x.IsValid == true && !x.DeletedDate.HasValue);
		}
		public void AddDefaultUser()
		{
			var user = new User
			{
				EmailAddress = "admin@mail.com",
				FailedPasswordCount = 0,
				IsValid = true,
				RegisterDate = DateTime.Now,
				RoleType = 2,
				Password = Convert.ToBase64String("admin".MD5Hash()),
			};
			Add(user);
		}
	}

}
