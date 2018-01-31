using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Data.Entities
{
    public class Post
    {
        public string Id { get; set; }
        public string AuthorId { get; set; }
        public DateTime Date { get; set; }
        public byte[] Image { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
