using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Objects
{
    class Post
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public DateTime Date { get; set; }
        public byte[] Image { get; set; }
        public string Text { get; set; }
    }
}
