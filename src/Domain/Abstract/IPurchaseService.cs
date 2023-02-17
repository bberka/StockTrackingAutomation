using Domain.Entities;
using EasMe.Models;

namespace Domain.Abstract;

public interface IPurchaseService
{
    List<Purchase> GetValidList();
    Result AddBuyLog(Purchase data);
}