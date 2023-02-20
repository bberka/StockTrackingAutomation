using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using EasMe;
using EasMe.Extensions;

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
            var ctx = new BusinessDbContext();

            var res = ctx.Database.EnsureCreated();
            ctx.Database.Migrate();

            var user = new User()
            {
                IsValid = true,
                DeletedDate = null,
                EmailAddress = "admin@mail.com",
                FailedPasswordCount = 0,
                LastLoginDate = null,
                LastLoginIp = null,
                LastLoginUserAgent = null,
                Password = "admin".MD5Hash().ToBase64String(),
                PasswordLastUpdateDate = null,
                RegisterDate = DateTime.Now,
                RoleType = 2,
            };
            ctx.Add(user);
			ctx.SaveChanges();
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
