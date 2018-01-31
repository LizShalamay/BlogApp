using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BlogApp.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BlogApp.Core.DataBase
{
    class BlogContext : IdentityDbContext
    {
        //public DbSet<User> Users { get; set; }
        //public DbSet<Role> Roles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public BlogContext() : base("Blog") { }
        public static BlogContext Create()
        {
            return new BlogContext();
        }
    }
}
