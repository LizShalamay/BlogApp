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
using BlogAppAdmin.Models;

namespace BlogAppAdmin.Controllers
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

        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated & User.IsInRole("admin"))
                return RedirectToAction("Index", "Admin");
            if (User.IsInRole("user")) Logoff();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.Login, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "No admin found");
                }
                else
                {
                    ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Index", "Account");
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
            if (isAdmin)
                return RedirectToAction("Index", "Admin");
            else
            {
                ViewBag.Error = "Users are not allowed to log in";
                return RedirectToAction("Login");
            }
        }
    }
}