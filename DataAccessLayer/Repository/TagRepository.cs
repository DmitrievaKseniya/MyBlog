using Microsoft.EntityFrameworkCore;
using MyBlog.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.DAL.Repository
{
    public class TagRepository : Repository<Tag>
    {
        public TagRepository(ApplicationDbContext db) : base(db) { }

        public async Task<List<Tag>> GetAllTagsWithArticles()
        {
            var ListTags = Set.Include(x => x.Articles).ToListAsync();

            return await ListTags;
        }
    }
}
