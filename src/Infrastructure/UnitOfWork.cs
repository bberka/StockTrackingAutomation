using Domain.Abstract;
using Domain.Entities;
using EasMe.EntityFrameworkCore.V1;
using EasMe.Logging;
using Infrastructure.DAL;

namespace Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private bool _disposed;
    private BusinessDbContext _dbContext;
    private static readonly IEasLog logger = EasLogFactory.CreateLogger();
    public UnitOfWork()
    {
        _disposed = false;
        _dbContext = new BusinessDbContext();
        PurchaseRepository = new PurchaseRepository(_dbContext);
        CustomerRepository = new CustomerRepository(_dbContext);
        DebtLogRepository = new DebtLogRepository(_dbContext);
        ProductRepository = new ProductRepository(_dbContext);
        SaleRepository = new SaleRepository(_dbContext);
        SupplierRepository = new SupplierRepository(_dbContext);
        UserRepository = new UserRepository(_dbContext);
    }

    public void Dispose()
    {
        if (_disposed) return;
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
        _disposed = true;
    }

    public IEntityRepository<Purchase> PurchaseRepository { get; }
    public IEntityRepository<Customer> CustomerRepository { get; }
    public IEntityRepository<DebtLog> DebtLogRepository { get; }
    public IEntityRepository<Product> ProductRepository { get; }
    public IEntityRepository<Sale> SaleRepository { get; }
    public IEntityRepository<Supplier> SupplierRepository { get; }
    public IEntityRepository<User> UserRepository { get; }
    public bool Save()
    {
        using var transaction = _dbContext.Database.BeginTransaction();
        try
        {
            var affected = _dbContext.SaveChanges();
            if (affected > 0)
            {
                transaction.Commit();
                return true;
            }
        }
        catch (Exception ex)
        {
            logger.Exception(ex,"InternalDbError");
        }
        transaction.Rollback();
        return false;
    }
}