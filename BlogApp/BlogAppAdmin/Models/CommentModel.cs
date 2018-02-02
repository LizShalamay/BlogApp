using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogAppAdmin.Models
{
    public class CommentModel
    {
        public string AuthorName { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string Id { get; set; }
        public string PostId { get; set; }
        public string ParentId { get; set; }
    }

    public class CommentListModel
    {
        public string Seed { get; set; }
        public List<CommentModel> Comments { get; set; }
    }
}