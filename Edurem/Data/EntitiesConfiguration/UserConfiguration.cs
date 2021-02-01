using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Edurem.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data;
using Edurem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Edurem.Services;
using Microsoft.AspNetCore.Components;

namespace Edurem.Data
{
    public static partial class ModelBuilderConfiguration
    {
        /// <summary>
        /// Добавляет конфигурации для пользователей: <c><see cref="User"/></c>, <c><see cref="NotificationOptions"/></c>
        /// </summary>
        public static void AddUserConfiguration(this ModelBuilder builder, ISecurityService securityService)
        {
            builder.ApplyConfiguration(new UserConfiguration(securityService));
            builder.ApplyConfiguration(new NotifOptionsConfiguration());
        }
    }

    // Конфигурация модели User
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        ISecurityService SecurityService { get; init; }
        public UserConfiguration(ISecurityService securityService)
        {
            SecurityService = securityService;
        }

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id).HasColumnType("int");

            builder
                .HasMany(user => user.Roles)
                .WithOne(userRole => userRole.User);

            // Добавочное поле для кода подтверждения Email
            
            builder
                .Property<string>("EmailConfirmCode")
                .HasColumnType("nvarchar(10)")
                .HasColumnName("EmailCode")
                .ValueGeneratedNever();

            CreateAdminUser(builder);
        }

        private void CreateAdminUser(EntityTypeBuilder<User> builder)
        {
            // Добавляем администратора
            var adminUser = new RegisterViewModel
            {
                Name = "Илья",
                Surname = "Нечаев",
                DateOfBirth = "01.01.2020",
                Password = "123",
                Login = "root",
                Gender = "MALE",
                Email = "ilia.nechaeff@yandex.ru"
            }.ToUser(SecurityService);

            adminUser.Id = 1;

            builder.HasData(adminUser);
        }
    }

    public class NotifOptionsConfiguration : IEntityTypeConfiguration<NotificationOptions>
    {
        public void Configure(EntityTypeBuilder<NotificationOptions> builder)
        {
            builder.HasKey(options => options.UserId);

            var notifOptions = new NotificationOptions
            {
                NewTasksToEmail = false,
                TaskResultToEmail = false,
                TeacherMessageToEmail = false,
                UserId = 1
            };

            builder
                .HasData(notifOptions);
        }
    }
}
