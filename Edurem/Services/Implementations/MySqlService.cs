using Edurem.Data;
using Edurem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Edurem.Services
{
    public class MySqlService : IDbService
    {
        EduremDbContext Context;
        public MySqlService([FromServices] EduremDbContext context)
        {
            Context = context;
        }

        public User GetUserById(int userId)
        {
            return GetUserByExpression(u => u.Id == userId);
        }

        public User GetUserByLogin(string userLogin)
        {
            return GetUserByExpression(u => u.Login == userLogin);
        }

        public User GetUserByEmail(string userEmail)
        {
            return GetUserByExpression(u => u.Email == userEmail);
        }

        private User GetUserByExpression(Expression<Func<User, bool>> expression)
        {
            var user = Context.Users.FirstOrDefault(expression);

            if (user != null && user.Roles == null)
                user.Roles = new List<UserRole>();

            return user;
        }

        public async Task AddUserAsync(User user, List<Role> userRoles = null)
        {
            await Context.Users.AddAsync(user);
            await Context.NotificationOptions.AddAsync(new NotificationOptions() { UserToNotify = user });
            await Context.SaveChangesAsync();

            if (userRoles != null)
            {
                await SetUserRolesAsync(user, userRoles);
            }
        }

        public async Task RemoveUserAsync(User user)
        {
            Context.UsersRoles.RemoveRange(user.Roles);
            Context.Users.Remove(user);
            await Context.SaveChangesAsync();
        }

        public async Task SetUserRolesAsync(User user, List<Role> userRoles)
        {

            foreach (Role role in userRoles)
            {
                await AddUserRole(user, role);
            }

            await Context.SaveChangesAsync();

            // ЛОКАЛЬНАЯ ФУНКЦИЯ
            // Добавление пользователю роли
            async Task AddUserRole(User user, Role role)
            {
                UserRole userRole = new UserRole();

                userRole.Role = role;
                userRole.RoleId = role.Id;
                role.Users.Add(userRole);

                userRole.User = user;
                userRole.UserId = user.Id;
                user.Roles.Add(userRole);

                await Context.UsersRoles.AddAsync(userRole);
            }
        }

        public async Task UpdateUser(User newUser)
        {
            var users = Context.Users.ToList();

            // Находим по Id индекс пользователя, который присутсвует в БД
            int userIndex = users.IndexOf(GetUserById(newUser.Id));

            if (userIndex == -1)
                throw new DatabaseServiceException("There is no user with such Id in the Database");
            else
            {
                await Task.Run(() =>
                {
                    users[userIndex] = newUser;
                });

                await Context.SaveChangesAsync();
            }
        }

        public NotificationOptions GetUserNotificationOptions(User user)
        {
            return Context.NotificationOptions.AsQueryable().FirstOrDefault(not => not.UserId == user.Id);
        }

        public async Task<List<(Group, RoleInGroup)>> GetUserGroups(User user)
        {
            List<(Group, RoleInGroup)> groupsWithRoles = new();

            var groupMembers = await Context.GroupsMembers.Include(gm => gm.Group)
                                                          .Where(gm => gm.UserId == user.Id)
                                                          .ToListAsync();
            foreach (var groupMember in groupMembers)
            {
                groupsWithRoles.Add((groupMember.Group, groupMember.RoleInGroup));
            }

            return groupsWithRoles;
        }

        public async Task SetEntityProperty<EntityType, ValueType>(EntityType entity, string propertyName, ValueType propertyValue)
        {
            try
            {
                Context.Entry(entity).Property(propertyName).CurrentValue = propertyValue;
                await Context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ValueType GetEntityProperty<EntityType, ValueType>(EntityType entity, string propertyName)
        {
            ValueType result = default(ValueType);
            try
            {
                result = (ValueType)Context.Entry(entity).Property(propertyName).CurrentValue;
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public async Task AddGroupAsync(Group newGroup)
        {
            if (!Context.Groups.Any(group => group.Id == newGroup.Id))
            {
                await Context.Groups.AddAsync(newGroup);
                await Context.SaveChangesAsync();
            }
        }

        public async Task AddUserToGroupAsync(Group group, User user, RoleInGroup userRole)
        {
            if (!Context.GroupsMembers.Any(groupMember => groupMember.GroupId == group.Id && groupMember.UserId == user.Id))
            {
                await Context.GroupsMembers.AddAsync(new GroupMember() { Group = group, User = user, RoleInGroup = userRole });
                await Context.SaveChangesAsync();
            }
        }

        public async Task AddSubjectAsync(Subject subject)
        {
            await Context.Subjects.AddAsync(subject);
            await Context.SaveChangesAsync();
        }

        public async Task<List<Subject>> GetUserSubjectsAsync(User user)
        {
            return await Context.Subjects.Where(subject => subject.AuthorId == user.Id).ToListAsync();
        }
    }
}
