using Domain.Entities;
using EasMe.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Concrete
{
    public class SupplierDAL : EfEntityRepositoryBase<Supplier, BusinessDbContext>, IEfEntityRepository<Supplier>
    {
		private SupplierDAL() { }
		public static SupplierDAL This
		{
			get
			{
				Instance ??= new();
				return Instance;
			}
		}
		private static SupplierDAL? Instance;
    }
}
