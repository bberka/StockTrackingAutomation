using Application.Manager;
using Domain.Enums;
using Domain.Helpers;
using Domain.Models;
using EasMe;
using EasMe.Extensions;
using EasMe.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockTrackingAutomation.Web.Filters;

namespace StockTrackingAutomation.Web.Controllers
{
    [AuthFilter(RoleType.Owner)]
    public class PurchaseController : Controller
    {
        private readonly IBuyLogMgr _buyLogMgr;
        private readonly ISupplierService _supplierMgr;
        private readonly IProductService _productMgr;
        private static readonly IEasLog logger = EasLogFactory.CreateLogger();

        public PurchaseController(
            IBuyLogMgr buyLogMgr,
            ISupplierService supplierMgr,
            IProductService productMgr)
        {
            _buyLogMgr = buyLogMgr;
            _supplierMgr = supplierMgr;
            _productMgr = productMgr;
        }
        [HttpGet]
        public IActionResult List()
        {
            var list = _buyLogMgr.GetValidList();
            logger.Info("BuyLogList: " + list.Count);
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var res = new BuyLogCreateViewModel
            {
                Products = _productMgr.GetValidProducts(),
                Suppliers = _supplierMgr.GetValidSuppliers()
            };
            return View(res);
        }
        [HttpPost]
        public IActionResult Create(BuyLogCreateViewModel viewModel)
        {
            viewModel.Products = _productMgr.GetValidProducts();
            viewModel.Suppliers = _supplierMgr.GetValidSuppliers();
            var userNo = HttpContext.GetUser().Id;
            viewModel.Data.UserId = userNo;
            var res = _buyLogMgr.AddBuyLog(viewModel.Data);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.ErrorCode);
                logger.Warn("BuyLogCreate:" + viewModel.Data.ToJsonString(), res.ToJsonString());
                return View(viewModel);
            }
            logger.Info("BuyLogCreate:" + viewModel.Data.ToJsonString());
            return RedirectToAction("List");
        }
    }
}
