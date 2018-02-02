using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using BlogApp.Data.Entities;
using BlogData;

namespace BlogApp.Data
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

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 5,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };

            return manager;
        }
    }
}
