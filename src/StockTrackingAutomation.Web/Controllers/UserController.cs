using Domain.Entities;
using Domain.Enums;
using EasMe;
using EasMe.Extensions;
using Infrastructure.Caching;
using Infrastructure.DAL;
using Microsoft.AspNetCore.Mvc;
using StockTrackingAutomation.Web.Filters;

namespace StockTrackingAutomation.Web.Controllers
{
    [AuthFilter(RoleType.Owner)]
    public class UserController : Controller
    {
        private static EasLog logger = EasLogFactory.CreateLogger(nameof(UserController));

        [HttpGet]
        public IActionResult List()
        {
            DbCache.UserCache.Refresh();
            var list = DbCache.UserCache.Get();
            logger.Info("User list count:" + list.Count);
            return View();
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var user = UserDAL.This.Find(id);
            logger.Info("User details:" + id);
            return View(user);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = UserDAL.This.Find(id);
            logger.Info("User edit:" + id);
            return View(user);
        }
        [HttpPost]
        public IActionResult Edit(User user)
        {
            var current = UserDAL.This.Find(user.UserNo);
            if(current is null)
            {
                logger.Warn("User edit:" + user.ToJsonString(), "User not exist");
                ModelState.AddModelError("", "User not exist");
                return View(user);
            }
            current.IsValid = user.IsValid;
            current.EmailAddress = user.EmailAddress;
            current.Password = Convert.ToBase64String(user.Password.MD5Hash());
            current.RoleType = user.RoleType;
            var res = UserDAL.This.Update(current);
            if (!res)
            {
                ModelState.AddModelError("", "DbError");
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
            user.Password = Convert.ToBase64String(user.Password.MD5Hash());
            var res = UserDAL.This.Add(user);
            if (!res)
            {
                logger.Warn("User add:" + user.ToJsonString(), "DbError");
                ModelState.AddModelError("", "DbError");
                return View(user);
            }
            logger.Info("User add:" + user.ToJsonString());

            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var user = UserDAL.This.Find(id);
            if (user == null)
            {
                return RedirectToAction("List");
            }
            user.DeletedDate = DateTime.Now;
            user.IsValid = false;
            var res = UserDAL.This.Update(user);
            if (!res)
            {
                logger.Warn("User delete:" + user.ToJsonString(), "DbError");
                ModelState.AddModelError("", "DbError");
                return View(user);
            }
            logger.Info("User delete:" + user.ToJsonString());
            return RedirectToAction("List");
        }
    }
}
