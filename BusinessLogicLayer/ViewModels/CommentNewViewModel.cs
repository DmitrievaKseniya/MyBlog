using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.ViewModels
{
    public class CommentNewViewModel
    {
        [Required(ErrorMessage = "Поле ID пользователя обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "ИД пользователя", Prompt = "Введите ИД пользователя")]
        public string IdUser { get; set; }

        [Required(ErrorMessage = "Нельзя оставить пустой комментарий")]
        [DataType(DataType.Text)]
        [Display(Name = "Комментарий", Prompt = "Напишите ваш комментарий")]
        public string Text { get; set; }

        [Required(ErrorMessage = "Поле ID статьи обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "ИД статьи", Prompt = "Введите ИД статьи")]
        public int IdArticle { get; set; }
    }
}
