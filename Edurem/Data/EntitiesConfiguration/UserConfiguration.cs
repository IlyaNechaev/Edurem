﻿using System;
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
                .WithMany(role => role.Users)
                .UsingEntity(j => j.ToTable("user_roles"));

            builder
                .HasMany(user => user.Groups)
                .WithOne(groupMember => groupMember.User);

            CreateAdminUser(builder);
        }

        private void CreateAdminUser(EntityTypeBuilder<User> builder)
        {
            // Добавляем администратора
            var adminUser = new RegisterEditModel
            {
                Name = "Илья",
                Surname = "Нечаев",
                DateOfBirth = "01.01.2020",
                Password = "123",
                Login = "root",
                Gender = "MALE",
                Email = "ilia.nechaeff@yandex.ru"
            }
            .ToUser(SecurityService);

            adminUser.Id = 1;
            adminUser.OptionsId = 1;

            builder.HasData(adminUser);
        }
    }

    public class NotifOptionsConfiguration : IEntityTypeConfiguration<NotificationOptions>
    {
        public void Configure(EntityTypeBuilder<NotificationOptions> builder)
        {
            builder.HasKey(options => options.Id);

            var notifOptions = new NotificationOptions
            {
                Id = 1,
                NewTasksToEmail = false,
                TaskResultToEmail = false,
                TeacherMessageToEmail = false
            };

            builder
                .HasData(notifOptions);
        }
    }
}
