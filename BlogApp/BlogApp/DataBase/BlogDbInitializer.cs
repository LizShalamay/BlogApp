using BlogApp.Core.DataBase;
using BlogApp.Core.Identity;
using BlogApp.Core.Objects;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BlogApp.DataBase
{
    class BlogDbInitializer : DropCreateDatabaseAlways<BlogContext>
    {
        public void SeedIdentity(BlogContext context)
        {
            var userManager = new AppUserManager(new UserStore<User>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var roles = new List<IdentityRole>
            {
                new IdentityRole { Name = "admin" },
                new IdentityRole { Name = "user" }
            };

            foreach (var role in roles)
            {
                var existingRole = roleManager.FindByName(role.Name);
                if (existingRole == null)
                {
                    roleManager.Create(role);
                }
            }

            var admin = new User
            {
                UserName = "admin",
                Login = "admin",
                EmailConfirmed = true,
                Id = Guid.NewGuid().ToString()
            };
            string adminPassword = "admin123";

            var adminUser = userManager.FindByName(admin.UserName);
            if (adminUser == null)
            {
                var result = userManager.Create(admin, adminPassword);
                userManager.SetLockoutEnabled(admin.Id, false);

                if (result.Succeeded)
                {
                    userManager.AddToRole(admin.Id, roles[0].Name);
                }
            }
            base.Seed(context);
        }
    }
}