using Domain.Entities;
using EasMe.Models;

namespace Domain.Abstract;

public interface ICustomerService
{
    List<Customer> GetCustomers();
    ResultData<Customer> GetCustomer(int id);
    Result UpdateCustomer(Customer customer);
    Result AddCustomer(Customer customer);
    Result DeleteCustomer(int id);
}