using Domain.Entities;
using EasMe.Models;

namespace Domain.Abstract;

public interface IPurchaseService
{
    List<Purchase> GetValidList();
    List<Purchase> GetSupplierPurchases(int supplierId);
    Result AddBuyLog(Purchase data);
}