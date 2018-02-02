namespace BlogData.Migrations
{
    using BlogApp.Data.Entities;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity.Migrations;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Collections.Generic;
    using BlogApp.Data;

    internal sealed class Configuration : DbMigrationsConfiguration<BlogContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BlogContext context)
        {
            var userManager = new AppUserManager(new UserStore<User>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 5,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };

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
            string adminPassword = "admin";

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