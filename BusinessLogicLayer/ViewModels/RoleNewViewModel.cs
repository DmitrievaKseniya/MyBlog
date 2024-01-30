using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.ViewModels
{
    public class RoleNewViewModel
    {
        [Required(ErrorMessage = "Поле Название обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Название роли", Prompt = "Название")]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Описание роли", Prompt = "Описание")]
        public string Description { get; set; }
    }
}
