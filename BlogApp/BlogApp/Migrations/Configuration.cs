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
    using BlogApp.Core.DataBase;

    internal sealed class Configuration : DbMigrationsConfiguration<BlogContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BlogContext context)
        { }
    }
}