﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.ViewModels
{
    public class TagEditViewModel
    {
        [Required(ErrorMessage = "Поле ID тега обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "ИД тега", Prompt = "Введите ИД тега")]
        public int IdTag { get; set; }

        [Required (ErrorMessage = "Поле Название обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Название тега", Prompt = "Укажите название")]
        public string Name { get; set; }
    }
}
