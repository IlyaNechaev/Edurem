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
#line 1 "D:\Институт\8 семестр\ВКР\Edurem_temp\Edurem\Components\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Институт\8 семестр\ВКР\Edurem_temp\Edurem\Components\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\Институт\8 семестр\ВКР\Edurem_temp\Edurem\Components\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\Институт\8 семестр\ВКР\Edurem_temp\Edurem\Components\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\Институт\8 семестр\ВКР\Edurem_temp\Edurem\Components\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\Институт\8 семестр\ВКР\Edurem_temp\Edurem\Components\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "D:\Институт\8 семестр\ВКР\Edurem_temp\Edurem\Components\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "D:\Институт\8 семестр\ВКР\Edurem_temp\Edurem\Components\_Imports.razor"
using MatBlazor;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "D:\Институт\8 семестр\ВКР\Edurem_temp\Edurem\Components\_Imports.razor"
using Edurem;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "D:\Институт\8 семестр\ВКР\Edurem_temp\Edurem\Components\_Imports.razor"
using Edurem.Components;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "D:\Институт\8 семестр\ВКР\Edurem_temp\Edurem\Components\SubjectsDropdown.razor"
using Edurem.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Институт\8 семестр\ВКР\Edurem_temp\Edurem\Components\SubjectsDropdown.razor"
using Edurem.ViewModels;

#line default
#line hidden
#nullable disable
    public partial class SubjectsDropdown : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 50 "D:\Институт\8 семестр\ВКР\Edurem_temp\Edurem\Components\SubjectsDropdown.razor"
       

    // Id авторизованного пользователя
    [Parameter]
    public int UserId { get; set; }

    [Parameter]
    public int SubjectId { get; set; }

    List<Subject> AvailableSubjects { get; set; }

    bool IsAddSubjectOpened { get; set; }

    string SubjectName { get; set; }

    protected override void OnInitialized()
    {
        Task.WaitAll(FillAvailableSubjects());
    }

    async Task FillAvailableSubjects()
    {
        AvailableSubjects = new();
        AvailableSubjects.Add(new Subject() { Name = "-", Id = 0 });

        // Добавляем все имеющиеся дисциплины
        (await GroupService.GetUserSubjects(UserId))
            .ForEach(AvailableSubjects.Add);
    }

    void AddSubject()
    {
        Task.WaitAll(
            Task.Run(async () =>
            {
                await GroupService.AddSubject(SubjectName, UserId);
                await FillAvailableSubjects();
            })
        );
    }

    public Task SelectedSubjectChanged(ChangeEventArgs e)
    {
        var s = e.Value.ToString();
        SubjectId = int.Parse(s);

        return UpdateSubject();
    }

    public async Task UpdateSubject()
    {
        await JSRuntime.InvokeVoidAsync("updateSubject", SubjectId);
    }


#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime JSRuntime { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private Edurem.Services.IGroupService GroupService { get; set; }
    }
}
#pragma warning restore 1591
