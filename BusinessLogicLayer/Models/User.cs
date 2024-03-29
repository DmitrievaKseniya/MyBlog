﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public DateTime BirthDate { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }

        public string GetFullName()
        {
            return LastName + " " + FirstName + " " + MiddleName;
        }
    }
}
