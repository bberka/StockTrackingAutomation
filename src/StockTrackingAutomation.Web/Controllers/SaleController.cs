using Application.Services;
using Domain.Abstract;
using Domain.Enums;
using Domain.Helpers;
using Domain.Models;
using EasMe.Logging;
using Microsoft.AspNetCore.Mvc;
using StockTrackingAutomation.Web.Filters;

namespace StockTrackingAutomation.Web.Controllers
{
    [AuthFilter(RoleType.Owner)]
    public class SaleController : Controller
    {
        private readonly ISaleService _saleService;
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;

        private static readonly IEasLog logger = EasLogFactory.CreateLogger();

        public SaleController(
            ISaleService saleService,
            IProductService productService,
            ICustomerService customerService)
        {
            _saleService = saleService;
            _productService = productService;
            _customerService = customerService;
        }
        [HttpGet]
        public IActionResult CustomerSales(int id)
        {
            var list = _saleService.GetCustomerSales(id);
            return View(list);
        }

        [HttpGet]
        public IActionResult List()
        {
            var list = _saleService.GetList();
            logger.Info("SaleLogList: " + list.Count);
            return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var res = new SaleLogCreateViewModel
            {
                Products = _productService.GetList(),
                Customers = _customerService.GetCustomers()
            };
            return View(res);
        }
        [HttpPost]
        public IActionResult Create(SaleLogCreateViewModel viewModel)
        {
            viewModel.Products = _productService.GetList();
            viewModel.Customers = _customerService.GetCustomers();
            var userNo = HttpContext.GetUser().Id;
            viewModel.Data.UserId = userNo;
            var res = _saleService.AddSaleLog(viewModel.Data);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.ErrorCode);
                logger.Warn("SaleLogCreate", res.Rv + res.ErrorCode);
                return View(viewModel);
            }
            logger.Info("SaleLogCreate");
            return RedirectToAction("List");
        }
    }
}
