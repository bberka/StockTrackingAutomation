using Domain.Entities;
using EasMe;
using Infrastructure.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Caching
{
    public static class DbCache
    {
        public readonly static EasCache<List<User>> UserCache = new(UserDAL.This.GetValidUsers,1);
        public readonly static EasCache<List<Product>> ProductCache = new(ProductDAL.This.GetValidProducts,1);

        public static string GetProductName(int id)
        {
            var list = ProductCache.Get();
            return list.FirstOrDefault(x => x.ProductNo == id)?.Name ?? "NaN";
        }
        public static string GetUserEmail(int id)
        {
            var list = UserCache.Get();
            return list.FirstOrDefault(x => x.UserNo == id)?.EmailAddress ?? "NaN";
        }
    }
}
