namespace BlogApp.Core.Migrations
{
    using BlogApp.Core.Identity;
    using BlogApp.Core.Objects;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using BlogApp.Controllers;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<BlogApp.Core.DataBase.BlogContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BlogApp.Core.DataBase.BlogContext context)
        {
            var userManager = new AppUserManager(new UserStore<User>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var roles = new List<IdentityRole>
            {
                new IdentityRole { Name = "admin" },
                new IdentityRole { Name = "user" }
            };

            foreach(var role in roles)
            {
                var currentRole = roleManager.FindByName(role.Name);
                if(currentRole == null)
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
            if(adminUser == null)
            {
                var result = userManager.Create(admin, adminPassword);
                userManager.SetLockoutEnabled(admin.Id, false);

                if (result.Succeeded)
                {
                    userManager.AddToRole(admin.Id, roles[0].Name);
                }
            }            
        }
    }
}
