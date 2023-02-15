using Domain.Abstract;
using Domain.Entities;
using EasMe.Models;

namespace Application.Manager
{
    public interface ISaleLogMgr
    {
        List<SaleLog> GetValidList();
        Result AddSaleLog(SaleLog data);
    }

    public class SaleLogMgr : ISaleLogMgr
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductMgr _productMgr;
        private readonly ICustomerMgr _customerMgr;

        public SaleLogMgr(
            IUnitOfWork unitOfWork,
            IProductMgr productMgr,
            ICustomerMgr customerMgr)
        {
            _unitOfWork = unitOfWork;
            _productMgr = productMgr;
            _customerMgr = customerMgr;
        }
		public List<SaleLog> GetValidList()
		{
            var list = _unitOfWork.SaleLogs.GetList();
            var products = _productMgr.GetValidProducts().Select(x => x.Id);
            list.RemoveAll(x => !products.Contains(x.ProductId));
			return list;
        }

		public Result AddSaleLog(SaleLog data)
		{
            var product = _unitOfWork.Products.Find(data.ProductId);
            if (product is null)
            {
                return Result.Error(1, "Ürün bulunamadı");
            }
            var customerResult = _customerMgr.GetValidCustomer(data.CustomerId);
            if (customerResult.IsFailure)
            {
                return customerResult.ToResult(100);
            }
            var customer = customerResult.Data;
            var totalPrice = data.PricePerUnit * data.Count;
            product.Stock -= data.Count;
            customer.Debt += totalPrice;
            if (product.Stock < 0)
            {
                return Result.Error(3, "Yeterli stok yok");
            }
            _unitOfWork.Products.Update(product);
            _unitOfWork.Customers.Update(customer);
            _unitOfWork.SaleLogs.Add(data);
            var res = _unitOfWork.Save();
            if (!res)
            {
                return Result.Error(4, "DbError");
            }
            return Result.Success();
        }
    }
}
