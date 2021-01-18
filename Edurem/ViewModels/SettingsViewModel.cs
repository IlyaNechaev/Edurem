using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edurem.Models;

namespace Edurem.ViewModels
{
    public class SettingsViewModel
    {
        public SettingsViewModel()
        {
            Profile = new User();
            Notifications = new NotificationOptions();
        }
        public User Profile { get; set; }     
        
        public NotificationOptions Notifications { get; set; }
    }    
}
