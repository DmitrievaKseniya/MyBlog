﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string AuthorId { get; set; }
        public User Author { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
}
