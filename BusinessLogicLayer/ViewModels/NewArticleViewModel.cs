using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ViewModels
{
    public class NewArticleViewModel
    {
        [Required(ErrorMessage = "Поле ID пользователя обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "ИД пользователя", Prompt = "Введите ИД пользователя")]
        public string IdUser { get; set; }

        [Required(ErrorMessage = "Поле Заголовок обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Заголовок", Prompt = "Введите заголовок")]
        public string Title { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Текст статьи", Prompt = "Напишите статью")]
        public string? Text { get; set; }
    }
}
