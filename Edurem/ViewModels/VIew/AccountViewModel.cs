using Edurem.Models;
using Edurem.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.ViewModels
{
    public class AccountViewModel<TViewModel>
    {
        public CurrentUserViewModel CurrentUser { get; set; }

        public TViewModel ViewModel { get; set; }
    }

    public class AccountViewModel
    {
        public AccountViewModel(HttpContext context)
        {
            CurrentUser = new CurrentUserViewModel
            {
                Id = int.Parse(context.User.GetClaim(ClaimKey.Id)),
                Name = context.User.GetClaim(ClaimKey.Name),
                Surname = context.User.GetClaim(ClaimKey.Surname)
            };
        }
        public CurrentUserViewModel CurrentUser { get; set; }
    }
}
