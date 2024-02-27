using MyBlog.BLL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MyBlog.DAL.Repository
{
    public class CommentRepository : Repository<Comment>
    {
        public CommentRepository(ApplicationDbContext db) : base(db) { }

        public async override Task<List<Comment>> GetAll()
        {
            var comments = Set.Include(x => x.Author).Include(x => x.Article).ToListAsync();

            return await comments;
        }

        public async Task<List<Comment>> GetByArticleId(int id)
        {
            var comments = Set.Include(x => x.Author).Where(x => x.ArticleId == id).ToListAsync();

            return await comments;
        }

        public async Task<List<Comment>> GetByUserId(string id)
        {
            var comments = Set.Include(x => x.Article).Where(x => x.AuthorId == id).ToListAsync();

            return await comments;
        }

        public async override Task<Comment> Get(int id)
        {
            var comment = Set.Include(x => x.Author).Include(x => x.Article).FirstOrDefaultAsync(x => x.Id == id);

            return await comment;
        }
    }
}
