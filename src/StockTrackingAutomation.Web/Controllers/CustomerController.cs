using Application.Caching;
using Application.Manager;
using Domain.Entities;
using Domain.Enums;
using EasMe;
using EasMe.Extensions;
using Infrastructure.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockTrackingAutomation.Web.Filters;

namespace StockTrackingAutomation.Web.Controllers
{
    [AuthFilter(RoleType.Owner)]
    public class CustomerController : Controller
    {
        private static EasLog logger = EasLogFactory.CreateLogger(nameof(CustomerController));

        [HttpGet]
        public IActionResult List()
        {
            DbCache.CustomerCache.Refresh();
            var list = DbCache.CustomerCache.Get();
            logger.Info("Customer list count:" + list.Count);
            return View(list);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var user = CustomerMgr.This.GetValidCustomer(id);
            logger.Info("Customer details:" + id);
            return View(user);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = CustomerMgr.This.GetValidCustomer(id);
            logger.Info("Customer edit:" + id);
            return View(user);
        }
        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
           
            var res = CustomerMgr.This.UpdateCustomer(customer);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.Message);
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
            var res = CustomerMgr.This.AddCustomer(customer);
            if (!res.IsSuccess)
            {
                logger.Warn("customer add:" + customer.ToJsonString(), res.ToJsonString());
                ModelState.AddModelError("", res.Message);
                return View(customer);
            }
            logger.Info("customer add:" + customer.ToJsonString());
            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var res = CustomerMgr.This.DeleteCustomer(id);
            if (!res.IsSuccess)
            {
                logger.Warn("Customer delete:" + id, res.ToJsonString());
                ModelState.AddModelError("", res.Message);
                return RedirectToAction("List");
            }
            logger.Info("Customer delete:" + id);
            return RedirectToAction("List");
        }
    }
}
