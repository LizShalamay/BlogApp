using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogAppAdmin.Models
{
    public class PostModel
    {
        public string AuthorName { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Id { get; set; }
        public int CommentsNumber { get; set; }
    }
}