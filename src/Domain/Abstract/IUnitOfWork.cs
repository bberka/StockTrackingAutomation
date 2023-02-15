using Domain.Entities;

namespace Domain.Abstract;

public interface IUnitOfWork : IDisposable
{

    IEntityRepository<BuyLog> BuyLogs { get; }
    IEntityRepository<Customer> Customers { get; }
    IEntityRepository<DebtLog> DebtLogs{ get; }
    IEntityRepository<Product> Products { get; }
    IEntityRepository<SaleLog> SaleLogs { get; }
    IEntityRepository<Supplier> Suppliers { get; }
    IEntityRepository<User> Users { get; }
    bool Save();

}