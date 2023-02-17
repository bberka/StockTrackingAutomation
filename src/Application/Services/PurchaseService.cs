using Domain.Abstract;
using Domain.Entities;
using EasMe.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{


    public class PurchaseService : IPurchaseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productMgr;
        private readonly ISupplierService _supplierMgr;

        public PurchaseService(
            IUnitOfWork unitOfWork,
            IProductService productMgr,
            ISupplierService supplierMgr)
        {
            _unitOfWork = unitOfWork;
            _productMgr = productMgr;
            _supplierMgr = supplierMgr;
        }
        public List<Purchase> GetValidList()
        {
            var list = _unitOfWork.PurchaseRepository.Get()
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

        public List<Purchase> GetSupplierPurchases(int supplierId)
        {
            var isValid = _unitOfWork.SupplierRepository.Any(x => !x.DeletedDate.HasValue && x.Id == supplierId);
            if (!isValid) return new();
            var list = _unitOfWork.PurchaseRepository.Get(x => x.SupplierId == supplierId)
                .Include(x => x.Product)
                .Include(x => x.Supplier)
                .Include(x => x.User)
                .ToList();
            var products = _productMgr.GetValidProducts().Select(x => x.Id);
            list.RemoveAll(x => !products.Contains(x.ProductId));
            return list;
        }

        public Result AddBuyLog(Purchase data)
        {
            var product = _unitOfWork.ProductRepository.Find(data.ProductId);
            if (product is null)
            {
                return Result.Error(1, "Ürün bulunamadı");
            }
            var supplierResult = _supplierMgr.GetValidSupplier(data.SupplierId);
            if(supplierResult.IsFailure)
            {
                return supplierResult.ToResult(100);
            }
            var supplier = supplierResult.Data;
            product.Stock += data.Count;
            _unitOfWork.ProductRepository.Update(product);
            var totalPrice = data.PricePerUnit * data.Count;
            supplier.Debt += totalPrice;
            _unitOfWork.SupplierRepository.Update(supplier);
            _unitOfWork.PurchaseRepository.Add(data);
            var res = _unitOfWork.Save();
            if (!res)
            {
                return Result.Error(5, "DbError");
            }
            return Result.Success();
        }
    }
}
