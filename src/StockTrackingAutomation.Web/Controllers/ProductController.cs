using Domain.Entities;
using EasMe;
using EasMe.Extensions;
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
            var res = DbCache.ProductCache.Get();
            logger.Info("Product list count: " + res.Count);
            return View(res);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = ProductDAL.This.Find(id);
            logger.Info("Product edit: " + product.ToJsonString());
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            var current = ProductDAL.This.Find(product.ProductNo);
            if (current is null)
            {
                logger.Info("Product edit: " + product.ToJsonString(),"Ürün bulunamadı");
                ModelState.AddModelError("", "Ürün bulunamadı");
                return View(product);
            }
            current.Description = product.Description;
            current.Name = product.Name;
            var res = ProductDAL.This.Update(current);
            if (!res)
            {
                logger.Warn("Product edit: " + product.ToJsonString(), res.ToJsonString());
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
                logger.Warn("Product edit: " + product.ToJsonString(), res.ToJsonString());
                return View(product);
            }
            logger.Info("Product add: " + product.ToJsonString(), res.ToJsonString());
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
                logger.Warn("Product delete: " + id, res.ToJsonString());
                ModelState.AddModelError("", "DbError");
                return View(data);
            }
            logger.Info("Product delete: " + id, res.ToJsonString());
            return RedirectToAction("List");
        }
    }
}
