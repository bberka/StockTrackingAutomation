using Application.Manager;
using Domain.Helpers;
using Domain.Models;
using EasMe;
using EasMe.Extensions;
using Infrastructure.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
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
            var res = UserMgr.This.Login(model);
			if (!res.IsSuccess)
			{
				ModelState.AddModelError("", res.Message);
                logger.Warn("Login failed", res.ToJsonString());
                return View(model);
			}
			HttpContext.SetUser(res.Data);
			logger.Info("Login success", res.ToJsonString());
			return RedirectToAction("Statistics");
		}
		[HttpGet]
        public IActionResult Logout()
        {
			var user = HttpContext.GetUser();
            logger.Info("Logging out" , user.ToJsonString());
            HttpContext.RemoveAuth();
            return RedirectToAction("Index");
        }
        [HttpGet]
		[AuthFilter]
		public IActionResult Statistics()
		{
			return View();
		}



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			var err = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
			logger.Error(err.ToJsonString());
            return View();
		}
	}
}