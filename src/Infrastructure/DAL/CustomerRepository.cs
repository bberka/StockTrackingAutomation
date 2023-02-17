using Domain.Entities;
using EasMe.EntityFrameworkCore.V1;

namespace Infrastructure.DAL;

public class CustomerRepository : EntityRepositoryBase<Customer, BusinessDbContext>
{
    public CustomerRepository(BusinessDbContext dbContext) : base(dbContext)
    {
    }
}