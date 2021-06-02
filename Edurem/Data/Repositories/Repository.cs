using Edurem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Edurem.Data
{
    public class Repository<TEntity>
        where TEntity : class
    {
        private DbContext Context { get; init; }

        public Repository(DbContext context) => Context = context;

        public async Task Add(TEntity entity, bool forceSave = true)
        {
            try
            {
                Context.Set<TEntity>().Add(entity);
                if (forceSave)
                {
                    await Context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                throw;
            }

        }

        public async Task AddRange(List<TEntity> entity, bool forceSave = true)
        {
            if (entity.Count == 0) return;
            try
            {
                Context.Set<TEntity>().AddRange(entity);
                if (forceSave)
                {
                    await Context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                throw;
            }

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
                entity = Context.Set<TEntity>().FirstOrDefault(predicate);
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
                entity = contextEntites.FirstOrDefault(predicate);
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

        public async Task Update(TEntity entity, bool forceSave = true)
        {
            try
            {
                Context.Set<TEntity>().Update(entity);

                if (forceSave)
                {
                    Context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Delete(TEntity entity, bool forceSave = true)
        {
            try
            {
                var s = Context.Set<TEntity>();
                Context.Set<TEntity>().Remove(entity);
                if (forceSave)
                {
                    Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task DeleteRange(IEnumerable<TEntity> entities, bool forceSave = true)
        {
            try
            {
                Context.Set<TEntity>().RemoveRange(entities);
                if (forceSave)
                {
                    await Context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
