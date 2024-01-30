using MyBlog.BLL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.BLL.ViewModels
{
    public class UserEditViewModel
    {
        [Required]
        [Display(Name = "Идентификатор пользователя")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Имя", Prompt = "Введите имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Поле Фамилия обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Фамилия", Prompt = "Введите фамилию")]
        public string LastName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Отчество", Prompt = "Введите отчество")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Поле Email обязательно для заполнения")]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "example.com")]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        public DateTime BirthDate { get; set; }

        public string UserName => Email;

        public List<Role>? AllRoles {  get; set; }

        public List<Role>? SelectedRoles { get; set; }

        [Display(Name = "Список ролей")]
        public List<string>? SelectedNameRoles { get; set; }
    }
}
