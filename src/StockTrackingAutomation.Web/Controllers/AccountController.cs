using System.Net;
using Domain.Abstract;
using Domain.Helpers;
using Domain.Models;
using EasMe.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockTrackingAutomation.Web.Filters;

namespace StockTrackingAutomation.Web.Controllers
{
    [AuthFilter]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private static readonly IEasLog logger = EasLogFactory.CreateLogger();

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public IActionResult Details()
        {
            var user = HttpContext.GetUser();
            return View(user);
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordModel model)
        {
            var user = HttpContext.GetUser();
            var res = _userService.ChangePassword(user.Id, model);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.ErrorCode);
                logger.Warn("User edit:" + user.Id, res.Rv + res.ErrorCode);
                return View(model);
            }
            logger.Info("User edit:" + user.Id);

            return RedirectToAction("Logout", "Home");
        }
    }
}
