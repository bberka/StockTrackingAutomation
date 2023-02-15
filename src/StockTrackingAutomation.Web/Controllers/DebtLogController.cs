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
        private readonly IDebtLogMgr _debtLogMgr;
        private readonly ICustomerMgr _customerMgr;
        private static readonly IEasLog logger = EasLogFactory.CreateLogger();

        public DebtLogController(
            IDebtLogMgr debtLogMgr,
            ICustomerMgr customerMgr)
        {
            _debtLogMgr = debtLogMgr;
            _customerMgr = customerMgr;
        }
        [HttpGet]
        public IActionResult List()
        {
            var list = _debtLogMgr.GetValidList();
            logger.Info("DebtLogList: " + list.Count);
            return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var res = new DebtLogCreateViewModel
            {
                Customers = _customerMgr.GetValidCustomers()
            };
            return View(res);
        }
        [HttpPost]
        public IActionResult Create(DebtLogCreateViewModel viewModel)
        {
            viewModel.Customers = _customerMgr.GetValidCustomers();
            var userNo = HttpContext.GetUser().UserNo;
            var res = _debtLogMgr.AddNewRecord(viewModel.Data, userNo);
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
