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

        [Range(1, int.MaxValue, ErrorMessage = "Выберите дисциплину")]
        public int SubjectId { get; set; }

        public Group ToGroup()
        {
            var group = new Group();

            group.Name = Name;
            group.SubjectId = SubjectId;

            return group;
        }
    }
}
