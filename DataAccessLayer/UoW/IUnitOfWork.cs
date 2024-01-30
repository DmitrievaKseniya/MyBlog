using MyBlog.DAL.Repository;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.DAL.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges(bool ensureAutoHistory = false);

        IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = true) where TEntity : class;
    }
}
