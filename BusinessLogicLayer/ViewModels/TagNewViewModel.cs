using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.ViewModels
{
    public class TagNewViewModel
    {
        [Required(ErrorMessage = "Поле Название обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Название тега", Prompt = "Укажите название")]
        public string Name { get; set; }
    }
}
