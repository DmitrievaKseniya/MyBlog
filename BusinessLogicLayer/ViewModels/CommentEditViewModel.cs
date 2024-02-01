using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.ViewModels
{
    public class CommentEditViewModel
    {
        [Required(ErrorMessage = "Поле ID комментария обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "ИД комментария", Prompt = "Введите ИД комментария")]
        public int IdComment { get; set; }

        [Required(ErrorMessage = "Нельзя оставить пустой комментарий")]
        [DataType(DataType.Text)]
        [Display(Name = "Комментарий", Prompt = "Напишите комментарий")]
        public string? Text { get; set; }
    }
}
