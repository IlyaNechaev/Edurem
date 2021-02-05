using Edurem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Edurem.Data.Repositories
{
    public class Repository<TEntity>
        where TEntity : class
    {
        private DbContext Context { get; init; }

        public Repository(DbContext context) => Context = context;

        public async Task Add(TEntity entity)
        {
            try
            {
                Context.Set<TEntity>().Add(entity);
            }
            catch (Exception)
            {
                throw;
            }

            await Context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            IEnumerable<TEntity> entities;
            try
            {
                entities = await Context.Set<TEntity>().ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return entities;
        }

        public async Task<IEnumerable<TEntity>> GetAll(params string[] inclusions)
        {
            IEnumerable<TEntity> entities;
            try
            {
                var contextEntites = Context.Set<TEntity>().AsQueryable();
                foreach (var inclusion in inclusions)
                {
                    contextEntites = contextEntites.Include(inclusion);
                }
                entities = await contextEntites.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return entities;
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            TEntity entity;
            try
            {
                entity = await Context.Set<TEntity>().FirstOrDefaultAsync(predicate);
            }
            catch (Exception)
            {
                throw;
            }

            return entity;
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate, params string[] inclusions)
        {
            TEntity entity;
            try
            {
                var contextEntites = Context.Set<TEntity>().AsQueryable();
                foreach (var inclusion in inclusions)
                {
                    contextEntites = contextEntites.Include(inclusion);
                }
                entity = await contextEntites.FirstOrDefaultAsync(predicate);
            }
            catch (Exception)
            {
                throw;
            }

            return entity;
        }

        public async Task<IEnumerable<TEntity>> Find(Func<TEntity, bool> predicate)
        {
            IEnumerable<TEntity> entities;

            try
            {
                entities = Context.Set<TEntity>()
                    .Where(predicate);

                await Task.FromResult(entities.ToList());
            }
            catch (Exception)
            {
                throw;
            }

            return entities;
        }

        public async Task<IEnumerable<TEntity>> Find(Func<TEntity, bool> predicate, params string[] inclusions)
        {
            IEnumerable<TEntity> entities;

            try
            {
                var contextEntites = Context.Set<TEntity>().AsQueryable();
                foreach (var inclusion in inclusions)
                {
                    contextEntites = contextEntites.Include(inclusion);
                }

                entities = contextEntites
                    .Where(predicate);

                await Task.FromResult(entities.ToList());
            }
            catch (Exception)
            {
                throw;
            }

            return entities;
        }

        public async Task Update(TEntity entity)
        {
            try
            {
                Context.Set<TEntity>().Update(entity);
            }
            catch (Exception)
            {
                throw;
            }

            await Context.SaveChangesAsync();
        }

        public async Task Delete(TEntity entity)
        {
            try
            {
                Context.Set<TEntity>().Remove(entity);
            }
            catch (Exception)
            {
                throw;
            }

            await Context.SaveChangesAsync();
        }
    }
}
