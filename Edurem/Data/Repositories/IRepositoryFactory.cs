using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Data
{
    public interface IRepositoryFactory
    {
        public Repository<TEntity> GetRepository<TEntity>() where TEntity : class;
    }
}
