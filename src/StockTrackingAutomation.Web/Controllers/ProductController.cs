using Domain.Entities;
using EasMe;
using Infrastructure.Caching;
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
            DbCache.ProductCache.Refresh();
            return View(DbCache.ProductCache.Get());
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
            var current = ProductDAL.This.Find(product.ProductNo);
            if (current is null)
            {
                ModelState.AddModelError("", "Ürün bulunamadı");
                return View(product);
            }
            current.Description = product.Description;
            current.Name = product.Name;
            var res = ProductDAL.This.Update(current);
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
