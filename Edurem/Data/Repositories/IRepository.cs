using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Data.Repositories
{
    public interface IRepository<TEntity> 
        where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> GetAll(params string[] inclusions);
        Task<TEntity> Get(int id);
        Task<TEntity> Get(int id, params string[] inclusions);
        Task<IEnumerable<TEntity>> Find(Func<TEntity, Boolean> predicate);
        Task<IEnumerable<TEntity>> Find(Func<TEntity, Boolean> predicate, params string[] inclusions);
        Task Add(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(TEntity entity);
    }
}
