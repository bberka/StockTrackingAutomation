﻿using Application.Manager;
using Domain.Entities;
using Domain.Enums;
using EasMe.Extensions;
using EasMe.Logging;
using Microsoft.AspNetCore.Mvc;
using StockTrackingAutomation.Web.Filters;

namespace StockTrackingAutomation.Web.Controllers
{
    [AuthFilter(RoleType.Owner)]
    public class SupplierController : Controller
    {
        private readonly ISupplierMgr _supplierMgr;
        private static readonly IEasLog logger = EasLogFactory.CreateLogger();

        public SupplierController(ISupplierMgr supplierMgr)
        {
            _supplierMgr = supplierMgr;
        }
        [HttpGet]
        public IActionResult List()
        {
            var list = _supplierMgr.GetValidSuppliers();
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
                logger.Warn("Supplier add:" + data.ToJsonString(), res.ToJsonString());
                ModelState.AddModelError("", res.ErrorCode);
                return View(data);
            }
            logger.Info("Supplier add:" + data.ToJsonString());
            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var res = _supplierMgr.RemoveSupplier(id);
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
            var data = _supplierMgr.GetValidSupplier(id);
            logger.Info("Supplier details:" + id);
            return View(data);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var data = _supplierMgr.GetValidSupplier(id);
            logger.Info("Data edit:" + id);
            return View(data);
        }
        [HttpPost]
        public IActionResult Edit(Supplier data)
        {
            var res = _supplierMgr.UpdateSupplier(data);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.ErrorCode);
                logger.Warn("Supplier edit:" + data.ToJsonString(), res.ToJsonString());
                return View(data);
            }
            logger.Info("Supplier edit:" + data.ToJsonString());
            return RedirectToAction("List");
        }
    }
}