using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ViewModels
{
    public class TagNewViewModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "Название тега", Prompt = "Укажите название")]
        public string Name { get; set; }
    }
}
