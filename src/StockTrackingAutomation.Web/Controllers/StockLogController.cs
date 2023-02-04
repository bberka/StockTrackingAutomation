﻿using Application.Manager;
using Domain.Entities;
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
    [AuthFilter]
    public class StockLogController : Controller
    {
        private static EasLog logger = EasLogFactory.CreateLogger(nameof(StockLogController));

        [HttpGet]
        public IActionResult List()
        {
            var list = StockLogMgr.This.GetValidList();
            logger.Info("StockLog list: " + list.Count);
            return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var res = new StockLogCreateViewModel
            {
                Products = ProductMgr.This.GetValidProducts(),
                Customers = CustomerMgr.This.GetValidCustomers()
            };
            return View(res);
        }
        [HttpPost]
        public IActionResult Create(StockLogCreateViewModel viewModel)
        {
            viewModel.Products = ProductMgr.This.GetValidProducts();
            viewModel.Customers = CustomerMgr.This.GetValidCustomers();
            var userNo = HttpContext.GetUser().UserNo;
            var res = StockLogMgr.This.AddStockLog(viewModel.Data, userNo);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.Message);
                logger.Warn("Stock log create:" + viewModel.Data.ToJsonString(), res.ToJsonString());
                return View(viewModel);
            }
            logger.Info("Stock log create:" + viewModel.Data.ToJsonString());
            return RedirectToAction("List");
        }
    }
}
