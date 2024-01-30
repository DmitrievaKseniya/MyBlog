using MyBlog.BLL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.ViewModels
{
    public class ArticleEditViewModel
    {
        [Required(ErrorMessage = "Поле ID статьи обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "ИД статьи", Prompt = "Введите ИД статьи")]
        public int IdArticle { get; set; }

        public List<Tag>? AllTags { get; set; }

        [Required(ErrorMessage = "Необходимо выбрать хотя бы один тэг")]
        [Display(Name = "Список тэгов")]
        public List<int> SelectedIdTags { get; set; }

        public List<Tag>? SelectedTags { get; set; }

        [Required(ErrorMessage = "Поле Заголовок обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Заголовок", Prompt = "Введите заголовок")]
        public string Title { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Текст статьи", Prompt = "Напишите статью")]
        public string? Text { get; set; }

        
    }
}
