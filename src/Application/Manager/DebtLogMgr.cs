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
            var customer = CustomerMgr.This.GetValidCustomer(log.CustomerId);
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
            return DebtLogDAL.This.GetList();
        }
    }
}
