using Edurem.Models;
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
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UsersRoles { get; set; }

        public EduremDbContext(DbContextOptions<EduremDbContext> builder) : base(builder)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRole>()
                .HasKey(userRole => new { userRole.UserId, userRole.RoleId });

            builder.Entity<User>()
                .HasKey(user => user.Id);

            builder.Entity<User>()
                .HasMany(user => user.Roles)
                .WithOne(userRole => userRole.User);

            builder.Entity<Role>()
                .HasMany(role => role.Users)
                .WithOne(userRole => userRole.Role);

            builder.Entity<Role>().HasData(
                Enum.GetValues(typeof(Roles))
                    .Cast<Roles>()
                    .Select(role => new Role(role))
                );
        }
    }
}
