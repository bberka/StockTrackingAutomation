using Domain.Helpers;
using Domain.Models;
using EasMe.Extensions;
using EasMe.Logging;
using Microsoft.AspNetCore.Mvc;
using StockTrackingAutomation.Web.Filters;
using StockTrackingAutomation.Web.Models;
using System.Diagnostics;
using Domain.Abstract;

namespace StockTrackingAutomation.Web.Controllers
{
	public class HomeController : Controller
    {
        private readonly IUserService _userMgr;
        private static readonly IEasLog logger = EasLogFactory.CreateLogger();

        public HomeController(IUserService userMgr)
        {
            _userMgr = userMgr;
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
            var res = _userMgr.Login(model);
			if (!res.IsSuccess)
			{
				ModelState.AddModelError("", res.ErrorCode);
                logger.Warn("Login failed: " + model.EmailAddress, res.Rv + res.ErrorCode);
                return View(model);
			}
			HttpContext.SetUser(res.Data);
			logger.Info("Login success: " + model.EmailAddress , res.Rv + res.ErrorCode);
			return RedirectToAction("Statistics");
		}
		[HttpGet]
        public IActionResult Logout()
        {
			var user = HttpContext.GetUser();
            logger.Info("Logging out: " + user.Id);
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