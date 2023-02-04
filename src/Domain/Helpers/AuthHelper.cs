using Domain.Entities;
using Domain.Enums;
using EasMe.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helpers
{
    public static class AuthHelper
    {
        public static void SetUser(this HttpContext context,User user)
        {
            context?.Session.SetString("user", user.ToJsonString());
        }
        public static User GetUser(this HttpContext context)
        {
            var str = context?.Session.GetString("user");
            if (str is null)
            {
                throw new UnauthorizedAccessException();
            }
            var data = str.FromJsonString<User>();
            if(data is null)
            {
                throw new UnauthorizedAccessException();
            }
            return data;
        }
        public static RoleType GetRole(this HttpContext context)
        {
            var user = context.GetUser();
            return (RoleType)user.RoleType;
        }
        public static bool IsInRole(this HttpContext context,RoleType roleType)
        {
            var role = context.GetRole();
            return role == roleType;
        }
        public static void RemoveAuth(this HttpContext context)
        {
            context?.Session.Remove("user");
        }
        public static bool IsAuthenticated(this HttpContext context)
        {
            try
            {
                _ = context.GetUser();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
