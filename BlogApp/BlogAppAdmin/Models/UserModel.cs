using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogAppAdmin.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public int PostsNumber { get; set; }
        public int CommentsNumber { get; set; }
    }
}