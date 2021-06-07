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
        /// Добавляет конфигурации для записей: <c><see cref="PostModel"/></c>, <c><see cref="PostFile"/></c>,
        /// <c><see cref="Discussion"/></c>
        /// </summary>
        public static void AddPostsConfiguration(this ModelBuilder builder)
        {
            builder.ApplyConfiguration(new PostsConfiguration());
            builder.ApplyConfiguration(new DiscussionsConfiguration());
            builder.ApplyConfiguration(new TestsConfiguration());
        }
    }

    public class PostsConfiguration : IEntityTypeConfiguration<PostModel>
    {
        public void Configure(EntityTypeBuilder<PostModel> builder)
        {
            // Первичный ключ
            builder
                .HasKey(post => post.Id);

            // Связь с таблицей posts_files
            builder
                .HasMany(post => post.AttachedFiles)
                .WithMany(file => file.Posts)
                .UsingEntity(j => j.ToTable("post_files"));
        }
    }

    public class DiscussionsConfiguration : IEntityTypeConfiguration<Discussion>
    {
        public void Configure(EntityTypeBuilder<Discussion> builder)
        {
            // Первичный ключ
            builder
                .HasKey(discussion => discussion.Id);

            // Связь с таблицей messages
            builder
                .HasMany(discussion => discussion.Messages)
                .WithOne(message => message.TargetedDiscussion);
        }
    }

    public class TestsConfiguration : IEntityTypeConfiguration<TestInfo>
    {
        public void Configure(EntityTypeBuilder<TestInfo> builder)
        {
            // Первичный ключ
            builder
                .HasKey(test => test.Id);

            builder
                .HasMany(test => test.FilesToTest)
                .WithMany(file => file.TestsInfo)
                .UsingEntity(j => j.ToTable("test_files"));
        }
    }
}
