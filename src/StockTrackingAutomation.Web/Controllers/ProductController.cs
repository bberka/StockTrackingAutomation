using Application.Caching;
using Application.Manager;
using Domain.Entities;
using EasMe;
using EasMe.Extensions;
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
            var res = ProductMgr.This.UpdateProduct(product);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.Message);
                logger.Warn("Product edit:" + product.ToJsonString(), res.ToJsonString());
                return View(product);
            }
            logger.Info("Product edit:" + product.ToJsonString());
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
            var res = ProductMgr.This.AddProduct(product);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.Message);
                logger.Warn("Product add: " + product.ToJsonString(), res.ToJsonString());
                return View(product);
            }
            logger.Info("Product add: " + product.ToJsonString(), res.ToJsonString());
            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var res = ProductMgr.This.DeleteProduct(id);
            if (!res.IsSuccess)
            {
                logger.Warn("Product delete: " + id, res.ToJsonString());
                return RedirectToAction("List");
            }
            logger.Info("Product delete: " + id, res.ToJsonString());
            return RedirectToAction("List");
        }
    }
}
