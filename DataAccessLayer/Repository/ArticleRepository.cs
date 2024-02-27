using MyBlog.BLL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace MyBlog.DAL.Repository
{
    public class ArticleRepository : Repository<Article>
    {
        public ArticleRepository(ApplicationDbContext db) : base(db) { }

        public async Task<List<Article>> GetArticlesByUserId(string userId)
        {
            var articles = Set.Include(x => x.Author).Include(x => x.Tags).Where(x => x.AuthorId == userId).ToListAsync();

            return await articles;
        }

        public async override Task<List<Article>> GetAll()
        {
            var articles = Set.Include(x => x.Author).Include(x => x.Tags).ToListAsync();

            return await articles;
        }

        public async override Task<Article> Get(int id)
        {
            var article = Set.Include(x => x.Author).Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);

            return await article;
        }
    }
}
