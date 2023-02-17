using Domain.Entities;
using EasMe.Models;

namespace Domain.Abstract;

public interface IProductService
{
    List<Product> GetValidProducts();
    Product? GetProduct(int id);
    Result UpdateProduct(Product product);
    Result AddProduct(Product product);
    Result DeleteProduct(int id);
}