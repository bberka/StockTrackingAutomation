using Domain.Abstract;
using Domain.Entities;
using EasMe.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{


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
   
        public Result AddCustomerDebtLogRecord(DebtLog log, int authorUserId)
        {
            var customerResult = _customerService.GetCustomer(log.CustomerId.Value);
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
                ////Verilen
                //case 2:
                //    customer.Debt += log.Money;
                //    break;
                default:
                    throw new InvalidOperationException();
            }
            _unitOfWork.CustomerRepository.Update(customer);
            log.UserId = authorUserId;
            _unitOfWork.DebtLogRepository.Add(log);
            var res = _unitOfWork.Save();
            if (!res)
            {
                return Result.Error(1, "DbError");
            }
            return Result.Success();
            throw new NotImplementedException();
        }

        public Result AddSupplierDebtLogRecord(DebtLog log, int authorUserId)
        {
            var supplierResult = _supplierService.GetSupplier(log.SupplierId.Value);
            if (supplierResult.IsFailure)
            {
                return supplierResult.ToResult(100);
            }

            var supplier = supplierResult.Data;
            switch (log.Type)
            {
                //Alınan
                //case 1:
                //    supplier.Debt -= log.Money;
                //    break;
                //Verilen
                case 2:
                    supplier.Debt -= log.Money;
                    break;
                default:
                    throw new InvalidOperationException();
            }
            _unitOfWork.SupplierRepository.Update(supplier);
            log.UserId = authorUserId;
            _unitOfWork.DebtLogRepository.Add(log);
            var res = _unitOfWork.Save();
            if (!res)
            {
                return Result.Error(1, "DbError");
            }
            return Result.Success();
        }

        public List<DebtLog> GetList()
        {
            var list = _unitOfWork.DebtLogRepository
                .Get()
                .Include(x => x.Supplier)
                .Include(x => x.Customer)
                .Include(x => x.User)
                .ToList();
            var customers = _customerService
                .GetCustomers()
                .Select(x => x.Id)
                .ToList();
            var suppliers = _supplierService
                .GetList()
                .Select(x => x.Id)
                .ToList();
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

        public List<DebtLog> GetSupplierDebtLogs(int supplierId)
        {
            var isValid = _unitOfWork.SupplierRepository.Any(x => !x.DeletedDate.HasValue && x.Id == supplierId);
            if (!isValid) return new();
            var list = _unitOfWork.DebtLogRepository
                .Get(x => x.SupplierId == supplierId)
                .Include(x => x.Supplier)
                .Include(x => x.Customer)
                .Include(x => x.User)
                .ToList();
            return list;
        }

        public List<DebtLog> GetCustomerDebtLogs(int customerId)
        {
            var isValid = _unitOfWork.CustomerRepository.Any(x => !x.DeletedDate.HasValue && x.Id == customerId);
            if (!isValid) return new();
            var list = _unitOfWork.DebtLogRepository
                .Get(x => x.CustomerId == customerId)
                .Include(x => x.Supplier)
                .Include(x => x.Customer)
                .Include(x => x.User)
                .ToList();
            return list;
        }
    }
}
