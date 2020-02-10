using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Photo_WebApp.Repository;
using Photo_WebApp.Models;

namespace Photo_WebApp.Controllers
{
    public class LoginController : Controller
    {
        DB_Context repos;

        public LoginController(DB_Context r)
        {
            repos = r;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Login model)
        {
            User user = repos.GetUser(model);

            if (repos.Login(model))
            {
                Authenticate(user);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Некорректные логин и(или) пароль"); // не понимаю, как вывусти эти ошибки в View

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User model)
        {
            // if (ModelState.IsValid)
            {
                if (!repos.IsRegisted(model.Email))
                {
                    repos.AddUser(model);
                    Authenticate(model);
                    return RedirectToAction("Index", "Home");
                }
                //else
                //    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        private void Authenticate(User model)
        {
            HttpContext.Session.SetString("Username", model.Nickname);
            HttpContext.Session.SetString("UserRole", model.Role.ToString());
            HttpContext.Session.SetString("UserId", model.Id.ToString());
        }
    }
}
