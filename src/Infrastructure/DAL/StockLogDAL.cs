using Domain.Entities;
using EasMe.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DAL
{
	public class StockLogDAL : EfEntityRepositoryBase<StockLog, BusinessDbContext>, IEfEntityRepository<StockLog>
	{
		private StockLogDAL() { }
		public static StockLogDAL This
		{
			get
			{
				Instance ??= new();
				return Instance;
			}
		}
		private static StockLogDAL? Instance;
	}
}
