using Domain.Entities;
using EasMe.EntityFrameworkCore.V1;

namespace Infrastructure.DAL;

public class SaleRepository : EntityRepositoryBase<Sale, BusinessDbContext>
{
    public SaleRepository(BusinessDbContext dbContext) : base(dbContext)
    {
    }
}