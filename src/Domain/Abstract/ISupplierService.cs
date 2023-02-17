using Domain.Entities;
using EasMe.Models;

namespace Domain.Abstract;

public interface ISupplierService
{
    ResultData<Supplier> GetValidSupplier(int id);
    List<Supplier> GetValidSuppliers();
    Result AddSupplier(Supplier supplier);
    Result RemoveSupplier(int id);
    Result UpdateSupplier(Supplier supplier);
}