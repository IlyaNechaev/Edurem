using Edurem.Data;
using Edurem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Edurem.Services
{
    public class MySqlService : IDbService
    {
        DbContext Context;
        public MySqlService([FromServices] DbContext context)
        {
            Context = context;
        }               

        public async Task SetEntityProperty<EntityType, ValueType>(EntityType entity, string propertyName, ValueType propertyValue)
        {
            try
            {
                Context.Entry(entity).Property(propertyName).CurrentValue = propertyValue;
                await Context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ValueType GetEntityProperty<EntityType, ValueType>(EntityType entity, string propertyName)
        {
            ValueType result = default(ValueType);
            try
            {
                result = (ValueType)Context.Entry(entity).Property(propertyName).CurrentValue;
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
    }
}
