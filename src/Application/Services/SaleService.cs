using Domain.Abstract;
using Domain.Entities;
using EasMe.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    
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
            var list = _unitOfWork.SaleRepository
                .Get()
                .Include(x => x.Product)
                .Include(x => x.User)
                .Include(x => x.Customer)
                .ToList();
            var products = _productService.GetValidProducts().Select(x => x.Id);
            list.RemoveAll(x => !products.Contains(x.ProductId));
			return list;
        }
		public Result AddSaleLog(Sale data)
		{
            var product = _unitOfWork.ProductRepository.Find(data.ProductId);
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
            _unitOfWork.ProductRepository.Update(product);
            _unitOfWork.CustomerRepository.Update(customer);
            _unitOfWork.SaleRepository.Add(data);
            var res = _unitOfWork.Save();
            if (!res)
            {
                return Result.Error(4, "DbError");
            }
            return Result.Success();
        }

        public ResultData<Sale> GetSale(int id)
        {
            var data = _unitOfWork.SaleRepository
                .Get(x => x.Id == id)
                .Include(x => x.Customer)
                .Include(x => x.User)
                .Include(x => x.Product)
                .FirstOrDefault();
            if (data is null)
            {
                return Result.Warn(1,"Satış bulunmadı");
            }

            return data;
        }
    }
}
