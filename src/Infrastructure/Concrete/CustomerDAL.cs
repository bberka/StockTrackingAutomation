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
	public class CustomerDAL : EfEntityRepositoryBase<Customer, BusinessDbContext>, IEfEntityRepository<Customer>
	{
		private CustomerDAL() { }
		public static CustomerDAL This
		{
			get
			{
				Instance ??= new();
				return Instance;
			}
		}
		private static CustomerDAL? Instance;
		

    }

}
