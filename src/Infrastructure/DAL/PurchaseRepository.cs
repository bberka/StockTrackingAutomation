using Domain.Entities;
using EasMe.EntityFrameworkCore.V1;

namespace Infrastructure.DAL;

public class PurchaseRepository : EntityRepositoryBase<Purchase, BusinessDbContext>
{
    public PurchaseRepository(BusinessDbContext dbContext) : base(dbContext)
    {
    }
}