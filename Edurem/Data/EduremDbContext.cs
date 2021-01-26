using Edurem.Models;
using Edurem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Data
{
    public class EduremDbContext : DbContext
    {
        ISecurityService SecurityService { get; init; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UsersRoles { get; set; }
        public DbSet<NotificationOptions> NotificationOptions { get; set; }

        public EduremDbContext(DbContextOptions<EduremDbContext> builder,
                               [FromServices] ISecurityService securityService) : base(builder)
        {
            SecurityService = securityService;

            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Добавление конфигураций для моделей
            builder.ApplyConfiguration(new UserConfiguration(SecurityService));
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new UserRoleConfiguration());
            builder.ApplyConfiguration(new NotifOptionsConfiguration());            
        }
    }
}
