using Domain.Entities;
using EasMe.Models;

namespace Domain.Abstract;

public interface ICustomerService
{
    List<Customer> GetValidCustomers();
    ResultData<Customer> GetValidCustomer(int id);
    Result UpdateCustomer(Customer customer);
    Result AddCustomer(Customer customer);
    Result DeleteCustomer(int id);
}