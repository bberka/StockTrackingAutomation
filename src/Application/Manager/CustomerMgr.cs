using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Manager
{
    public class CustomerMgr
    {
        private CustomerMgr() { }
        public static CustomerMgr This
        {
            get
            {
                Instance ??= new();
                return Instance;
            }
        }
        private static CustomerMgr? Instance;

        public List<Customer> GetValidCustomers()
        {
            return CustomerDAL.This.GetList(x => !x.DeletedDate.HasValue);
        }
        public Customer GetValidCustomer(int id)
        {
            var customer = CustomerDAL.This.Find(id);
            if (customer == null) throw new NullReferenceException("Customer is NULL");
            return customer;
        }

        public Result UpdateCustomer(Customer customer)
        {
            var exist = GetValidCustomer(customer.CustomerNo);
            if (exist == null)
            {
                return Result.Error(1, "Müşteri bulunamadı");
            }
            exist.PhoneNumber = customer.PhoneNumber;
            exist.CompanyName = customer.CompanyName;
            exist.EmailAddress = customer.EmailAddress;
            exist.Name = customer.Name;
            var res = CustomerDAL.This.Update(exist);
            if (!res)
            {
                return Result.Error(2, "DbError");
            }
            return Result.Success();
        }

        public Result AddCustomer(Customer customer)
        {
            var exist = CustomerDAL.This.Any(x => x.CompanyName == customer.CompanyName);
            if (exist)
            {
                return Result.Error(1, "Müşteri şirket zaten ekli");
            }
            var res = CustomerDAL.This.Add(customer);
            if (!res)

            {
                return Result.Error(2, "DbError");
            }
            return Result.Success();
        }

        public Result DeleteCustomer(int id)
        {
            var current = GetValidCustomer(id);
            if (current is null)
            {
                return Result.Error(1, "Müşteri bulunamadı");
            }
            current.DeletedDate = DateTime.Now;
            var res = CustomerDAL.This.Update(current);
            if (!res) { 

                return Result.Error(2, "DbError");
        }
            return Result.Success();
        }
}
}
