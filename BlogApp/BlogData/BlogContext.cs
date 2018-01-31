using System.Data.Entity;
using BlogApp.Data.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BlogApp.Data
{
    public class BlogContext : IdentityDbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public BlogContext() : base("Blog") { }
        public static BlogContext Create()
        {
            return new BlogContext();
        }
    }
}
