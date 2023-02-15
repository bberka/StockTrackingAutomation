using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Infrastructure
{
	public class BusinessDbContext : DbContext
	{
		public BusinessDbContext() : base()
		{
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["BusinessDb"].ConnectionString);

		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

		}
		public static bool EnsureCreated()
		{
			var res = new BusinessDbContext().Database.EnsureCreated();
			//if (res)
			//{
			//	UserDAL.This.AddDefaultUser();
			//}
			return res;
		}
		public DbSet<Product> Products { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Sale> Sales { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<DebtLog> DebtLogs { get; set; }
		public DbSet<Purchase> Purchases { get; set; }
		public DbSet<Supplier> Suppliers { get; set; }
	}
}
