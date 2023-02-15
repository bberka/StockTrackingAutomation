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
    public class DebtLogController : Controller
    {
        private readonly IDebtService _debtService;
        private readonly ICustomerService _customerService;
        private static readonly IEasLog logger = EasLogFactory.CreateLogger();

        public DebtLogController(
            IDebtService debtService,
            ICustomerService customerService
            )
        {
            _debtService = debtService;
            _customerService = customerService;
        }
        [HttpGet]
        public IActionResult List()
        {
            var list = _debtService.GetValidList();
            logger.Info("DebtLogList: " + list.Count);
            return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var res = new DebtLogCreateViewModel
            {
                Customers = _customerService.GetValidCustomers()
            };
            return View(res);
        }
        [HttpPost]
        public IActionResult Create(DebtLogCreateViewModel viewModel)
        {
            viewModel.Customers = _customerService.GetValidCustomers();
            var userNo = HttpContext.GetUser().Id;
            var res = _debtService.AddNewRecord(viewModel.Data, userNo);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.ErrorCode);
                logger.Warn("DebtLogCreate:" + viewModel.Data.ToJsonString(), res.ToJsonString());
                return View(viewModel);
            }
            logger.Info("DebtLogCreate:" + viewModel.Data.ToJsonString());
            return RedirectToAction("List");
        }
    }
}
