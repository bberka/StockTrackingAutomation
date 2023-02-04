using Application.Manager;
using Domain.Enums;
using Domain.Helpers;
using Domain.Models;
using EasMe;
using EasMe.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockTrackingAutomation.Web.Filters;

namespace StockTrackingAutomation.Web.Controllers
{
    [AuthFilter(RoleType.Owner)]
    public class BuyLogController : Controller
    {
        private static EasLog logger = EasLogFactory.CreateLogger(nameof(BuyLogController));
        [HttpGet]
        public IActionResult List()
        {
            var list = BuyLogMgr.This.GetValidList();
            logger.Info("BuyLogList: " + list.Count);
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var res = new BuyLogCreateViewModel
            {
                Products = ProductMgr.This.GetValidProducts(),
                Suppliers = SupplierMgr.This.GetValidSuppliers()
            };
            return View(res);
        }
        [HttpPost]
        public IActionResult Create(BuyLogCreateViewModel viewModel)
        {
            viewModel.Products = ProductMgr.This.GetValidProducts();
            viewModel.Suppliers = SupplierMgr.This.GetValidSuppliers();
            var userNo = HttpContext.GetUser().UserNo;
            viewModel.Data.UserId = userNo;
            var res = BuyLogMgr.This.AddBuyLog(viewModel.Data);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.Message);
                logger.Warn("BuyLogCreate:" + viewModel.Data.ToJsonString(), res.ToJsonString());
                return View(viewModel);
            }
            logger.Info("BuyLogCreate:" + viewModel.Data.ToJsonString());
            return RedirectToAction("List");
        }
    }
}
