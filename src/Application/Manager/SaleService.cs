using Domain.Abstract;
using Domain.Entities;
using EasMe.Models;

namespace Application.Manager
{
    public interface ISaleService
    {
        List<Sale> GetValidList();
        Result AddSaleLog(Sale data);
    }

    public class SaleService : ISaleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;

        public SaleService(
            IUnitOfWork unitOfWork,
            IProductService productService,
            ICustomerService customerService)
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
            _customerService = customerService;
        }
		public List<Sale> GetValidList()
		{
            var list = _unitOfWork.Sales.GetList();
            var products = _productService.GetValidProducts().Select(x => x.Id);
            list.RemoveAll(x => !products.Contains(x.ProductId));
			return list;
        }

		public Result AddSaleLog(Sale data)
		{
            var product = _unitOfWork.Products.Find(data.ProductId);
            if (product is null)
            {
                return Result.Error(1, "Ürün bulunamadı");
            }
            var customerResult = _customerService.GetValidCustomer(data.CustomerId);
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
            _unitOfWork.Sales.Add(data);
            var res = _unitOfWork.Save();
            if (!res)
            {
                return Result.Error(4, "DbError");
            }
            return Result.Success();
        }
    }
}
