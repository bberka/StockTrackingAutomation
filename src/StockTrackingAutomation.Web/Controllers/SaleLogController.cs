using Application.Manager;
using Domain.Enums;
using Domain.Helpers;
using Domain.Models;
using EasMe.Extensions;
using EasMe.Logging;
using Microsoft.AspNetCore.Mvc;
using StockTrackingAutomation.Web.Filters;

namespace StockTrackingAutomation.Web.Controllers
{
    [AuthFilter(RoleType.Owner)]
    public class SaleLogController : Controller
    {
        private readonly ISaleLogMgr _saleLogMgr;
        private readonly IProductMgr _productMgr;
        private readonly ICustomerMgr _customerMgr;
        private static readonly IEasLog logger = EasLogFactory.CreateLogger();

        public SaleLogController(
            ISaleLogMgr saleLogMgr,
            IProductMgr productMgr,
            ICustomerMgr customerMgr)
        {
            _saleLogMgr = saleLogMgr;
            _productMgr = productMgr;
            _customerMgr = customerMgr;
        }
        [HttpGet]
        public IActionResult List()
        {
            var list = _saleLogMgr.GetValidList();
            logger.Info("SaleLogList: " + list.Count);
            return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var res = new SaleLogCreateViewModel
            {
                Products = _productMgr.GetValidProducts(),
                Customers = _customerMgr.GetValidCustomers()
            };
            return View(res);
        }
        [HttpPost]
        public IActionResult Create(SaleLogCreateViewModel viewModel)
        {
            viewModel.Products = _productMgr.GetValidProducts();
            viewModel.Customers = _customerMgr.GetValidCustomers();
            var userNo = HttpContext.GetUser().UserNo;
            viewModel.Data.UserId = userNo;
            var res = _saleLogMgr.AddSaleLog(viewModel.Data);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.ErrorCode);
                logger.Warn("SaleLogCreate:" + viewModel.Data.ToJsonString(), res.ToJsonString());
                return View(viewModel);
            }
            logger.Info("SaleLogCreate:" + viewModel.Data.ToJsonString());
            return RedirectToAction("List");
        }
    }
}
