using BusinessLogicLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class CommentRepository : Repository<Comment>
    {
        public CommentRepository(ApplicationDbContext db) : base(db) { }

        public async override Task<List<Comment>> GetAll()
        {
            var comments = Set.Include(x => x.Author).ToListAsync();

            return await comments;
        }

        public async override Task<Comment> Get(int id)
        {
            var comment = Set.Include(x => x.Author).FirstOrDefaultAsync(x => x.Id == id);

            return await comment;
        }
    }
}
