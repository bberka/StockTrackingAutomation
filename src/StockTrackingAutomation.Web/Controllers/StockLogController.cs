using Domain.Entities;
using Domain.Helpers;
using Domain.Models;
using EasMe;
using EasMe.Extensions;
using Infrastructure.DAL;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using StockTrackingAutomation.Web.Filters;
using System.Collections.Generic;
using System.Linq;

namespace StockTrackingAutomation.Web.Controllers
{
    [AuthFilter]
    public class StockLogController : Controller
    {
        private static EasLog logger = EasLogFactory.CreateLogger(nameof(StockLogController));

        [HttpGet]
        public IActionResult List()
        {
            var list = StockLogDAL.This.GetList();
            var products = ProductDAL.This.GetValidProducts().Select(x => x.ProductNo);
            list.RemoveAll(x => !products.Contains(x.ProductId));
            logger.Info("StockLog list: " + list.Count);
            return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var res = new StockLogCreateViewModel
            {
                Products = ProductDAL.This.GetList(x => !x.DeletedDate.HasValue),
            };
            return View(res);
        }
        [HttpPost]
        public IActionResult Create(StockLogCreateViewModel viewModel)
        {
            viewModel.Products = ProductDAL.This.GetList(x => !x.DeletedDate.HasValue);
            var data = viewModel.Data;
            data.RegisterDate = DateTime.Now;
            data.UserId = HttpContext.GetUser().UserNo;
            var product = ProductDAL.This.Find(data.ProductId);
            if(product is null)
            {
                ModelState.AddModelError("", "Ürün bulunamadı");
                logger.Warn("StockLog create: " + data.ToJsonString(),"Product not found");
                return View(viewModel);
            }
            if (data.Type == 1)
            {
                product.Stock += data.Count;
            }
            else if(data.Type == 2)
            {
                product.Stock -= data.Count;
            }
            if(product.Stock < 0)
            {
                logger.Warn("StockLog create: " + data.ToJsonString(), "Not enough stock");
                ModelState.AddModelError("", "Yeterli stok yok");
                return View(viewModel);
            }
            var productUpdate = ProductDAL.This.Update(product);
            if (!productUpdate)
            {
                ModelState.AddModelError("", "DbError");
                logger.Warn("StockLog create: " + data.ToJsonString(), "DbError");
                return View(viewModel);
            }
            var res = StockLogDAL.This.Add(data);
            if (!res)
            {
                ModelState.AddModelError("", "DbError");
                logger.Warn("StockLog create: " + data.ToJsonString(), "DbError");
                return View(viewModel);
            }
            logger.Info("StockLog create: " + data.ToJsonString());
            return RedirectToAction("List");
        }
    }
}
