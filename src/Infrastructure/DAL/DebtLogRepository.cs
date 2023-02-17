using Domain.Entities;
using EasMe.EntityFrameworkCore.V1;

namespace Infrastructure.DAL;

public class DebtLogRepository : EntityRepositoryBase<DebtLog, BusinessDbContext>
{
    public DebtLogRepository(BusinessDbContext dbContext) : base(dbContext)
    {
    }
}