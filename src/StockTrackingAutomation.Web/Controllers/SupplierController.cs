using Domain.Abstract;
using Domain.Entities;
using Domain.Enums;
using EasMe.Logging;
using Microsoft.AspNetCore.Mvc;
using StockTrackingAutomation.Web.Filters;

namespace StockTrackingAutomation.Web.Controllers
{
    [AuthFilter(RoleType.Owner)]
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierMgr;
        private static readonly IEasLog logger = EasLogFactory.CreateLogger();

        public SupplierController(ISupplierService supplierMgr)
        {
            _supplierMgr = supplierMgr;
        }
        [HttpGet]
        public IActionResult List()
        {
            var list = _supplierMgr.GetList();
            logger.Info("Supplier count:" + list.Count);
            return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Supplier data)
        {
            var res = _supplierMgr.AddSupplier(data);
            if (!res.IsSuccess)
            {
                logger.Warn("Supplier add:" + data.EmailAddress, res.Rv + res.ErrorCode);
                ModelState.AddModelError("", res.ErrorCode);
                return View(data);
            }
            logger.Info("Supplier add:" + data.EmailAddress, res.Rv + res.ErrorCode);
            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var res = _supplierMgr.RemoveSupplier(id);
            if (!res.IsSuccess)
            {
                logger.Warn("Supplier delete:" + id, res.Rv + res.ErrorCode);
                return RedirectToAction("List");
            }
            logger.Info("Supplier delete:" + id);
            return RedirectToAction("List");
        }




        [HttpGet]
        public IActionResult Details(int id)
        {
            var data = _supplierMgr.GetSupplier(id);
            logger.Info("Supplier details:" + id);
            return View(data.Data);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var data = _supplierMgr.GetSupplier(id);
            logger.Info("Data edit:" + id);
            return View(data.Data);
        }
        [HttpPost]
        public IActionResult Edit(Supplier data)
        {
            var res = _supplierMgr.UpdateSupplier(data);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.ErrorCode);
                logger.Warn("Supplier edit:" + data.Id, res.Rv + res.ErrorCode);
                return View(data);
            }
            logger.Info("Supplier edit:" + data.Id);
            return RedirectToAction("List");
        }
    }
}
