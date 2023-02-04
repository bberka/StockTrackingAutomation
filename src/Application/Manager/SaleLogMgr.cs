using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Manager
{
    public class SaleLogMgr
    {

		private SaleLogMgr() { }
		public static SaleLogMgr This
		{
			get
			{
				Instance ??= new();
				return Instance;
			}
		}
		private static SaleLogMgr? Instance;

		public List<SaleLog> GetValidList()
		{
            var list = SaleLogDAL.This.GetList();
            var products = ProductMgr.This.GetValidProducts().Select(x => x.ProductNo);
            list.RemoveAll(x => !products.Contains(x.ProductId));
			return list;
        }

		public Result AddSaleLog(SaleLog data)
		{
            var product = ProductDAL.This.Find(data.ProductId);
            if (product is null)
            {
                return Result.Error(1, "Ürün bulunamadı");
            }
            var customer = CustomerMgr.This.GetValidCustomer(data.CustomerId);
            if (customer is null)
            {
                return Result.Error(2, "Müşteri bulunamadı");
            }
            var totalPrice = data.PricePerUnit * data.Count;
            product.Stock -= data.Count;
            customer.Debt += totalPrice;
            if (product.Stock < 0)
            {
                return Result.Error(3, "Yeterli stok yok");
            }
            var productUpdate = ProductDAL.This.Update(product);
            if (!productUpdate)
            {
                return Result.Error(4, "DbError");
            }
            var customerUpdate = CustomerDAL.This.Update(customer);
            if (!customerUpdate)
            {
                return Result.Error(5, "DbError");
            }
            var res = SaleLogDAL.This.Add(data);
            if (!res)
            {
                return Result.Error(6, "DbError");
            }
            return Result.Success();
        }
    }
}
