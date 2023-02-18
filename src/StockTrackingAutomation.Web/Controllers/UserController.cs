using Domain.Abstract;
using Domain.Entities;
using Domain.Enums;
using Domain.Helpers;
using Domain.Models;
using EasMe.Logging;
using Microsoft.AspNetCore.Mvc;
using StockTrackingAutomation.Web.Filters;

namespace StockTrackingAutomation.Web.Controllers
{
    [AuthFilter(RoleType.Owner)]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private static readonly IEasLog logger = EasLogFactory.CreateLogger();

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public IActionResult List()
        {
            var list = _userService.GetList();
            logger.Info("User list count:" + list.Count);
            return View(list);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var user = _userService.GetUser(id);
            logger.Info("User details:" + id);
            return View(user);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _userService.GetUser(id);
            logger.Info("User edit:" + id);
            return View(user);
        }
        [HttpPost]
        public IActionResult Edit(User user)
        {
           
            var res = _userService.UpdateUser(user);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.ErrorCode);
                logger.Warn("User edit:" + user.Id, res.Rv + res.ErrorCode);
                return View(user);
            }
            logger.Info("User edit:" + user.Id);
            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(User user)
        {
            var res = _userService.Register(user);
            if (!res.IsSuccess)
            {
                logger.Warn("User add:" + user.EmailAddress, res.Rv + res.ErrorCode);
                ModelState.AddModelError("", res.ErrorCode);
                return View(user);
            }
            logger.Info("User add:" + user.EmailAddress);
            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var res = _userService.DeleteUser(id);
            if (!res.IsSuccess)
            {
                logger.Warn("User delete:" + id, res.Rv + res.ErrorCode);
                ModelState.AddModelError("", res.ErrorCode);
                return RedirectToAction("List");
            }
            logger.Info("User delete:" + id);
            return RedirectToAction("List");
        }

       
    }
}
