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
            Notifications = new NotificationOptions();
        } 
        
        public NotificationOptions Notifications { get; set; }
    }    
}
