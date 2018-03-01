using BlogApp.Data;
using BlogAppAdmin.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace BlogAppAdmin.Controllers
{
    public class AdminController : Controller
    {
        BlogContext db = new BlogContext();

        public ActionResult ManagingPage()
        {
            return PartialView();
        }

        public ActionResult CommentTable()
        {
            var comments = Comments;
            return PartialView("CommentTable", comments);
        }
        public ActionResult UserTable()
        {
            var users = Users;
            return PartialView("UserTable", users);
        }
        public ActionResult PostTable()
        {
            var posts = Posts;
            return PartialView("PostTable", posts);
        }

        public ActionResult DeleteUser(string userId)
        {
            var user = (from u in db.Users
                        where u.Id == userId
                        select u).ToList()[0];
            DeletePosts(user.Id);
            DeleteComments(userId: user.Id);
            db.Users.Remove(user);
            db.SaveChanges();
            return View("Index");
        }
        public ActionResult DeletePost(string postId)
        {
            var post = (from p in db.Posts
                        where p.Id == postId
                        select p).ToList()[0];
            DeleteComments(postId: post.Id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return View("Index");
        }
        public ActionResult DeleteComment(string commentId)
        {
            var comment = (from c in db.Comments
                           where c.Id == commentId
                           select c).ToList()[0];
            db.Comments.Remove(comment);
            db.SaveChanges();
            return View("Index");
        }

        public ActionResult OpenUser(string userId = "", string userName = "")
        {
            if (userId != "")
            {
                var user = (from u in db.Users
                            where u.Id == userId
                            select u).ToList()[0];
                UserModel userModel = new UserModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Posts = (from post in db.Posts
                             where post.AuthorId == user.Id
                             select post).ToList(),
                    Comments = (from comment in db.Comments
                                where comment.AuthorId == user.Id
                                select comment)

                };
                return View(userModel);
            }
            else if (userName != "")
            {
                var user = (from u in db.Users
                            where u.UserName == userName
                            select u).ToList()[0];
                UserModel userModel = new UserModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Posts = (from post in db.Posts
                             where post.AuthorId == user.Id
                             select post).ToList(),
                    Comments = (from comment in db.Comments
                                where comment.AuthorId == user.Id
                                select comment)
                };
                return View(userModel);
            }
            return View();
        }
        public ActionResult OpenPost(string postId)
        {
            var post = (from item in db.Posts
                        where item.Id == postId
                        select item).ToList()[0];
            PostModel postModel = new PostModel
            {
                Id = post.Id,
                AuthorName = ((from user in db.Users
                               where user.Id == post.AuthorId
                               select user).ToList())[0].UserName,
                Date = post.Date,
                Title = post.Title,
                Image = post.Image,
                Text = post.Text,
                CommentsNumber = (from comment in db.Comments
                                  where comment.PostId == post.Id
                                  select comment).ToList().Count,               
                Comments = (from comment in db.Comments
                            where comment.PostId == post.Id
                            select comment)
            };

            return View(postModel);
        }

        private void DeletePosts(string userId)
        {
            var posts = (from post in db.Posts
                         where post.AuthorId == userId
                         select post).ToList();
            foreach (var post in posts)
            {
                DeleteComments(postId: post.Id);
                db.Posts.Remove(post);
            }
            db.SaveChanges();
        }
        private void DeleteComments(string userId = "", string postId = "")
        {
            if (postId != "")
            {
                var comments = (from c in db.Comments
                                where c.PostId == postId
                                select c).ToList();
                foreach (var comment in comments)
                {
                    db.Comments.Remove(comment);
                }
                db.SaveChanges();
            }
            if (userId != "")
            {
                var comments = (from c in db.Comments
                                where c.AuthorId == userId
                                select c).ToList();
                foreach (var comment in comments)
                {
                    db.Comments.Remove(comment);
                }
                db.SaveChanges();
            }
        }

        private IEnumerable<PostModel> Posts
        {
            get
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
                                          select comment).Count()
                    });
                }
                return postModels;
            }
        }
        private IEnumerable<CommentModel> Comments
        {
            get
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
                        Text = comment.Text.Length >= 20 ? comment.Text.Substring(0, 17) + "..." : comment.Text,
                        Date = comment.Date,
                        Id = comment.Id,
                        PostId = comment.PostId,
                        ParentId = comment.ParentId == null ? "" : comment.ParentId
                    });
                }
                return commentModels;
            }
        }
        private IEnumerable<UserModel> Users
        {
            get
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
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}