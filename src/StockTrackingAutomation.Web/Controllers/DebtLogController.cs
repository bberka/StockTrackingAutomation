using Application.Manager;
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
    public class DebtLogController : Controller
    {
        private static EasLog logger = EasLogFactory.CreateLogger(nameof(DebtLogController));

        [HttpGet]
        public IActionResult List()
        {
            var list = DebtLogMgr.This.GetValidList();
            logger.Info("DebtLog list: " + list.Count);
            return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var res = new DebtLogCreateViewModel
            {
                Customers = CustomerMgr.This.GetValidCustomers()
            };
            return View(res);
        }
        [HttpPost]
        public IActionResult Create(DebtLogCreateViewModel viewModel)
        {
            viewModel.Customers = CustomerMgr.This.GetValidCustomers();
            var userNo = HttpContext.GetUser().UserNo;
            var res = DebtLogMgr.This.AddNewRecord(viewModel.Data, userNo);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.Message);
                logger.Warn("DebtLog create:" + viewModel.Data.ToJsonString(), res.ToJsonString());
                return View(viewModel);
            }
            logger.Info("DebtLog create:" + viewModel.Data.ToJsonString());
            return RedirectToAction("List");
        }
    }
}
