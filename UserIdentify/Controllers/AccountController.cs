﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using UserIdentify.Models;
using UserIdentify.ViewModels;

namespace UserIdentify.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public async  Task<IActionResult> Login(LoginViewModel model)
        { 
            if(ModelState.IsValid)
            {
                var result=await signInManager.PasswordSignInAsync(model.Username,model.Password,model.RememberMe,false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid login attempt");
                return View(model);
            }

            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>Register(RegisterViewmodel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new()
                {
                    Name=model.Name,
                    UserName=model.Email,
                    Email=model.Email,
                    Address=model.Address
                };
                var result = await userManager.CreateAsync(user, model.Password!);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult>LogOut() {

            await signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
