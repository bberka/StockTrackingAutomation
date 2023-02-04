using Application.Manager;
using Domain.Entities;
using Domain.Enums;
using Domain.Helpers;
using Domain.Models;
using EasMe;
using EasMe.Extensions;
using Infrastructure.DAL;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using StockTrackingAutomation.Web.Filters;
using System.Collections.Generic;
using System.Linq;

namespace StockTrackingAutomation.Web.Controllers
{
    [AuthFilter(RoleType.Owner)]
    public class SaleLogController : Controller
    {
        private static EasLog logger = EasLogFactory.CreateLogger(nameof(SaleLogController));

        [HttpGet]
        public IActionResult List()
        {
            var list = SaleLogMgr.This.GetValidList();
            logger.Info("SaleLogList: " + list.Count);
            return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var res = new SaleLogCreateViewModel
            {
                Products = ProductMgr.This.GetValidProducts(),
                Customers = CustomerMgr.This.GetValidCustomers()
            };
            return View(res);
        }
        [HttpPost]
        public IActionResult Create(SaleLogCreateViewModel viewModel)
        {
            viewModel.Products = ProductMgr.This.GetValidProducts();
            viewModel.Customers = CustomerMgr.This.GetValidCustomers();
            var userNo = HttpContext.GetUser().UserNo;
            viewModel.Data.UserId = userNo;
            var res = SaleLogMgr.This.AddSaleLog(viewModel.Data);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.Message);
                logger.Warn("SaleLogCreate:" + viewModel.Data.ToJsonString(), res.ToJsonString());
                return View(viewModel);
            }
            logger.Info("SaleLogCreate:" + viewModel.Data.ToJsonString());
            return RedirectToAction("List");
        }
    }
}
