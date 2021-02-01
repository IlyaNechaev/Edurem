using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edurem.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edurem.Data
{
    public static partial class ModelBuilderConfiguration
    {
        /// <summary>
        /// Добавляет конфигурации для ролей пользователей: <c><see cref="Role"/></c>, <c><see cref="UserRole"/></c>
        /// </summary>
        public static void AddRoleConfiguration(this ModelBuilder builder)
        {
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new UserRoleConfiguration());
        }
    }

    // Конфигурация модели Role
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder
                .HasMany(role => role.Users)
                .WithOne(userRole => userRole.Role);

            builder
                .HasData(
                    Enum.GetValues(typeof(Roles))
                        .Cast<Roles>()
                        .Select(role => new Role(role))
                );
        }
    }
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            // Первичный ключ
            builder
                .HasKey(userRole => new { userRole.UserId, userRole.RoleId });
        }
    }
}
