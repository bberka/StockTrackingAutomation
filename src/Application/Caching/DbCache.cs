using Application.Manager;
using Domain.Entities;
using EasMe;
using Infrastructure.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Caching
{
    public static class DbCache
    {
        public readonly static EasCache<List<User>> UserCache = new(UserMgr.This.GetValidUsers,1);
        public readonly static EasCache<List<Product>> ProductCache = new(ProductMgr.This.GetValidProducts,1);
        public readonly static EasCache<List<Customer>> CustomerCache = new(CustomerMgr.This.GetValidCustomers,1);
        public readonly static EasCache<List<Supplier>> SupplierCache = new(SupplierMgr.This.GetValidSuppliers,1);
        public static string GetSupplierName(int id)
        {
            var data = SupplierCache.Get().FirstOrDefault(x => x.SupplierNo == id);
            if (data is null) return "NaN";
            return data.Name;
        }
     
        public static string GetCustomerName(int id)
        {
            var list = CustomerCache.Get();
            var data = list.FirstOrDefault(x => x.CustomerNo == id);
            if (data == null) return "NaN";
            return data.Name + " " + data.CompanyName;

        }
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
