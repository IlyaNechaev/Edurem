using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Surname { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Login { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; }

        [Column("Password", TypeName = "nvarchar(512)")]
        public string PasswordHash { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public Status Status { get; set; }

        public List<UserRole> Roles { get; set; }

        public bool IsEnabled => !Status.Equals(Status.DELETED) && !Status.Equals(Status.BLOCKED);

        public bool IsActivated => Status.Equals(Status.ACTIVATED);
    }

    [Flags]
    public enum Gender
    {
        MALE,
        FEMALE
    }

    [Flags]
    public enum Status
    {
        REGISTERED = 0,
        ACTIVATED = 1 << 0,
        BLOCKED = 1 << 1,
        DELETED = 1 << 2
    }
}
