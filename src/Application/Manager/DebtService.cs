using Domain.Abstract;
using Domain.Entities;
using EasMe.Models;

namespace Application.Manager
{
    public interface IDebtService
    {
        Result AddNewRecord(DebtLog log, int id);
        List<DebtLog> GetValidList();
    }

    public class DebtService : IDebtService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerService _customerService;
        private readonly ISupplierService _supplierService;

        public DebtService(
            IUnitOfWork unitOfWork,
            ICustomerService customerService,
            ISupplierService supplierService)
        {
            _unitOfWork = unitOfWork;
            _customerService = customerService;
            _supplierService = supplierService;
        }
        public Result AddNewRecord(DebtLog log, int id)
        {
            var customerResult = _customerService.GetValidCustomer(log.CustomerId.Value);
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
            var customers = _customerService.GetValidCustomers().Select(x => x.Id).ToList();
            var suppliers = _supplierService.GetValidSuppliers().Select(x => x.Id).ToList();
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
