﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edurem.Models;

namespace Edurem.Services
{
    public interface IGroupService
    {

        // Возвращает группы в которых состоит пользователь
        public Task<List<(Group Group, RoleInGroup UserRole)>> GetUserGroups(User user);

        public Task CreateGroup(Group group, User creator);
    }
}
