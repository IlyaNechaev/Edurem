using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Edurem.Models;
using Edurem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Edurem.ViewModels
{
    public class RegisterEditModel
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Не указано имя")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Не указан email")]
        [EmailAddress(ErrorMessage = "Неверный формат email")]
        public string Email { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Не указан логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Введите пароль повторно")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        [MinLength(10, ErrorMessage = "Не выбрана дата")]
        public string DateOfBirth { get; set; }

        [Required(ErrorMessage = "Не выбран пол")]
        public string Gender { get; set; }

        [NotMapped]
        public string Redirect { get; set; }

        public User ToUser(ISecurityService securityService)
        {
            var user = new User();

            user.Name = Name;
            user.Surname = Surname;
            user.Login = Login;
            user.PasswordHash = securityService.GetPasswordHash(Password);
            user.Email = Email;
            user.EmailCode = null;
            user.DateOfBirth = new DateTime(
                int.Parse(DateOfBirth.Split('.')[2]),
                int.Parse(DateOfBirth.Split('.')[1]),
                int.Parse(DateOfBirth.Split('.')[0])
                );
            user.Gender = GetGenderDict().GetValueOrDefault(Gender);
            user.Roles = new List<UserRole>();
            user.Status = Status.REGISTERED;

            return user;

            // ЛОКАЛЬНАЯ ФУНКЦИЯ
            // Генерирует словарь (название пола; соответствующее значение в Models.Gender)
            Dictionary<string, Gender> GetGenderDict()
            {
                var genderDict = new Dictionary<string, Gender>();
                foreach (var gender in Enum.GetValues(typeof(Gender)))
                {
                    genderDict.Add(gender.ToString(), (Gender)gender);
                }
                return genderDict;
            }
        }
    }
}
