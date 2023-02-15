using Domain.Abstract;
using Domain.Entities;
using EasMe.Models;

namespace Application.Manager
{
    public interface IProductMgr
    {
        List<Product> GetValidProducts();
        Product? GetProduct(int id);
        Result UpdateProduct(Product product);
        Result AddProduct(Product product);
        Result DeleteProduct(int id);
    }

    public class ProductMgr : IProductMgr
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductMgr(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<Product> GetValidProducts()
        {
            return _unitOfWork.Products.GetList(x => !x.DeletedDate.HasValue);
        }

		public Product? GetProduct(int id)
		{
			return _unitOfWork.Products.Find(id);
		}
		public Result UpdateProduct(Product product)
		{
            var current = _unitOfWork.Products.Find(product.Id);
            if (current is null)
            {
                return Result.Error(1, "Ürün bulunamadı");
            }
            current.Description = product.Description;
            current.Name = product.Name;
            _unitOfWork.Products.Update(current);
            var res = _unitOfWork.Save();
            if (!res)
            {
                return Result.Error(2, "DbError");
            }
            return Result.Success();
        }
        public Result AddProduct(Product product)
        {
            var exist = _unitOfWork.Products.Any(x => x.Name == product.Name && !x.DeletedDate.HasValue);
            if (exist) return Result.Error(1, "Ürün zaten mevcut");
            _unitOfWork.Products.Add(product);
            var res = _unitOfWork.Save();
            if (!res) return Result.Error(2, "DbError");
            return Result.Success();
        }
        public Result DeleteProduct(int id)
        {
            var product = GetProduct(id);
            if (product is null) return Result.Error(1, "Ürün bulunamadı");
            product.DeletedDate = DateTime.Now;
            _unitOfWork.Products.Update(product);
            var res = _unitOfWork.Save();
            if (!res) return Result.Error(2, "DbError");
            return Result.Success();

        }
    }
}
