using Domain.Entities;
using Infrastructure.DAL;
using Microsoft.AspNetCore.Mvc;
using StockTrackingAutomation.Web.Filters;

namespace StockTrackingAutomation.Web.Controllers
{
    [AuthFilter]
    public class StockLogController : Controller
    {
        [HttpGet]
        public IActionResult List()
        {
            var list = StockLogDAL.This.GetList();
            return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(StockLog data)
        {
            data.RegisterDate = DateTime.Now;
            var res = StockLogDAL.This.Add(data);
            if (!res)
            {
                ModelState.AddModelError("", "DbError");
                return View(data);
            }

            return RedirectToAction("List");
        }
    }
}
