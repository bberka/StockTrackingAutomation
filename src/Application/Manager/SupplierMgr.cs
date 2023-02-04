using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Manager
{
    public class SupplierMgr
    {

		private SupplierMgr() { }
		public static SupplierMgr This
		{
			get
			{
				Instance ??= new();
				return Instance;
			}
		}
		private static SupplierMgr? Instance;

		public Supplier? GetValidSupplier(int id)
		{
			return SupplierDAL.This.Find(id);
		}
		public List<Supplier> GetValidSuppliers()
		{
			return SupplierDAL.This.GetList(x => !x.DeletedDate.HasValue);
		}
		public Result AddSupplier(Supplier supplier)
		{
			var res = SupplierDAL.This.Add(supplier);
			if (!res) return Result.Error(1, "DbError");
			return Result.Success();
		}
		public Result RemoveSupplier(int id)
		{
			var exist = SupplierDAL.This.Find(id);
			if (exist is null) return Result.Error(1, "Tedarikçi bulunamadı");
			exist.DeletedDate = DateTime.Now;
			var res = SupplierDAL.This.Update(exist);
			if (!res) return Result.Error(2, "DbError");
			return Result.Success();
		}
		public Result UpdateSupplier(Supplier supplier)
		{
			var exist = SupplierDAL.This.Find(supplier.SupplierNo);
			if (exist is null) return Result.Error(1, "Tedarikçi bulunamadı");
			exist.Name = supplier.Name;
			exist.CompanyName = supplier.CompanyName;
			exist.PhoneNumber = supplier.PhoneNumber;
			exist.EmailAddress = supplier.EmailAddress;
			var res = SupplierDAL.This.Update(exist);
            if (!res) return Result.Error(2, "DbError");
            return Result.Success();
        }
    }
}
