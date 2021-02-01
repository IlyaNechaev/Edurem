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
            builder.ApplyConfiguration(new PostFilesConfiguration());
            builder.ApplyConfiguration(new DiscussionsConfiguration());
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
                .WithOne(file => file.Post);
        }
    }

    public class PostFilesConfiguration : IEntityTypeConfiguration<PostFile>
    {
        public void Configure(EntityTypeBuilder<PostFile> builder)
        {
            // Первичный ключ
            builder
                .HasKey(post => new { post.FileId, post.PostId });

            // Связь с таблицей posts
            builder
                .HasOne(fileModel => fileModel.Post)
                .WithMany(post => post.AttachedFiles);
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
}
