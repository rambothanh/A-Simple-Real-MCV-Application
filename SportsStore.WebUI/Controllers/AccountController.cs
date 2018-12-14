﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private IAuthProvider authProvider;

        public AccountController(IAuthProvider auth)
        {
            authProvider = auth;
        }

        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (authProvider.Authenticate(loginViewModel.UserName,
                    loginViewModel.Password))
                {
                    return Redirect(returnUrl ??
                                    Url.Action("Index", "Admin"));
                }
                else
                {
                    ModelState.AddModelError("", 
                        "Incorrect username or password");
                    return View();
                }
            }

            return View();
        }

    }
}