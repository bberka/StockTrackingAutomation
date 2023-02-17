using Domain.Entities;
using EasMe.EntityFrameworkCore.V1;

namespace Infrastructure.DAL;

public class ProductRepository : EntityRepositoryBase<Product, BusinessDbContext>
{
    public ProductRepository(BusinessDbContext dbContext) : base(dbContext)
    {
    }
}