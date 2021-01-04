// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace Edurem.Components
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\_Imports.razor"
using MatBlazor;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\_Imports.razor"
using Edurem;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\_Imports.razor"
using Edurem.Components;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\_Imports.razor"
using Edurem.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\ConfirmEmail.razor"
using MimeKit;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\ConfirmEmail.razor"
using System.Net.Mail;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\ConfirmEmail.razor"
using Edurem.Services;

#line default
#line hidden
#nullable disable
    public partial class ConfirmEmail : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 44 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\ConfirmEmail.razor"
       
    [Parameter]
    public string Email { get; set; }

    private string Password { get; set; }

    private bool IsSendingFailed { get; set; }

    private bool IsSendingPerformed { get; set; }

    MatTheme theme = new MatTheme
    {
        Primary = "#FF8740", // Цвет кнопки
        Surface = "white" // Цвет фона диалогового окна
    };

    private async Task SendEmail()
    {
        IsSendingPerformed = true;

        var password = SecurityService.GeneratePassword();

        Password = password;

        var text = "Код подвтерждения электронной почты: <b>" + password + "</b>";
        var emailMessage = EmailService.CreateEmailMessage(text, "Подтверждение Email", ("ilia.nechaeff@yandex.ru", "Edurem"), (Email, ""));
        EmailService.SendCompleted += OnSendCompleted;

        await EmailService.SendEmailAsync(emailMessage, ("smtp.yandex.ru", 25, false), ("ilia.nechaeff@yandex.ru", "02081956Qw"));
    }

    private void Show(MatToastType type, string title, string message, string icon = "")
    {
        Toaster.Add(message, type, title, icon);
    }

    private void OnSendCompleted(object sender, SendCompletedEventArgs e)
    {
        if (e.IsFailed)
        {
            IsSendingFailed = true;
        }
        else
        {
            Show(MatToastType.Success, "Сообщение отправлено", "На указанную почту было отправлено сообщение с кодом подтверждения");
        }

        IsSendingPerformed = false;
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IMatToaster Toaster { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private Edurem.Services.ISecurityService SecurityService { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private Edurem.Services.IEmailService<MimeMessage> EmailService { get; set; }
    }
}
#pragma warning restore 1591
