using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using BlogApp.Core.Objects;
using BlogApp.Core.Identity;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using BlogApp.Core.DataBase;

[assembly: OwinStartup(typeof(BlogApp.App_Start.Startup))]
namespace BlogApp.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<BlogContext>(BlogContext.Create);
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