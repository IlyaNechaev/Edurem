using Edurem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Data.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
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
                var contextEntites = Context.Set<TEntity>();
                foreach (var inclusion in inclusions)
                {
                    contextEntites.Include(inclusion);
                }
                entities = await contextEntites.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return entities;
        }

        public async Task<TEntity> Get(int id)
        {
            TEntity entity;
            try
            {
                entity = await Context.Set<TEntity>().FirstOrDefaultAsync(entity => entity.Id == id);
            }
            catch (Exception)
            {
                throw;
            }

            return entity;
        }

        public async Task<TEntity> Get(int id, params string[] inclusions)
        {
            TEntity entity;
            try
            {
                var contextEntites = Context.Set<TEntity>();
                foreach (var inclusion in inclusions)
                {
                    contextEntites.Include(inclusion);
                }
                entity = await contextEntites.FirstOrDefaultAsync(entity => entity.Id == id);
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
                entities = await Context.Set<TEntity>()
                    .Where(predicate)
                    .AsQueryable()
                    .ToListAsync();
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
                var contextEntites = Context.Set<TEntity>();
                foreach (var inclusion in inclusions)
                {
                    contextEntites.Include(inclusion);
                }

                entities = await contextEntites
                    .Where(predicate)
                    .AsQueryable()
                    .ToListAsync();
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
