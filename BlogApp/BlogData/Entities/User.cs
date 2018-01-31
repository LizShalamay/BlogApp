using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BlogApp.Data.Entities
{
    public class User : IdentityUser
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
