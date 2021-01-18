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

namespace Edurem.Data
{
    // Конфигурация модели User
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        ISecurityService SecurityService;
        public UserConfiguration([FromServices] ISecurityService securityService)
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
                .Property("EmailConfirmCode")
                .HasColumnType("nvarchar(10)")
                .HasColumnName("emailCode")
                .ValueGeneratedNever();

            var adminUser = new RegisterViewModel
            {
                Name = "Илья",
                Surname = "Нечаев",
                DateOfBirth = "2020.01.01",
                Password = "123",
                Login = "root",
                Gender = "MALE",
                Email = "ilia.nechaeff@yandex.ru"
            }
            .ToUser(SecurityService);

            builder.HasData(adminUser);
        }
    }
}
