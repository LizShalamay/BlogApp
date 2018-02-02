using BlogData;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogApp.Data.Entities;
using BlogApp.Data;
using BlogApp.Models;

namespace BlogApp.Controllers
{
    public class BlogController : Controller
    {
        BlogContext db = new BlogContext();
        public ActionResult CreatePost()
        {
            return PartialView("CreatePost");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePost(Post post, HttpPostedFileBase uploadImage)
        {
            byte[] imageData = null;
            using (var binaryReader = new BinaryReader(uploadImage.InputStream))
            {
                imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
            }
            post.Image = imageData;
            post.Id = Guid.NewGuid().ToString();
            post.Date = DateTime.Now;
            post.AuthorId = User.Identity.GetUserId();
            db.Posts.Add(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        private IEnumerable<Post> Posts()
        {
            BlogContext db = new BlogContext();
            string currentUserId = User.Identity.GetUserId();
            var posts = from item in db.Posts
                        where item.AuthorId == currentUserId
                        orderby item.Date descending
                        select item;
            return posts;
        }
        public ActionResult OpenPost(string postId)
        {
            var post = (from item in db.Posts
                        where item.Id == postId
                        select item).ToList();
            return PartialView(post[0]);
        }
        public ActionResult CreateComment(string parentId, string postId, bool answer)
        {
            if (answer)
            {
                var comments = (from item in db.Comments
                                where item.PostId == postId
                                orderby item.Date
                                select item).ToList();
                CommentListModel commentModels = new CommentListModel { Comments = new List<CommentModel>(), Seed = "" };
                foreach (var comment in comments)
                {
                    string author = (from item in db.Users
                                     where item.Id == comment.AuthorId
                                     select item.UserName).ToList().First();
                    commentModels.Comments.Add(new CommentModel
                    {
                        AuthorName = author,
                        Text = comment.Text,
                        Date = comment.Date,
                        Id = comment.Id,
                        PostId = comment.PostId,
                        ParentId = comment.ParentId == null ? "" : comment.ParentId,
                        Answer = comment.Id == parentId ? true : false
                    });
                }
                //return PartialView("Comments", commentModels);
            }
                return PartialView(new Comment { ParentId = parentId , PostId = postId, Date = DateTime.Now});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment(Comment comment)
        {
                comment.AuthorId = User.Identity.GetUserId();
                comment.Id = Guid.NewGuid().ToString();
                comment.ParentId = comment.ParentId;
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("OpenPost", new { postId = comment.PostId });
        }
        public ActionResult Comments(string postId)
        {
            var comments = (from item in db.Comments
                           where item.PostId == postId
                           orderby item.Date 
                           select item).ToList();
            CommentListModel commentModels = new CommentListModel { Comments = new List<CommentModel>(), Seed = "" };
            foreach (var comment in comments)
            {
                string author = (from item in db.Users
                                 where item.Id == comment.AuthorId
                                 select item.UserName).ToList().First();
                commentModels.Comments.Add(new CommentModel
                {
                    AuthorName = author,
                    Text = comment.Text,
                    Date = comment.Date,
                    Id = comment.Id,
                    PostId = comment.PostId,
                    ParentId = comment.ParentId == null ? "" : comment.ParentId,
                    Answer = false
                });
            }
            return PartialView(commentModels);
        }
        public ActionResult ShowNews(IEnumerable<PostModel> model)
        {
            var news = News();
            return PartialView("News", news);
        }
        public IEnumerable<PostModel> News()
        {
            var news = (from item in db.Posts
                       orderby item.Date descending
                       select item).ToList();
            List<PostModel> postModels = new List<PostModel>();
            foreach(var item in news)
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
        public string CommentsCount(string postId)
        {
            var comments = (from item in db.Comments
                            where item.PostId == postId
                            orderby item.Date descending
                            select item).ToList();
            return comments.Count.ToString();
        }
        public ActionResult Index()
        {
            var posts = Posts();
            
            return View(posts);
        }
    }
}