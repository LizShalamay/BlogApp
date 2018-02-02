using BlogApp.Data;
using BlogAppAdmin.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web;
using BlogApp.Data.Entities;

namespace BlogAppAdmin.Controllers
{
    public class AdminController : Controller
    {
        BlogContext db = new BlogContext();

        public ActionResult ManagingPage()
        {
            return PartialView();
        }
        public IEnumerable<PostModel> News()
        {
            var news = (from item in db.Posts
                        orderby item.Date descending
                        select item).ToList();
            List<PostModel> postModels = new List<PostModel>();
            foreach (var item in news)
            {
                var author = from user in db.Users
                             where user.Id == item.AuthorId
                             select user.UserName;
                postModels.Add(new PostModel
                {
                    AuthorName = author.First(),
                    Title = item.Title,
                    Date = item.Date,
                    Id = item.Id
                });
            }
            return postModels;
        }
        public ActionResult OpenPost(string postId)
        {
            var post = (from item in db.Posts
                        where item.Id == postId
                        select item).ToList();
            return PartialView(post[0]);
        }
        public ActionResult CommentTable()
        {
            var comments = Comments();
            return PartialView("CommentTable", comments);
        }
        private IEnumerable<CommentModel> Comments()
        {
            var comments = (from item in db.Comments
                            select item).ToList();
            List<CommentModel> commentModels = new List<CommentModel>();
            foreach (var comment in comments)
            {
                string author = (from item in db.Users
                                 where item.Id == comment.AuthorId
                                 select item.UserName).ToList().First();
                commentModels.Add(new CommentModel
                {
                    AuthorName = author,
                    Text = comment.Text.Length >= 20 ? comment.Text.Substring(0,17) + "..." : comment.Text,
                    Date = comment.Date,
                    Id = comment.Id,
                    PostId = comment.PostId,
                    ParentId = comment.ParentId == null ? "" : comment.ParentId
                });
            }
            return commentModels;
        }        
        public ActionResult UserTable()
        {
            var users = Users();
            return PartialView("UserTable", users);
        }
        private IEnumerable<UserModel> Users()
        {
            var users = (from user in db.Users
                         where user.UserName != "admin"
                         select user).ToList();
            List<UserModel> userModels = new List<UserModel>();
            foreach (var user in users)
            {
                userModels.Add(new UserModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    PostsNumber = (from post in db.Posts
                                   where post.AuthorId == user.Id
                                   select post).ToList().Count,
                    CommentsNumber = (from comment in db.Comments
                                      where comment.AuthorId == user.Id
                                      select comment).ToList().Count
                });
            }
            return userModels;
        }
        public ActionResult PostTable()
        {
            var posts = Posts();
            return PartialView("PostTable", posts);
        }
        private IEnumerable<PostModel> Posts()
        {
            var posts = (from post in db.Posts
                         select post).ToList();
            List<PostModel> postModels = new List<PostModel>();
            foreach (var post in posts)
            {
                postModels.Add(new PostModel
                {
                    Id = post.Id,
                    AuthorName = ((from user in db.Users
                                  where user.Id == post.AuthorId
                                  select user).ToList())[0].UserName,
                    Date = post.Date,
                    Title = post.Title,
                    CommentsNumber = (from comment in db.Comments
                                      where comment.PostId == post.Id
                                      select comment).ToList().Count
                });
            }
            return postModels;
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}