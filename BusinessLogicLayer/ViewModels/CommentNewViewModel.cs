using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ViewModels
{
    public class CommentNewViewModel
    {
        [Required(ErrorMessage = "Поле ID пользователя обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "ИД пользователя", Prompt = "Введите ИД пользователя")]
        public string IdUser { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Комментарий", Prompt = "Напишите ваш комментарий")]
        public string Text { get; set; }
    }
}
