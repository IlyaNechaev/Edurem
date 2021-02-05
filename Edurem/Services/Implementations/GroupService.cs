using Edurem.Data.Repositories;
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
        Repository<Group> GroupRepository { get; init; }
        Repository<User> UserRepository { get; init; }
        Repository<GroupMember> GroupMemberRepository { get; init; }
        Repository<Subject> SubjectRepository { get; init; }

        public GroupService([FromServices] DbContext context)
        {
            GroupRepository = new (context);
            UserRepository = new (context);
            GroupMemberRepository = new (context);
            SubjectRepository = new (context);
        }

        public async Task<List<(Group, RoleInGroup)>> GetUserGroups(User user)
        {
            var groupsMember = (await UserRepository.Get(u => u.Id == user.Id, nameof(user.Groups))).Groups;

            return groupsMember.Select(gm => (gm.Group, gm.RoleInGroup)).ToList();
        }

        public async Task CreateGroup(Group group, User creator)
        {
            await GroupRepository.Add(group);

            await GroupMemberRepository.Add(new GroupMember { GroupId = group.Id, UserId = creator.Id, RoleInGroup = RoleInGroup.ADMIN });
        }

        public async Task AddSubject(string subjectName, User user)
        {
            var subject = new Subject() { AuthorId = user.Id, Name = subjectName };

            await SubjectRepository.Add(subject);
        }

        public async Task<List<Subject>> GetUserSubjects(User user)
        {
            return (await SubjectRepository.Find(subject => subject.AuthorId == user.Id)).ToList();
        }
    }
}
