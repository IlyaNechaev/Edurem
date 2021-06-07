using Edurem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Data
{    
    public static partial class ModelBuilderConfiguration
    {
        /// <summary>
        /// Добавляет конфигурации для групп: <c><see cref="Group"/></c>, <c><see cref="GroupAdministrator"/></c>,
        /// <c><see cref="GroupMember"/></c>, <c><see cref="GroupPost"/></c>
        /// </summary>
        public static void AddGroupConfiguration(this ModelBuilder builder)
        {
            builder.ApplyConfiguration(new GroupConfiguration());
            builder.ApplyConfiguration(new GroupMemberConfiguration());
        }
    }

    // Конфигурация групп
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            // Первичный ключ
            builder
                .HasKey(group => group.Id);

            
            builder
                .HasMany(group => group.Members)
                .WithOne(member => member.Group);


            // Связь с таблицей group_posts
            builder
                .HasMany(group => group.Posts)
                .WithMany(post => post.Groups)
                .UsingEntity(builder => builder.ToTable("group_posts"));
        }
    }

    public class GroupMemberConfiguration : IEntityTypeConfiguration<GroupMember>
    {
        public void Configure(EntityTypeBuilder<GroupMember> builder)
        {
            // Первичный ключ
            builder
                .HasKey(member => new { member.GroupId, member.UserId });

            // Связь с таблицей groups
            builder
                .HasOne(admin => admin.Group)
                .WithMany(group => group.Members);            
        }
    }

    public class SubjectsConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            // Первичный ключ
            builder
                .HasKey(subject => subject.Id);

            builder
                .HasOne(subject => subject.Author);
                
        }
    }
}
