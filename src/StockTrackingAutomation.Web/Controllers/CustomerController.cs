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
        private readonly ICustomerService _customerService;
        private static readonly IEasLog logger = EasLogFactory.CreateLogger();

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [HttpGet]
        public IActionResult List()
        {
            var list = _customerService.GetValidCustomers();
            logger.Info("Customer list count:" + list.Count);
            return View(list);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var user = _customerService.GetValidCustomer(id);
            logger.Info("Customer details:" + id);
            return View(user.Data);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _customerService.GetValidCustomer(id);
            logger.Info("Customer edit:" + id);
            return View(user.Data);
        }
        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
           
            var res = _customerService.UpdateCustomer(customer);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.ErrorCode);
                logger.Warn("Customer edit:" + customer.Id, res.Rv + res.ErrorCode);
                return View(customer);
            }
            logger.Info("Customer edit:" + customer.Id);
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
            var res = _customerService.AddCustomer(customer);
            if (!res.IsSuccess)
            {
                logger.Warn("customer add:" + customer.Id, res.Rv + res.ErrorCode);
                ModelState.AddModelError("", res.ErrorCode);
                return View(customer);
            }
            logger.Info("customer add:" + customer.Id);
            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var res = _customerService.DeleteCustomer(id);
            if (!res.IsSuccess)
            {
                logger.Warn("Customer delete:" + id, res.Rv + res.ErrorCode);
                ModelState.AddModelError("", res.ErrorCode);
                return RedirectToAction("List");
            }
            logger.Info("Customer delete:" + id);
            return RedirectToAction("List");
        }
    }
}
