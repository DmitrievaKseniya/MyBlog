using MyBlog.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.ViewModels
{
    public class ArticleViewModel
    {
        public Article Article { get; set; }

        public ArticleViewModel(Article article)
        {
            Article = article;
        }

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public CommentNewViewModel NewCommentVM { get; set; }
    }
}
