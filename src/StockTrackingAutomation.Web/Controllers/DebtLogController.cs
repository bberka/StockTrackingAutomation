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
    public class DebtLogController : Controller
    {
        private readonly IDebtService _debtService;
        private readonly ICustomerService _customerService;
        private readonly ISupplierService _supplierService;
        private static readonly IEasLog logger = EasLogFactory.CreateLogger();

        public DebtLogController(
            IDebtService debtService,
            ICustomerService customerService,
            ISupplierService supplierService)
        {
            _debtService = debtService;
            _customerService = customerService;
            _supplierService = supplierService;
        }
        [HttpGet]
        public IActionResult SupplierDebtLog(int id)
        {
            var list = _debtService.GetSupplierDebtLogs(id);
            return View(list);
        }

        [HttpGet]
        public IActionResult List()
        {
            var list = _debtService.GetValidList();
            return View(list);
        }
        [HttpGet]
        public IActionResult CustomerDebtLogCreate()
        {
            var res = new DebtLogCreateViewModel
            {
                Customers = _customerService.GetValidCustomers()
            };
            return View(res);
        }
        [HttpPost]
        public IActionResult CustomerDebtLogCreate(DebtLogCreateViewModel viewModel)
        {
            viewModel.Customers = _customerService.GetValidCustomers();
            var userNo = HttpContext.GetUser().Id;
            var res = _debtService.AddCustomerDebtLogRecord(viewModel.Data, userNo);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.ErrorCode);
                logger.Warn("CustomerDebtLogCreate", res.Rv + res.ErrorCode);
                return View(viewModel);
            }
            logger.Info("CustomerDebtLogCreate");
            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult SupplierDebtLogCreate()
        {
            var res = new DebtLogCreateViewModel
            {
                Suppliers = _supplierService.GetValidSuppliers()
            };
            return View(res);
        }
        [HttpPost]
        public IActionResult SupplierDebtLogCreate(DebtLogCreateViewModel viewModel)
        {
            viewModel.Suppliers = _supplierService.GetValidSuppliers();
            var userNo = HttpContext.GetUser().Id;
            var res = _debtService.AddSupplierDebtLogRecord(viewModel.Data, userNo);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.ErrorCode);
                logger.Warn("SupplierDebtLogCreate", res.Rv + res.ErrorCode);
                return View(viewModel);
            }
            logger.Info("SupplierDebtLogCreate");
            return RedirectToAction("List");
        }
    }
}
