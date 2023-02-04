using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Concrete;
using Infrastructure.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Manager
{
    public class BuyLogMgr
    {

		private BuyLogMgr() { }
		public static BuyLogMgr This
		{
			get
			{
				Instance ??= new();
				return Instance;
			}
		}
		private static BuyLogMgr? Instance;
        public List<BuyLog> GetValidList()
        {
            var list = BuyLogDAL.This.GetList();
            var products = ProductMgr.This.GetValidProducts().Select(x => x.ProductNo);
            var suppliers = SupplierMgr.This.GetValidSuppliers().Select(x => x.SupplierNo);
            list.RemoveAll(x => !products.Contains(x.ProductId));
            list.RemoveAll(x => !suppliers.Contains(x.SupplierId));
            return list;
        }

        public Result AddBuyLog(BuyLog data)
        {
            var product = ProductDAL.This.Find(data.ProductId);
            if (product is null)
            {
                return Result.Error(1, "Ürün bulunamadı");
            }
            var supplier = SupplierMgr.This.GetValidSupplier(data.SupplierId);
            if(supplier is null)
            {
                return Result.Error(2, "Tedarikçi bulunamadı");
            }
            product.Stock += data.Count;
            var productUpdate = ProductDAL.This.Update(product);
            if (!productUpdate)
            {
                return Result.Error(3, "DbError");
            }
            var totalPrice = data.PricePerUnit * data.Count;
            supplier.Debt += totalPrice;
            var supplierUpdate = SupplierDAL.This.Update(supplier);
            if (!supplierUpdate)
            {
                return Result.Error(4, "DbError");
            }
            var res = BuyLogDAL.This.Add(data);
            if (!res)
            {
                return Result.Error(5, "DbError");
            }
            return Result.Success();
        }
    }
}
