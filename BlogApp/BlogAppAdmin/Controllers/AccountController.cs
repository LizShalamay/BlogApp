using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
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
            if (User.Identity.IsAuthenticated && User.IsInRole("admin"))
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
                if (user != null)
                {
                    ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

                    var authResponseGrant = new AuthenticationResponseGrant(claim, new AuthenticationProperties());
                    var userPrincipal = new ClaimsPrincipal(authResponseGrant.Identity);
                    if (userPrincipal.IsInRole("admin"))
                    {
                        AuthenticationManager.SignOut();
                        AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, claim);
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        ModelState.AddModelError("", "You have no permissions to sign in");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "No admin found");
                }
            }
            if (User.Identity.IsAuthenticated)
                AuthenticationManager.SignOut();
            return View(model);
        }

        public ActionResult Logoff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}