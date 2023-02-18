using Domain.Entities;
using EasMe.Models;

namespace Domain.Abstract;

public interface IUnitOfWork : IDisposable
{
    IEntityRepository<Purchase> PurchaseRepository { get; }
    IEntityRepository<Customer> CustomerRepository { get; }
    IEntityRepository<DebtLog> DebtLogRepository { get; }
    IEntityRepository<Product> ProductRepository { get; }
    IEntityRepository<Sale> SaleRepository { get; }
    IEntityRepository<Supplier> SupplierRepository { get; }
    IEntityRepository<User> UserRepository { get; }
    bool Save();
    Result SaveResult(ushort rv);

}