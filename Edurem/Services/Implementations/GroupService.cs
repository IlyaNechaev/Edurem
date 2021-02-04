﻿using Edurem.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Services
{
    public class GroupService : IGroupService
    {
        IDbService DbService { get; init; }

        public GroupService([FromServices] IDbService dbService)
        {
            DbService = dbService;
        }

        public async Task<List<(Group, RoleInGroup)>> GetUserGroups(User user)
        {
            return await DbService.GetUserGroups(user);
        }

        public async Task CreateGroup(Group group, User creator)
        {
            await DbService.AddGroupAsync(group);

            await DbService.AddUserToGroupAsync(group, creator, RoleInGroup.ADMIN);
        }

        public async Task AddSubject(string subjectName, User user)
        {
            var subject = new Subject() { Author = user, Name = subjectName };
            await DbService.AddSubjectAsync(subject);
        }

        public async Task<List<Subject>> GetUserSubjects(User user)
        {
            return await DbService.GetUserSubjectsAsync(user);
        }
    }
}
