using Domain.Entities;
using EasMe.Models;

namespace Domain.Abstract;

public interface IProductService
{
    List<Product> GetList();
    Product? GetProduct(int id);
    Result UpdateProduct(Product product);
    Result AddProduct(Product product);
    Result DeleteProduct(int id);
}