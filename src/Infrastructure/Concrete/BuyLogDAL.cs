using Domain.Entities;
using EasMe.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Concrete
{
    public class BuyLogDAL : EfEntityRepositoryBase<BuyLog, BusinessDbContext>, IEfEntityRepository<BuyLog>
    {
		private BuyLogDAL() { }
		public static BuyLogDAL This
		{
			get
			{
				Instance ??= new();
				return Instance;
			}
		}
		private static BuyLogDAL? Instance;
    }
}
