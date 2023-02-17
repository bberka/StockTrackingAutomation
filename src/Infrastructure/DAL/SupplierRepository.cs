using Domain.Entities;
using EasMe.EntityFrameworkCore.V1;

namespace Infrastructure.DAL;

public class SupplierRepository : EntityRepositoryBase<Supplier, BusinessDbContext>
{
    public SupplierRepository(BusinessDbContext dbContext) : base(dbContext)
    {
    }
}