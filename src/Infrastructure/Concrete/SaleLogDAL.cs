using Domain.Entities;
using EasMe.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DAL
{
	public class SaleLogDAL : EfEntityRepositoryBase<SaleLog, BusinessDbContext>, IEfEntityRepository<SaleLog>
	{
		private SaleLogDAL() { }
		public static SaleLogDAL This
		{
			get
			{
				Instance ??= new();
				return Instance;
			}
		}
		private static SaleLogDAL? Instance;
	}
}
