using Application.Manager;
using Domain.Entities;
using EasMe.Extensions;
using EasMe.Logging;
using Microsoft.AspNetCore.Mvc;
using StockTrackingAutomation.Web.Filters;

namespace StockTrackingAutomation.Web.Controllers
{
    [AuthFilter]
    public class ProductController : Controller
    {
        private readonly IProductService _productMgr;
        private static readonly IEasLog logger = EasLogFactory.CreateLogger();

        public ProductController(IProductService productMgr)
        {
            _productMgr = productMgr;
        }
        [HttpGet]
        public IActionResult List()
        {
            var res = _productMgr.GetValidProducts();
            logger.Info("Product list count: " + res.Count);
            return View(res);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _productMgr.GetProduct(id);
            logger.Info("Product edit: " + product.ToJsonString());
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            var res = _productMgr.UpdateProduct(product);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.ErrorCode);
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
            var res = _productMgr.AddProduct(product);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.ErrorCode);
                logger.Warn("Product add: " + product.ToJsonString(), res.ToJsonString());
                return View(product);
            }
            logger.Info("Product add: " + product.ToJsonString(), res.ToJsonString());
            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var res = _productMgr.DeleteProduct(id);
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
