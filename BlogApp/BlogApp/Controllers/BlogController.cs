using BlogApp.Core.DataBase;
using BlogApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BlogApp.Controllers
{
    public class BlogController : Controller
    {
        BlogContext db = new BlogContext();
        // Добавление
        public ActionResult CreatePost()
        {
            return PartialView("CreatePost");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePost(Post post, HttpPostedFileBase uploadImage)
        {
            byte[] imageData = null;
            // считываем переданный файл в массив байтов
            using (var binaryReader = new BinaryReader(uploadImage.InputStream))
            {
                imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
            }
            // установка массива байтов
            post.Image = imageData;
            post.Id = imageData.GetHashCode();
            post.Date = DateTime.Now;
            db.Posts.Add(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //private byte[] AddPicture()
        //{

        //}
        public ActionResult Index()
        {
            return View(db.Posts);
        }
    }
}