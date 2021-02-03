using System;
using System.Collections.Generic;
using System.Linq;
using Edurem.Models;
using System.Threading.Tasks;
using Edurem.Services;

namespace Edurem.ViewModels
{
    public class GroupViewModel
    {
        public int  Id { get; set; }

        public string Name { get; set; }

        public int MembersCount { get; set; }

        public RoleInGroup UserRole { get; set; }



        public GroupViewModel()
        {

        }
        public GroupViewModel(Group group, RoleInGroup role)
        {
            UserRole = role;

            Id = group.Id;
            Name = group.Name;
            MembersCount = group.Members.Count();
        }
    }

    public class GroupsListViewModel
    {
        // Все группы, в которых состоит пользователь
        // Данная коллекция меняется только при удалении или добавлении группы
        private List<GroupViewModel> Groups { get; set; }

        // Группы, к которым будут применяться фильтры
        // Данная коллекция меняется при фильтрации имеющихся групп
        public List<GroupViewModel> GroupsForView { get; set; }

        public GroupsListViewModel(List<(Group Group, RoleInGroup UserRole)> groups)
        {
            Groups = new();
            GroupsForView = new();

            groups.ForEach(group =>
            {
                Groups.Add(new(group.Group, group.UserRole));
            });

            GroupsForView = Groups;
        }

        public List<GroupViewModel> FilterGroups(GroupFilterOptions filterOptions)
        {
            GroupsForView = Groups;
            GroupsForView = GroupsForView.Where(group => (group.UserRole & filterOptions.UserRole) == group.UserRole).ToList();
            GroupsForView = GroupsForView.Where(group => group.MembersCount >= filterOptions.MinMembersCount).ToList();
            GroupsForView = GroupsForView.Where(group => group.MembersCount <= filterOptions.MaxMembersCount).ToList();

            return GroupsForView;
        }

        public List<GroupViewModel> ClearGroupsFilter()
        {
            GroupsForView = Groups;

            return GroupsForView;
        }
    }
}
