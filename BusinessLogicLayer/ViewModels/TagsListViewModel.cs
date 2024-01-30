using MyBlog.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.ViewModels
{
    public class TagsListViewModel
    {
        public Tag TagsList { get; set; }
        public int NumberArticles { get; set; }

    }
}
