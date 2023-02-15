using Domain.Abstract;
using Domain.Entities;
using EasMe.EntityFrameworkCore;
using EasMe.EntityFrameworkCore.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private bool _disposed;
    private BusinessDbContext _dbContext;
    public UnitOfWork()
    {
        _disposed = false;
        _dbContext = new BusinessDbContext();
        Purchases = new EntityRepositoryBase<Purchase, BusinessDbContext>(_dbContext);
        Customers = new EntityRepositoryBase<Customer, BusinessDbContext>(_dbContext);
        DebtLogs = new EntityRepositoryBase<DebtLog, BusinessDbContext>(_dbContext);
        Products = new EntityRepositoryBase<Product, BusinessDbContext>(_dbContext);
        Sales = new EntityRepositoryBase<Sale, BusinessDbContext>(_dbContext);
        Suppliers = new EntityRepositoryBase<Supplier, BusinessDbContext>(_dbContext);
        Users = new EntityRepositoryBase<User, BusinessDbContext>(_dbContext);

    }
    public void Dispose()
    {
        if (_disposed) return;
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
        _disposed = true;
    }

    public IEntityRepository<Purchase> Purchases { get; }
    public IEntityRepository<Customer> Customers { get; }
    public IEntityRepository<DebtLog> DebtLogs { get; }
    public IEntityRepository<Product> Products { get; }
    public IEntityRepository<Sale> Sales { get; }
    public IEntityRepository<Supplier> Suppliers { get; }
    public IEntityRepository<User> Users { get; }
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
            //TODO Log Exception Handling message                      
        }
        transaction.Rollback();
        return false;
    }
}