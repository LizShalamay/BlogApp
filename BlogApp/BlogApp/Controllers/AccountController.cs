using BlogApp.Models;
using BlogApp.Data.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using BlogApp.Data;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BlogApp.Controllers
{
    public class AccountController : Controller
    {
        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Login = model.Login, UserName = model.Login };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                await UserManager.AddToRoleAsync(user.Id, "user");

                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View(model);
        }

        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated & User.IsInRole("user"))
                return RedirectToAction("Index", "Blog");
            if(User.IsInRole("admin")) Logoff();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.Login, model.Password);                
                if (user == null)
                {
                    ModelState.AddModelError("", "No user found");
                }
                else
                {
                    ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Index", "Account",model);
                }
            }
            if (User.Identity.IsAuthenticated)
                Logoff();
            return View(model);
        }
        private bool IsAdmin()
        {
            return User.IsInRole("admin") ? true : false;
        }
        public ActionResult Logoff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }
        
        public ActionResult Index()
        {
            bool isAdmin = IsAdmin();
            if (!isAdmin)
                return RedirectToAction("Index","Blog");
            else
            {
                ViewBag.Error = "Admins are not allowed to log in";
                return RedirectToAction("Login");
            }
        }
    }
}