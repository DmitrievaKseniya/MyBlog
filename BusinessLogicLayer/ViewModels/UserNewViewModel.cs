using MyBlog.BLL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.ViewModels
{
    public class UserNewViewModel : RegisterViewModel
    {
        public List<Role>? AllRoles { get; set; }

        [Display(Name = "Список ролей")]
        public List<string>? SelectedNameRoles { get; set; }
    }
}
