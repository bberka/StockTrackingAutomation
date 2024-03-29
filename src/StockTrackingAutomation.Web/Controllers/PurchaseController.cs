﻿using Domain.Abstract;
using Domain.Enums;
using Domain.Helpers;
using Domain.Models;
using EasMe.Logging;
using Microsoft.AspNetCore.Mvc;
using StockTrackingAutomation.Web.Filters;

namespace StockTrackingAutomation.Web.Controllers
{
    [AuthFilter(RoleType.Owner)]
    public class PurchaseController : Controller
    {
        private readonly IPurchaseService _purchaseService;
        private readonly ISupplierService _supplierMgr;
        private readonly IProductService _productMgr;
        private static readonly IEasLog logger = EasLogFactory.CreateLogger();

        public PurchaseController(
            IPurchaseService purchaseService,
            ISupplierService supplierMgr,
            IProductService productMgr)
        {
            _purchaseService = purchaseService;
            _supplierMgr = supplierMgr;
            _productMgr = productMgr;
        }
        [HttpGet]
        public IActionResult SupplierPurchases(int id)
        {
            var list = _purchaseService.GetSupplierPurchases(id);
            return View(list);
        }
        [HttpGet]
        public IActionResult List()
        {
            var list = _purchaseService.GetList();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var res = new BuyLogCreateViewModel
            {
                Products = _productMgr.GetList(),
                Suppliers = _supplierMgr.GetList()
            };
            return View(res);
        }
        [HttpPost]
        public IActionResult Create(BuyLogCreateViewModel viewModel)
        {
            viewModel.Products = _productMgr.GetList();
            viewModel.Suppliers = _supplierMgr.GetList();
            var userNo = HttpContext.GetUser().Id;
            viewModel.Data.UserId = userNo;
            var res = _purchaseService.AddBuyLog(viewModel.Data);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.ErrorCode);
                logger.Warn("BuyLogCreate", res.Rv + res.ErrorCode);
                return View(viewModel);
            }
            logger.Info("BuyLogCreate");
            return RedirectToAction("List");
        }
    }
}
