using Application.Caching;
using Application.Manager;
using Domain.Entities;
using Domain.Enums;
using EasMe;
using EasMe.Extensions;
using Microsoft.AspNetCore.Mvc;
using StockTrackingAutomation.Web.Filters;

namespace StockTrackingAutomation.Web.Controllers
{
    [AuthFilter(RoleType.Owner)]
    public class SupplierController : Controller
    {
        private static EasLog logger = EasLogFactory.CreateLogger(nameof(SupplierController));

        [HttpGet]
        public IActionResult List()
        {
            DbCache.SupplierCache.Refresh();
            var list = DbCache.SupplierCache.Get();
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
            var res = SupplierMgr.This.AddSupplier(data);
            if (!res.IsSuccess)
            {
                logger.Warn("Supplier add:" + data.ToJsonString(), res.ToJsonString());
                ModelState.AddModelError("", res.Message);
                return View(data);
            }
            logger.Info("Supplier add:" + data.ToJsonString());
            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var res = SupplierMgr.This.RemoveSupplier(id);
            if (!res.IsSuccess)
            {
                logger.Warn("Supplier delete:" + id, res.ToJsonString());
                return RedirectToAction("List");
            }
            logger.Info("Supplier delete:" + id);
            return RedirectToAction("List");
        }




        [HttpGet]
        public IActionResult Details(int id)
        {
            var data = SupplierMgr.This.GetValidSupplier(id);
            logger.Info("Supplier details:" + id);
            return View(data);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var data = SupplierMgr.This.GetValidSupplier(id);
            logger.Info("Data edit:" + id);
            return View(data);
        }
        [HttpPost]
        public IActionResult Edit(Supplier data)
        {
            var res = SupplierMgr.This.UpdateSupplier(data);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.Message);
                logger.Warn("Supplier edit:" + data.ToJsonString(), res.ToJsonString());
                return View(data);
            }
            logger.Info("Supplier edit:" + data.ToJsonString());
            return RedirectToAction("List");
        }
    }
}
