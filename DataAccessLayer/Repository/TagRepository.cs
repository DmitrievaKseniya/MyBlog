using BusinessLogicLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class TagRepository : Repository<Tag>
    {
        public TagRepository(ApplicationDbContext db) : base(db) { }


    }
}
