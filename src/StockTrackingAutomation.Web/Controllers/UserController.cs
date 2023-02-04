﻿using Application.Caching;
using Application.Manager;
using Domain.Entities;
using Domain.Enums;
using EasMe;
using EasMe.Extensions;
using Infrastructure.DAL;
using Microsoft.AspNetCore.Identity;
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
            return View(list);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var user = UserMgr.This.GetUser(id);
            logger.Info("User details:" + id);
            return View(user);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = UserMgr.This.GetUser(id);
            logger.Info("User edit:" + id);
            return View(user);
        }
        [HttpPost]
        public IActionResult Edit(User user)
        {
           
            var res = UserMgr.This.UpdateUser(user);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("", res.Message);
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
            var res = UserMgr.This.Register(user);
            if (!res.IsSuccess)
            {
                logger.Warn("User add:" + user.ToJsonString(), res.ToJsonString());
                ModelState.AddModelError("", res.Message);
                return View(user);
            }
            logger.Info("User add:" + user.ToJsonString());
            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var res = UserMgr.This.DeleteUser(id);
            if (!res.IsSuccess)
            {
                logger.Warn("User delete:" + id, res.ToJsonString());
                ModelState.AddModelError("", res.Message);
                return RedirectToAction("List");
            }
            logger.Info("User delete:" + id);
            return RedirectToAction("List");
        }
    }
}
