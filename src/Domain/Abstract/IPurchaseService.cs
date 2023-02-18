using Domain.Entities;
using EasMe.Models;

namespace Domain.Abstract;

public interface IPurchaseService
{
    List<Purchase> GetList();
    List<Purchase> GetSupplierPurchases(int supplierId);
    Result AddBuyLog(Purchase data);
}