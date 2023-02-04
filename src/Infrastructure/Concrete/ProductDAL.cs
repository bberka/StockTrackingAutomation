using Domain.Entities;
using Domain.Models;
using Domain.ValueObjects;
using EasMe;
using EasMe.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DAL
{
	public class ProductDAL : EfEntityRepositoryBase<Product, BusinessDbContext>, IEfEntityRepository<Product>
	{
		private ProductDAL() { }
		public static ProductDAL This
		{
			get
			{
				Instance ??= new();
				return Instance;
			}
		}
		private static ProductDAL? Instance;

      
    }

}
