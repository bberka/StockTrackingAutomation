using Domain.Abstract;
using Domain.Entities;
using EasMe.Models;

namespace Application.Manager
{
	public interface ISupplierMgr
    {
        Supplier? GetValidSupplier(int id);
        List<Supplier> GetValidSuppliers();
        Result AddSupplier(Supplier supplier);
        Result RemoveSupplier(int id);
        Result UpdateSupplier(Supplier supplier);
    }

    public class SupplierMgr : ISupplierMgr
    {
        private readonly IUnitOfWork _unitOfWork;

        public SupplierMgr(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
		public Supplier? GetValidSupplier(int id)
		{
			return _unitOfWork.Suppliers.Find(id);
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
