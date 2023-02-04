using Domain.Entities;
using Domain.Models;
using Domain.ValueObjects;
using EasMe;
using EasMe.EFCore;
using EasMe.Extensions;
using Microsoft.Extensions.Logging;
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
            UserDAL.This.Add(user);
        }
    }

}
