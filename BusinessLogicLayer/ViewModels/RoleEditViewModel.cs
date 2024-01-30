using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.ViewModels
{
    public class RoleEditViewModel
    {
        [Required(ErrorMessage = "Поле ID роли обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "ИД роли", Prompt = "Введите ИД роли")]
        public string IdRole { get; set; }

        [Required(ErrorMessage = "Поле Название обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Название роли", Prompt = "Укажите название")]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Описание роли", Prompt = "Описание")]
        public string? Description { get; set; }
    }
}
