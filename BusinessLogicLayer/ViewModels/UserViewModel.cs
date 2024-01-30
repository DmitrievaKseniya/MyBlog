using MyBlog.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.ViewModels
{
    public class UserViewModel
    {
        public User User { get; set; }

        public UserViewModel(User user)
        {
            User = user;
        }
        public List<Role> Roles { get; set; }

        public UserViewModel(User user, List<Role> roles) : this(user)
        {
            Roles = roles;
        }

        public List<Article>? Articles { get; set; }
    }
}
