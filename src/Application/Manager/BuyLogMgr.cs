using Domain.Abstract;
using Domain.Entities;
using EasMe.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Manager
{
    public interface IBuyLogMgr
    {
        List<BuyLog> GetValidList();
        Result AddBuyLog(BuyLog data);
    }

    public class BuyLogMgr : IBuyLogMgr
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductMgr _productMgr;
        private readonly ISupplierMgr _supplierMgr;

        public BuyLogMgr(
            IUnitOfWork unitOfWork,
            IProductMgr productMgr,
            ISupplierMgr supplierMgr)
        {
            _unitOfWork = unitOfWork;
            _productMgr = productMgr;
            _supplierMgr = supplierMgr;
        }
        public List<BuyLog> GetValidList()
        {
            var list = _unitOfWork.BuyLogs.Get()
                .Include(x => x.Product)
                .Include(x => x.Supplier)
                .Include(x => x.User)
                .ToList();
            var products = _productMgr.GetValidProducts().Select(x => x.Id);
            var suppliers = _supplierMgr.GetValidSuppliers().Select(x => x.Id);
            list.RemoveAll(x => !products.Contains(x.ProductId));
            list.RemoveAll(x => !suppliers.Contains(x.SupplierId));
            return list;
        }

        public Result AddBuyLog(BuyLog data)
        {
            var product = _unitOfWork.Products.Find(data.ProductId);
            if (product is null)
            {
                return Result.Error(1, "Ürün bulunamadı");
            }
            var supplier = _supplierMgr.GetValidSupplier(data.SupplierId);
            if(supplier is null)
            {
                return Result.Error(2, "Tedarikçi bulunamadı");
            }
            product.Stock += data.Count;
            _unitOfWork.Products.Update(product);
            var totalPrice = data.PricePerUnit * data.Count;
            supplier.Debt += totalPrice;
            _unitOfWork.Suppliers.Update(supplier);
            _unitOfWork.BuyLogs.Add(data);
            var res = _unitOfWork.Save();
            if (!res)
            {
                return Result.Error(5, "DbError");
            }
            return Result.Success();
        }
    }
}
