using Domain.Abstract;
using Domain.Entities;
using EasMe.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Manager
{
    public interface ICustomerService
    {
        List<Customer> GetValidCustomers();
        ResultData<Customer> GetValidCustomer(int id);
        Result UpdateCustomer(Customer customer);
        Result AddCustomer(Customer customer);
        Result DeleteCustomer(int id);
    }

    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<Customer> GetValidCustomers()
        {
            return _unitOfWork.CustomerRepository
                .Get(x => !x.DeletedDate.HasValue)
                .Include(x => x.DebtLogs)
                .ToList();
        }
        public ResultData<Customer> GetValidCustomer(int id)
        {
            var customer = _unitOfWork.CustomerRepository
                .Get(x => x.Id == id)
                .Include(x => x.DebtLogs)
                .FirstOrDefault();
            if (customer is null) return Result.Error(1, "Müşteri bulunamadı");
            if (customer.DeletedDate.HasValue) return Result.Error(2, "Müşteri silinmiş");
            return customer;
        }

        public Result UpdateCustomer(Customer customer)
        {
            var customerResult = GetValidCustomer(customer.Id);
            if (customerResult.IsFailure)
            {
                return customerResult.ToResult(100);
            }

            var existingCustomer = customerResult.Data;
            existingCustomer.PhoneNumber = customer.PhoneNumber;
            existingCustomer.CompanyName = customer.CompanyName;
            existingCustomer.EmailAddress = customer.EmailAddress;
            existingCustomer.Name = customer.Name;
            _unitOfWork.CustomerRepository.Update(existingCustomer);
            var res = _unitOfWork.Save();
            if (!res)
            {
                return Result.Error(2, "DbError");
            }
            return Result.Success();
        }

        public Result AddCustomer(Customer customer)
        {
            var exist = _unitOfWork.CustomerRepository.Any(x => x.CompanyName == customer.CompanyName);
            if (exist)
            {
                return Result.Error(1, "Müşteri şirket zaten ekli");
            }
            _unitOfWork.CustomerRepository.Add(customer);
            var res = _unitOfWork.Save();
            if (!res)

            {
                return Result.Error(2, "DbError");
            }
            return Result.Success();
        }

        public Result DeleteCustomer(int id)
        {
            var customerResult = GetValidCustomer(id);
            if (customerResult.IsFailure)
            {
                return customerResult.ToResult(100);
            }
            var customer = customerResult.Data;
            customer.DeletedDate = DateTime.Now;
            _unitOfWork.CustomerRepository.Update(customer);
            var res = _unitOfWork.Save();
            if (!res)
            {
                return Result.Error(2, "DbError");
            }
            return Result.Success();
        }
    }
}
