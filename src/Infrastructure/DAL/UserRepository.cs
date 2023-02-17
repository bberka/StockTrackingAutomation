using Domain.Entities;
using EasMe.EntityFrameworkCore.V1;

namespace Infrastructure.DAL;

public class UserRepository : EntityRepositoryBase<User, BusinessDbContext>
{
    public UserRepository(BusinessDbContext dbContext) : base(dbContext)
    {
    }
}