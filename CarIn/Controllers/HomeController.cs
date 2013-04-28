﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarIn.BLL;
using CarIn.DAL.Repositories;
using CarIn.Models.Entities;
using CarIn.Models.ViewModels;

namespace CarIn.Controllers
{
    public class HomeController : Controller
    {
        private readonly Repository<User> _userRepo = new Repository<User>();

        public ActionResult Index()
        {
            ViewBag.AllUsers = _userRepo.FindAll().ToList(); 
            return View();
        }

        public ActionResult ChangePassword()
        {

            ViewBag.Message = "Ändra lösenord";
            ChangePasswordVm viewModelChangePassword = null;
            var tmpLoggedinUser = _userRepo.FindAll().FirstOrDefault();
            if(tmpLoggedinUser != null)
            {
                viewModelChangePassword = new ChangePasswordVm
                {
                    userId = tmpLoggedinUser.ID,
                    Username = tmpLoggedinUser.Username
                };
            }
            else
            {
                viewModelChangePassword = new ChangePasswordVm
                                              {
                                                  userId = 0,
                                                  Username = "TmpUserNotInDb"
                                              };
            }
            return View("ChangePassword", viewModelChangePassword);

        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordVm model)
        {
            if(ModelState.IsValid)
            {
                var passHelper = new PasswordHelper();

                if(!passHelper.CheckIfPasswordMatch(model.OldPassword, model.OldPassword))
                {
                    ModelState.AddModelError("OldPassword", "Felaktigt lösenord");
                    return View(model);
                }

                var user = _userRepo.FindAll(x => x.Username == model.Username).FirstOrDefault();
                user.Password = passHelper.HashPassword(model.NewPassword);
                _userRepo.Update(user);
                ViewBag.Message = "Lösenord ändrat";
                return RedirectToAction("Index");
            }
            return View("ChangePassword", model);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }


    }
}
