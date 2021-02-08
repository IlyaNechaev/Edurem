using Edurem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edurem.ViewModels
{
    public class AccountViewModel<TViewModel>
    {
        public User CurrentUser { get; set; }

        public TViewModel ViewModel { get; set; }
    }

    public class AccountViewModel
    {
        public User CurrentUser { get; set; }
    }
}
