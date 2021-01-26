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
}
