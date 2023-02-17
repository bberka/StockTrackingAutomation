using Domain.Entities;
using EasMe.Models;

namespace Domain.Abstract;

public interface ISaleService
{
    List<Sale> GetValidList();
    Result AddSaleLog(Sale data);
    ResultData<Sale> GetSale(int id);
}