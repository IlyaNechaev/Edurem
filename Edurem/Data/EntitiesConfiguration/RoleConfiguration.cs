using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edurem.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edurem.Data
{
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
}
