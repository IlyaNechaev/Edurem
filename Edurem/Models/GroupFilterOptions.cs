using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Models
{
    public struct GroupFilterOptions
    {
        /// <summary>
        /// Роль пользователя в группе
        /// </summary>
        public RoleInGroup UserRole { get; set; }

        /// <summary>
        /// Максимальное число участников
        /// </summary>
        public int MaxMembersCount { get; set; }

        /// <summary>
        /// Минимальное число участников
        /// </summary>
        public int MinMembersCount { get; set; }
    }
}
