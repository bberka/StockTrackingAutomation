using Domain.Abstract;
using Domain.Entities;
using EasMe.Models;

namespace Application.Services
{


    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<Product> GetList()
        {
            return _unitOfWork.ProductRepository.GetList(x => !x.DeletedDate.HasValue);
        }

		public Product? GetProduct(int id)
		{
			return _unitOfWork.ProductRepository.Find(id);
		}
		public Result UpdateProduct(Product product)
		{
            var current = _unitOfWork.ProductRepository.Find(product.Id);
            if (current is null)
            {
                return Result.Error(1, "Ürün bulunamadı");
            }
            current.Description = product.Description;
            current.Name = product.Name;
            _unitOfWork.ProductRepository.Update(current);
            var res = _unitOfWork.Save();
            if (!res)
            {
                return Result.Error(2, "DbError");
            }
            return Result.Success();
        }
        public Result AddProduct(Product product)
        {
            var exist = _unitOfWork.ProductRepository.Any(x => x.Name == product.Name && !x.DeletedDate.HasValue);
            if (exist) return Result.Error(1, "Ürün zaten mevcut");
            _unitOfWork.ProductRepository.Add(product);
            var res = _unitOfWork.Save();
            if (!res) return Result.Error(2, "DbError");
            return Result.Success();
        }
        public Result DeleteProduct(int id)
        {
            var product = GetProduct(id);
            if (product is null) return Result.Error(1, "Ürün bulunamadı");
            product.DeletedDate = DateTime.Now;
            _unitOfWork.ProductRepository.Update(product);
            var res = _unitOfWork.Save();
            if (!res) return Result.Error(2, "DbError");
            return Result.Success();

        }
    }
}
