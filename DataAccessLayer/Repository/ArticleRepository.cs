using BusinessLogicLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class ArticleRepository : Repository<Article>
    {
        public ArticleRepository(ApplicationDbContext db) : base(db) { }

        public async Task<List<Article>> GetArticlesByUserId(string userId)
        {
            var articles = Set.Include(x => x.Author).Where(x => x.AuthorId == userId).ToListAsync();

            return await articles;
        }

        public async override Task<List<Article>> GetAll()
        {
            var articles = Set.Include(x => x.Author).ToListAsync();

            return await articles;
        }
    }
}
