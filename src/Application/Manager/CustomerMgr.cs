﻿using Domain.Abstract;
using Domain.Entities;
using EasMe.Models;

namespace Application.Manager
{
    public interface ICustomerMgr
    {
        List<Customer> GetValidCustomers();
        ResultData<Customer> GetValidCustomer(int id);
        Result UpdateCustomer(Customer customer);
        Result AddCustomer(Customer customer);
        Result DeleteCustomer(int id);
    }

    public class CustomerMgr : ICustomerMgr
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerMgr(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<Customer> GetValidCustomers()
        {
            return _unitOfWork.Customers.GetList(x => !x.DeletedDate.HasValue);
        }
        public ResultData<Customer> GetValidCustomer(int id)
        {
            var customer = _unitOfWork.Customers.Find(id);
            if (customer == null) return Result.Error(1, "Müşteri bulunamadı");
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
            _unitOfWork.Customers.Update(existingCustomer);
            var res = _unitOfWork.Save();
            if (!res)
            {
                return Result.Error(2, "DbError");
            }
            return Result.Success();
        }

        public Result AddCustomer(Customer customer)
        {
            var exist = _unitOfWork.Customers.Any(x => x.CompanyName == customer.CompanyName);
            if (exist)
            {
                return Result.Error(1, "Müşteri şirket zaten ekli");
            }
            _unitOfWork.Customers.Add(customer);
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
            _unitOfWork.Customers.Update(customer);
            var res = _unitOfWork.Save();
            if (!res)
            {
                return Result.Error(2, "DbError");
            }
            return Result.Success();
        }
    }
}
