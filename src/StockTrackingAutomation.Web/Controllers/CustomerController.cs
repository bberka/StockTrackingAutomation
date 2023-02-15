using Application.Manager;
using Domain.Entities;
using Domain.Enums;
using EasMe.Extensions;
using EasMe.Logging;
using Microsoft.AspNetCore.Mvc;
using StockTrackingAutomation.Web.Filters;

namespace StockTrackingAutomation.Web.Controllers
{
    [AuthFilter(RoleType.Owner)]
    public class CustomerController : Controller
    {
        private readonly ICustomerMgr _customerMgr;
        private static readonly IEasLog logger = EasLogFactory.CreateLogger();

        public CustomerController(ICustomerMgr customerMgr)
        {
            _customerMgr = customerMgr;
        }
        [HttpGet]
        public IActionResult List()
        {
            var list = _customerMgr.GetValidCustomers();
            logger.Info("Customer list count:" + list.Count);
            return View(list);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var user = _customerMgr.GetValidCustomer(id);
            logger.Info("Customer details:" + id);
            return View(user);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _customerMgr.GetValidCustomer(id);
            logger.Info("Customer edit:" + id);
            return View(user);
        }
        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
           
            var res = _customerMgr.UpdateCustomer(customer);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.ErrorCode);
                logger.Warn("Customer edit:" + customer.ToJsonString(),res.ToJsonString());
                return View(customer);
            }
            logger.Info("Customer edit:" + customer.ToJsonString());
            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            var res = _customerMgr.AddCustomer(customer);
            if (!res.IsSuccess)
            {
                logger.Warn("customer add:" + customer.ToJsonString(), res.ToJsonString());
                ModelState.AddModelError("", res.ErrorCode);
                return View(customer);
            }
            logger.Info("customer add:" + customer.ToJsonString());
            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var res = _customerMgr.DeleteCustomer(id);
            if (!res.IsSuccess)
            {
                logger.Warn("Customer delete:" + id, res.ToJsonString());
                ModelState.AddModelError("", res.ErrorCode);
                return RedirectToAction("List");
            }
            logger.Info("Customer delete:" + id);
            return RedirectToAction("List");
        }
    }
}
