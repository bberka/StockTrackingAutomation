using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Concrete;
using Infrastructure.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Manager
{
    public class DebtLogMgr
    {
        private DebtLogMgr() { }
        public static DebtLogMgr This
        {
            get
            {
                Instance ??= new();
                return Instance;
            }
        }
        private static DebtLogMgr? Instance;

        public Result AddNewRecord(DebtLog log, int id)
        {

            var customer = CustomerMgr.This.GetValidCustomer(log.CustomerId.Value);
            if (customer == null)
            {
                return Result.Error(1, "Müşteri bulunamadı");
            }
            if (log.Type == 1) //Alınan
            {
                customer.Debt -= log.Money;
            }
            if (log.Type == 2) //Verilen
            {
                customer.Debt += log.Money;
            }
            var customerResult = CustomerDAL.This.Update(customer);
            if (!customerResult)
            {
                return Result.Error(1, "DbError");
            }
            log.UserId = id;
            var res = DebtLogDAL.This.Add(log);
            if (!res)
            {
                return Result.Error(1, "DbError");
            }
            return Result.Success();
        }
        public List<DebtLog> GetValidList()
        {
            var list = DebtLogDAL.This.GetList();
            var customers = CustomerMgr.This.GetValidCustomers().Select(x => x.CustomerNo).ToList();
            var suppliers = SupplierMgr.This.GetValidSuppliers().Select(x => x.SupplierNo).ToList();
            foreach(var item in list)
            {
                if(item.CustomerId != null)
                {
                    if (!customers.Contains(item.CustomerId.Value))
                    {
                        list.Remove(item);
                    }
                }
                if(item.SupplierId != null)
                {
                    if (!suppliers.Contains(item.SupplierId.Value))
                    {
                        list.Remove(item);
                    }
                }
            }
            return list;
        }
    }
}
