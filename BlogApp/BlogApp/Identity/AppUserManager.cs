using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using BlogApp.Models;
using BlogApp.Core.DataBase;

namespace BlogApp.Core.Identity
{
    public class AppUserManager:UserManager<User>
    {
        public AppUserManager(IUserStore<User> store)
           : base(store)
        { }
        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options,
            IOwinContext context)
        {
            BlogContext db = context.Get<BlogContext>();
            AppUserManager manager = new AppUserManager(new UserStore<User>(db));
            return manager;
        }
    }
}
