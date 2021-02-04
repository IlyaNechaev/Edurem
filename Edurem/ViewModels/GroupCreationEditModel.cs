using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Edurem.Models;

namespace Edurem.ViewModels
{
    public class GroupCreationEditModel
    {
        [Required(ErrorMessage = "Введите название группы")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Выберите дисциплину")]
        [Range(1, int.MaxValue)]
        public int SubjectId { get; set; }

        public Group ToGroup()
        {
            var group = new Group();

            group.Name = Name;

            return group;
        }
    }
}
