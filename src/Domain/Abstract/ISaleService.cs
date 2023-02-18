using Domain.Entities;
using EasMe.Models;

namespace Domain.Abstract;

public interface ISaleService
{
    List<Sale> GetList();
    List<Sale> GetCustomerSales(int customerId);
    Result AddSaleLog(Sale data);

}