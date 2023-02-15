using Domain.Abstract;
using Domain.Entities;
using EasMe.Models;

namespace Application.Manager
{
    public interface IDebtLogMgr
    {
        Result AddNewRecord(DebtLog log, int id);
        List<DebtLog> GetValidList();
    }

    public class DebtLogMgr : IDebtLogMgr
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerMgr _customerMgr;
        private readonly ISupplierMgr _supplierMgr;

        public DebtLogMgr(
            IUnitOfWork unitOfWork,
            ICustomerMgr customerMgr,
            ISupplierMgr supplierMgr)
        {
            _unitOfWork = unitOfWork;
            _customerMgr = customerMgr;
            _supplierMgr = supplierMgr;
        }
        public Result AddNewRecord(DebtLog log, int id)
        {
            var customerResult = _customerMgr.GetValidCustomer(log.CustomerId.Value);
            if (customerResult.IsFailure)
            {
                return customerResult.ToResult(100);
            }
            
            var customer = customerResult.Data;
            switch (log.Type)
            {
                //Alınan
                case 1:
                    customer.Debt -= log.Money;
                    break;
                //Verilen
                case 2:
                    customer.Debt += log.Money;
                    break;
            }
            _unitOfWork.Customers.Update(customer);
            log.UserId = id;
            _unitOfWork.DebtLogs.Add(log);
            var res = _unitOfWork.Save();
            if (!res)
            {
                return Result.Error(1, "DbError");
            }
            return Result.Success();
        }
        public List<DebtLog> GetValidList()
        {
            var list = _unitOfWork.DebtLogs.GetList();
            var customers = _customerMgr.GetValidCustomers().Select(x => x.Id).ToList();
            var suppliers = _supplierMgr.GetValidSuppliers().Select(x => x.Id).ToList();
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
