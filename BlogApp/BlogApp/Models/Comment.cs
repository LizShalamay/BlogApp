using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Models 
{
    class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int AuthorId { get; set; }
        public int ParentId { get; set; }
        public DateTime Date { get; set; }
        public int Text { get; set; }
    }
}
