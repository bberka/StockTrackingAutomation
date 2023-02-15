﻿using Domain.Abstract;
using Domain.Entities;
using EasMe.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Manager
{
	public interface ISupplierService
    {
        ResultData<Supplier> GetValidSupplier(int id);
        List<Supplier> GetValidSuppliers();
        Result AddSupplier(Supplier supplier);
        Result RemoveSupplier(int id);
        Result UpdateSupplier(Supplier supplier);
    }

    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SupplierService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
		public ResultData<Supplier> GetValidSupplier(int id)
		{
			var supplier = _unitOfWork.Suppliers.Get(x => x.Id == id )
                .Include(x => x.DebtLogs)
                .Include(x => x.BuyLogs)
                .FirstOrDefault();
            if (supplier is null) return Result.Warn(1, "Tedarikçi bulunamadı");
			if(supplier.DeletedDate.HasValue) return Result.Warn(2, "Tedarikçi silinmiş");
            return supplier;
        }
		public List<Supplier> GetValidSuppliers()
		{
			return _unitOfWork.Suppliers.GetList(x => !x.DeletedDate.HasValue);
		}
		public Result AddSupplier(Supplier supplier)
		{
			_unitOfWork.Suppliers.Add(supplier);
            var res = _unitOfWork.Save();
            if (!res) return Result.Error(1, "DbError");
			return Result.Success();
		}
		public Result RemoveSupplier(int id)
		{
			var exist = _unitOfWork.Suppliers.Find(id);
			if (exist is null) return Result.Error(1, "Tedarikçi bulunamadı");
			exist.DeletedDate = DateTime.Now;
			_unitOfWork.Suppliers.Update(exist);
            var res = _unitOfWork.Save();
            if (!res) return Result.Error(2, "DbError");
			return Result.Success();
		}
		public Result UpdateSupplier(Supplier supplier)
		{
			var exist = _unitOfWork.Suppliers.Find(supplier.Id);
			if (exist is null) return Result.Error(1, "Tedarikçi bulunamadı");
			exist.Name = supplier.Name;
			exist.CompanyName = supplier.CompanyName;
			exist.PhoneNumber = supplier.PhoneNumber;
			exist.EmailAddress = supplier.EmailAddress;
			_unitOfWork.Suppliers.Update(exist);
            var res = _unitOfWork.Save();
            if (!res) return Result.Error(2, "DbError");
            return Result.Success();
        }
    }
}