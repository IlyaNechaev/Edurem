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
        string ConnectionString { get; init; }

        public DbSet<FileModel> Files { get; set; }

        // Пользователи
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<NotificationOptions> NotificationOptions { get; set; }

        // Группы
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupsMembers { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        // Записи
        public DbSet<PostModel> Posts { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<TestInfo> TestsInfo { get; set; }

        public EduremDbContext(DbContextOptions<EduremDbContext> builder,
                               [FromServices] ISecurityService securityService,
                               [FromServices] IConfiguration configuration)
        {
            SecurityService = securityService;

            ConnectionString = configuration.GetConnectionString("DefaultConnection");

            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                ConnectionString,
                ServerVersion.AutoDetect(ConnectionString)
            );
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Добавление конфигураций для моделей
            builder.AddUserConfiguration(SecurityService);
            builder.AddRoleConfiguration();
            builder.AddGroupConfiguration();
            builder.AddPostsConfiguration();
        }
    }
}
