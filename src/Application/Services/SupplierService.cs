using Domain.Abstract;
using Domain.Entities;
using EasMe.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{


    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SupplierService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
		public ResultData<Supplier> GetSupplier(int id)
		{
			var supplier = _unitOfWork.SupplierRepository.Get(x => x.Id == id )
                .Include(x => x.DebtLogs)
                .Include(x => x.Purchases)
                .FirstOrDefault();
            if (supplier is null) return Result.Warn(1, "Tedarikçi bulunamadı");
			if(supplier.DeletedDate.HasValue) return Result.Warn(2, "Tedarikçi silinmiş");
            return supplier;
        }
		public List<Supplier> GetList()
		{
			return _unitOfWork.SupplierRepository
                .Get(x => !x.DeletedDate.HasValue)
                .Include(x => x.DebtLogs)
                .Include(x => x.Purchases)
                .ToList();
		}
		public Result AddSupplier(Supplier supplier)
		{
			_unitOfWork.SupplierRepository.Add(supplier);
            var res = _unitOfWork.Save();
            if (!res) return Result.Error(1, "DbError");
			return Result.Success();
		}
		public Result RemoveSupplier(int id)
		{
			var exist = _unitOfWork.SupplierRepository.Find(id);
			if (exist is null) return Result.Error(1, "Tedarikçi bulunamadı");
			exist.DeletedDate = DateTime.Now;
			_unitOfWork.SupplierRepository.Update(exist);
            var res = _unitOfWork.Save();
            if (!res) return Result.Error(2, "DbError");
			return Result.Success();
		}
		public Result UpdateSupplier(Supplier supplier)
		{
			var exist = _unitOfWork.SupplierRepository.Find(supplier.Id);
			if (exist is null) return Result.Error(1, "Tedarikçi bulunamadı");
			exist.Name = supplier.Name;
			exist.CompanyName = supplier.CompanyName;
			exist.PhoneNumber = supplier.PhoneNumber;
			exist.EmailAddress = supplier.EmailAddress;
			_unitOfWork.SupplierRepository.Update(exist);
            var res = _unitOfWork.Save();
            if (!res) return Result.Error(2, "DbError");
            return Result.Success();
        }
    }
}
