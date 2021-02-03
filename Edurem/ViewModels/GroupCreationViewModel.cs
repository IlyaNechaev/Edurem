using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Edurem.Models;

namespace Edurem.ViewModels
{
    public class GroupCreationViewModel
    {
        [Required]
        public string Name { get; set; }

        public Group ToGroup()
        {
            var group = new Group();

            group.Name = Name;

            return group;
        }
    }
}
