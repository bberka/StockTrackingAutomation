using Domain.Entities;

namespace Domain.Abstract;

public interface IUnitOfWork : IDisposable
{

    IEntityRepository<Purchase> Purchases { get; }
    IEntityRepository<Customer> Customers { get; }
    IEntityRepository<DebtLog> DebtLogs{ get; }
    IEntityRepository<Product> Products { get; }
    IEntityRepository<Sale> Sales { get; }
    IEntityRepository<Supplier> Suppliers { get; }
    IEntityRepository<User> Users { get; }
    bool Save();

}