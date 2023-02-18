using Domain.Abstract;
using Domain.Entities;
using EasMe.EntityFrameworkCore.V1;
using EasMe.Enums;
using EasMe.Extensions;
using EasMe.Logging;
using EasMe.Models;
using Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

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
        var entityNames = DetectChangedProperties();
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
            if (!entityNames.IsNullOrEmpty())
            {
                logger.Fatal(ex, "InternalDbError", entityNames);
            }
            else
            {
                logger.Fatal(ex, "InternalDbError");
            }
        }
        transaction.Rollback();
        return false;
    }

    public Result SaveResult(ushort rv)
    {
        var result = _dbContext.SaveChanges() > 0;
        if (result) rv = 0;
        return new Result(ResultSeverity.Fatal, rv, "InternalDbError");
    }
    /// <summary>
    /// Detects changed entities and properties and inserts it to <see cref="ChangeLogRepository"/> before saving database
    /// </summary>
    /// <returns>Changed entity names</returns>
    private string DetectChangedProperties()
    {
        var modifiedEntities = _dbContext.ChangeTracker
            .Entries()
            .Where(p => p.State == EntityState.Modified)
            .ToList();

        var entityNames = "";
        //var logs = new List<ChangeLog>();
        foreach (var change in modifiedEntities)
        {
            var entityName = change.Entity.GetType().Name;
            entityNames += entityName + ",";
            foreach (var prop in change.OriginalValues.Properties)
            {
                var originalValue = change.OriginalValues[prop]?.ToString();
                var currentValue = change.CurrentValues[prop]?.ToString();
                if (originalValue == currentValue) continue;
                //ChangeLog log = new ChangeLog()
                //{
                //    EntityName = entityName,
                //    PropertyName = prop.Name,
                //    OldValue = originalValue,
                //    NewValue = currentValue,
                //};
                //logs.Add(log);
            }
        }
        //ChangeLogRepository.InsertRange(logs);
        return entityNames;
    }
}