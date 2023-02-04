using Domain.Entities;
using EasMe;
using Infrastructure.DAL;
using Microsoft.AspNetCore.Mvc;
using StockTrackingAutomation.Web.Filters;

namespace StockTrackingAutomation.Web.Controllers
{
    [AuthFilter]
    public class ProductController : Controller
    {
        private readonly EasLog logger = EasLogFactory.CreateLogger(nameof(ProductController));

        [HttpGet]
        public IActionResult List()
        {
            var list = ProductDAL.This.GetList(x => !x.DeletedDate.HasValue);
            return View(list);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = ProductDAL.This.Find(id);
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            var res = ProductDAL.This.Update(product);
            if (!res)
            {
                ModelState.AddModelError("", "DbError");
                return View(product);
            }

            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            var res = ProductDAL.This.Add(product);
            if (!res)
            {
                ModelState.AddModelError("", "DbError");
                return View(product);
            }

            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var data = ProductDAL.This.Find(id);
            if (data == null)
            {
                return RedirectToAction("List");
            }
            data.DeletedDate = DateTime.Now;
            var res = ProductDAL.This.Update(data);
            if (!res)
            {
                ModelState.AddModelError("", "DbError");
                return View(data);
            }
            return RedirectToAction("List");
        }
    }
}
