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
            builder.ApplyConfiguration(new GroupPostsConfiguration());
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

            // Связь с таблицей groups_members
            builder
                .HasMany(group => group.Members)
                .WithOne(member => member.Group);


            // Связь с таблицей groups_posts
            builder
                .HasMany(group => group.GroupPosts)
                .WithOne(groupPost => groupPost.Group);
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

    public class GroupPostsConfiguration : IEntityTypeConfiguration<GroupPost>
    {
        public void Configure(EntityTypeBuilder<GroupPost> builder)
        {
            // Первичный ключ
            builder
                .HasKey(groupPost => new { groupPost.GroupId, groupPost.PostId });

            // Связь с таблицей groups
            builder
                .HasOne(groupPost => groupPost.Group)
                .WithMany(group => group.GroupPosts);
        }
    }
}
