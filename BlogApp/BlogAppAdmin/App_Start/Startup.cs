
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using BlogApp.Data;
using Microsoft.AspNet.Identity.EntityFramework;

[assembly: OwinStartup(typeof(BlogAppAdmin.App_Start.Startup))]
namespace BlogAppAdmin.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(BlogContext.Create);
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                ReturnUrlParameter = "returnUrl"
            });
        }
    }
}