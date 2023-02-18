using Domain.Entities;
using EasMe.Models;

namespace Domain.Abstract;

public interface ISupplierService
{
    ResultData<Supplier> GetSupplier(int id);
    List<Supplier> GetList();
    Result AddSupplier(Supplier supplier);
    Result RemoveSupplier(int id);
    Result UpdateSupplier(Supplier supplier);
}