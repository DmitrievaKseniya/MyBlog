﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.BLL.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Text { get; set; }
        public string AuthorId { get; set; }
        public User Author { get; set; }
        public DateTime DateTimeArticle { get; set; }
        public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
    }
}
