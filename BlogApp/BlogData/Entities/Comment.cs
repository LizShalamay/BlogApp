using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Data.Entities
{
    public class Comment
    {
        public string Id { get; set; }
        public string PostId { get; set; }
        public string AuthorId { get; set; }
        public string ParentId { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
    }
}
