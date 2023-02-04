using Domain.Entities;
using Domain.Helpers;
using Domain.Models;
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
            var res = new StockLogCreateViewModel
            {
                Products = ProductDAL.This.GetList(x => !x.DeletedDate.HasValue),
            };
            return View(res);
        }
        [HttpPost]
        public IActionResult Create(StockLogCreateViewModel viewModel)
        {
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
            else if(data.Type == 1)
            {
                product.Stock -= data.Count;
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
