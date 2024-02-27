using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.Models
{
    public class Role : IdentityRole
    {
        public string? Description { get; set; }

        public Role(string name) : base(name) { }

        public ICollection<UserRole> UserRoles { get; set; }

        public Role(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
