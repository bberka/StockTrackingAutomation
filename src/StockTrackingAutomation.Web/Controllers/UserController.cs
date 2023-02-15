using Application.Manager;
using Domain.Entities;
using Domain.Enums;
using EasMe.Extensions;
using EasMe.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using StockTrackingAutomation.Web.Filters;

namespace StockTrackingAutomation.Web.Controllers
{
    [AuthFilter(RoleType.Owner)]
    public class UserController : Controller
    {
        private readonly IUserMgr _userMgr;
        private static readonly IEasLog logger = EasLogFactory.CreateLogger();

        public UserController(IUserMgr userMgr)
        {
            _userMgr = userMgr;
        }
        [HttpGet]
        public IActionResult List()
        {
            var list = _userMgr.GetValidUsers();
            logger.Info("User list count:" + list.Count);
            return View(list);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var user = _userMgr.GetUser(id);
            logger.Info("User details:" + id);
            return View(user);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _userMgr.GetUser(id);
            logger.Info("User edit:" + id);
            return View(user);
        }
        [HttpPost]
        public IActionResult Edit(User user)
        {
           
            var res = _userMgr.UpdateUser(user);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.ErrorCode);
                logger.Warn("User edit:" + user.ToJsonString(),res.ToJsonString());
                return View(user);
            }
            logger.Info("User edit:" + user.ToJsonString());
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
            var res = _userMgr.Register(user);
            if (!res.IsSuccess)
            {
                logger.Warn("User add:" + user.ToJsonString(), res.ToJsonString());
                ModelState.AddModelError("", res.ErrorCode);
                return View(user);
            }
            logger.Info("User add:" + user.ToJsonString());
            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var res = _userMgr.DeleteUser(id);
            if (!res.IsSuccess)
            {
                logger.Warn("User delete:" + id, res.ToJsonString());
                ModelState.AddModelError("", res.ErrorCode);
                return RedirectToAction("List");
            }
            logger.Info("User delete:" + id);
            return RedirectToAction("List");
        }
    }
}
