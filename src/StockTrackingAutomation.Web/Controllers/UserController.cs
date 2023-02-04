using Domain.Entities;
using EasMe;
using EasMe.Extensions;
using Infrastructure.DAL;
using Microsoft.AspNetCore.Mvc;
using StockTrackingAutomation.Web.Filters;

namespace StockTrackingAutomation.Web.Controllers
{
    [AuthFilter]
    public class UserController : Controller
    {

        [HttpGet]
        public IActionResult List()
        {
            var list = UserDAL.This.GetList(x => x.IsValid == true && !x.DeletedDate.HasValue);
            return View(list);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var user = UserDAL.This.Find(id);
            return View(user);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = UserDAL.This.Find(id);
            return View(user);
        }
        [HttpPost]
        public IActionResult Edit(User user)
        {
            var current = UserDAL.This.Find(user.UserNo);
            if(current is null)
            {
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
                ModelState.AddModelError("", "DbError");
                return View(user);
            }

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
                ModelState.AddModelError("", "DbError");
                return View(user);
            }
            return RedirectToAction("List");
        }
    }
}
