using Domain.Entities;
using Domain.Helpers;
using Domain.Models;
using Infrastructure.DAL;
using Microsoft.AspNetCore.Mvc;
using StockTrackingAutomation.Web.Filters;
using System.Linq;

namespace StockTrackingAutomation.Web.Controllers
{
    [AuthFilter]
    public class StockLogController : Controller
    {
        [HttpGet]
        public IActionResult List()
        {
            var list = StockLogDAL.This.GetList();
            var products = ProductDAL.This.GetValidProducts().Select(x => x.ProductNo);
            list.RemoveAll(x => !products.Contains(x.ProductId));
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
                ModelState.AddModelError("", "Product:NotFound");
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
                ModelState.AddModelError("", "Yeterli stok yok");
                return View(viewModel);
            }
            var productUpdate = ProductDAL.This.Update(product);
            if (!productUpdate)
            {
                ModelState.AddModelError("", "DbError");
                return View(viewModel);
            }
            var res = StockLogDAL.This.Add(data);
            if (!res)
            {
                ModelState.AddModelError("", "DbError");
                return View(viewModel);
            }
            return RedirectToAction("List");
        }
    }
}
