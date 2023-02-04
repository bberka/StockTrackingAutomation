using Domain.Entities;
using Domain.ValueObjects;
using EasMe.Extensions;
using Infrastructure.DAL;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Manager
{
    public class ProductMgr
    {

		private ProductMgr() { }
		public static ProductMgr This
		{
			get
			{
				Instance ??= new();
				return Instance;
			}
		}
		private static ProductMgr? Instance;

        public List<Product> GetValidProducts()
        {
            return ProductDAL.This.GetList(x => !x.DeletedDate.HasValue);
        }

		public Product? GetProduct(int id)
		{
			return ProductDAL.This.Find(id);
		}
		public Result UpdateProduct(Product product)
		{
            var current = ProductDAL.This.Find(product.ProductNo);
            if (current is null)
            {
                return Result.Error(1, "Ürün bulunamadı");
            }
            current.Description = product.Description;
            current.Name = product.Name;
            var res = ProductDAL.This.Update(current);
            if (!res)
            {
                return Result.Error(2, "DbError");
            }
            return Result.Success();
        }
        public Result AddProduct(Product product)
        {
            var exist = ProductDAL.This.Any(x => x.Name == product.Name && !x.DeletedDate.HasValue);
            if (exist) return Result.Error(1, "Ürün zaten mevcut");
            var res = ProductDAL.This.Add(product);
            if (!res) return Result.Error(2, "DbError");
            return Result.Success();
        }
        public Result DeleteProduct(int id)
        {
            var product = GetProduct(id);
            if (product is null) return Result.Error(1, "Ürün bulunamadı");
            product.DeletedDate = DateTime.Now;
            var res = ProductDAL.This.Update(product);
            if (!res) return Result.Error(2, "DbError");
            return Result.Success();

        }
    }
}
