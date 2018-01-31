using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogApp.Models
{
    public class PostModel
    {
        public string AuthorName { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Id { get; set; }
    }
}