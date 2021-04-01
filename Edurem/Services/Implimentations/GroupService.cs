using Edurem.Data;
using Edurem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Services
{
    public class GroupService : IGroupService
    {
        IRepositoryFactory RepositoryFactory { get; init; }

        public GroupService([FromServices] IRepositoryFactory repositoryFactory)
        {
            RepositoryFactory = repositoryFactory;
        }

        public async Task<List<(Group, RoleInGroup)>> GetUserGroups(User user)
        {
            return await GetUserGroups(user.Id);
        }

        public async Task<List<(Group, RoleInGroup)>> GetUserGroups(int userId)
        {
            var GroupRepository = RepositoryFactory.GetRepository<Group>();
            // Находим группы, в которых состоит пользователь
            var groups = await GroupRepository.Find(group => group.Members.Any(gm => gm.UserId == userId), nameof(Group.Subject), nameof(Group.Members));

            return groups.Select(group => (group, group.Members.First(gm => gm.UserId == userId).RoleInGroup)).ToList();
        }

        public async Task CreateGroup(Group group, User creator)
        {
            var GroupRepository = RepositoryFactory.GetRepository<Group>();
            var GroupMemberRepository = RepositoryFactory.GetRepository<GroupMember>();

            await GroupRepository.Add(group);

            await GroupMemberRepository.Add(new GroupMember { GroupId = group.Id, UserId = creator.Id, RoleInGroup = RoleInGroup.ADMIN });
        }

        public async Task AddSubject(string subjectName, User user)
        {
            await AddSubject(subjectName, user.Id);
        }
        public async Task AddSubject(string subjectName, int userId)
        {
            var SubjectRepository = RepositoryFactory.GetRepository<Subject>();

            var subject = new Subject() { AuthorId = userId, Name = subjectName };

            await SubjectRepository.Add(subject);
        }

        public async Task<List<Subject>> GetUserSubjects(User user)
        {
            return await GetUserSubjects(user.Id);
        }

        public async Task<List<Subject>> GetUserSubjects(int userId)
        {
            var SubjectRepository = RepositoryFactory.GetRepository<Subject>();

            return (await SubjectRepository.Find(subject => subject.AuthorId == userId)).ToList();
        }

        public async Task<Group> GetGroup(int groupId)
        {
            var GroupRepository = RepositoryFactory.GetRepository<Group>();

            return await GroupRepository.Get(group => group.Id == groupId, nameof(Group.Subject), nameof(Group.Members));
        }

        public async Task<List<PostModel>> GetGroupPosts(int groupId, int startIndex = 0, int postsCount = 1)
        {
            var GroupPostRepository = RepositoryFactory.GetRepository<GroupPost>();
            var PostRepository = RepositoryFactory.GetRepository<PostModel>();

            // Находим идентификаторы публикаций для данной группы
            var postsId = (await GroupPostRepository.Find(gp => gp.GroupId == groupId)).Select(gp => gp.PostId);

            var posts = (await PostRepository.Find(post => postsId.Contains(post.Id), nameof(PostModel.AttachedFiles), nameof(PostModel.Author)));

            // Находим последние несколько постов (postsCount), начиная со startIndex
            return posts?
                .OrderBy(post => post.PublicationDate)?
                .Reverse()?
                .Skip(startIndex)?
                .Take(postsCount)?
                .ToList();
        }

        public async Task CreatePost(PostModel post, int groupId, List<FileModel> files = null)
        {
            var PostModelRepository = RepositoryFactory.GetRepository<PostModel>();
            var PostFileRepository = RepositoryFactory.GetRepository<PostFile>();
            var GroupPostRepository = RepositoryFactory.GetRepository<GroupPost>();

            try
            {
                // Добавляем новую публикацию в БД
                await PostModelRepository.Add(post);

                if (files is not null)
                {
                    // Создать связующие записи post_files
                    var postFiles = files.Select(file => new PostFile { FileId = file.Id, PostId = post.Id });

                    // Добавляем связующие записи в БД
                    foreach (var postFile in postFiles)
                    {
                        await PostFileRepository.Add(postFile);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            await GroupPostRepository.Add(new GroupPost { GroupId = groupId, PostId = post.Id });
        }
    }
}
