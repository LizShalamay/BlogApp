﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BlogApp.Core.Objects
{
    public class User : IdentityUser
    {
        //public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        //public virtual ICollection<Role> Roles { get; set; }
    }
}