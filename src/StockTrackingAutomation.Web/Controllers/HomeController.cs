using Domain.Helpers;
using Domain.Models;
using EasMe;
using Infrastructure.DAL;
using Microsoft.AspNetCore.Mvc;
using StockTrackingAutomation.Web.Filters;
using StockTrackingAutomation.Web.Models;
using System.Diagnostics;

namespace StockTrackingAutomation.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly EasLog logger = EasLogFactory.CreateLogger(nameof(HomeController));
		public HomeController()
		{
		}

		[HttpGet]
		public IActionResult Index()
		{
            if (HttpContext.IsAuthenticated())
            {
                return RedirectToAction("Statistics");
            }
            return View();
		}
		[HttpPost]
		public IActionResult Index(LoginModel model)
		{
			if (HttpContext.IsAuthenticated())
			{
				return RedirectToAction("Statistics");
            }
            var res = UserDAL.This.Login(model);
			if (!res.IsSuccess)
			{
				ModelState.AddModelError("", res.Message);
				return View(model);
			}
			HttpContext.SetUser(res.Data);
			return RedirectToAction("Statistics");
		}
		[HttpGet]
        public IActionResult Logout()
        {
			HttpContext.RemoveAuth();
            return RedirectToAction("Index");
        }
        [HttpGet]
		[AuthFilter]
		public IActionResult Statistics()
		{
			return View();
		}

		[HttpGet]
        public IActionResult Hash(string text)
        {
			var hash = Convert.ToBase64String(text.MD5Hash());
            return Ok(hash);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}