using Domain.Entities;
using EasMe.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DAL
{
    public class DebtLogDAL : EfEntityRepositoryBase<DebtLog, BusinessDbContext>, IEfEntityRepository<DebtLog>
    {
        private DebtLogDAL() { }
        public static DebtLogDAL This
        {
            get
            {
                Instance ??= new();
                return Instance;
            }
        }
        private static DebtLogDAL? Instance;


    }
}
