#pragma checksum "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\SubjectsDropdown.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e4350b92dc4092c304e924ca36cbfde108bdcfe3"
// <auto-generated/>
#pragma warning disable 1591
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
#line 1 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\SubjectsDropdown.razor"
using Edurem.Models;

#line default
#line hidden
#nullable disable
    public partial class SubjectsDropdown : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<div class=\"row mt-3\"><div class=\"col-12\"><span class=\"h5\">Дисциплина</span></div></div>\r\n");
            __builder.OpenElement(1, "div");
            __builder.AddAttribute(2, "class", "row align-items-center");
            __builder.OpenElement(3, "div");
            __builder.AddAttribute(4, "class", "col-5");
            __builder.OpenElement(5, "select");
            __builder.AddAttribute(6, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.ChangeEventArgs>(this, 
#nullable restore
#line 12 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\SubjectsDropdown.razor"
                           SelectedSubjectChanged

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(7, "class", "form-control");
#nullable restore
#line 13 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\SubjectsDropdown.razor"
             foreach (var subject in AvailableSubjects)
            {

#line default
#line hidden
#nullable disable
            __builder.OpenElement(8, "option");
            __builder.AddAttribute(9, "value", 
#nullable restore
#line 15 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\SubjectsDropdown.razor"
                                subject

#line default
#line hidden
#nullable disable
            );
            __builder.AddContent(10, 
#nullable restore
#line 15 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\SubjectsDropdown.razor"
                                          subject.Name

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
#nullable restore
#line 16 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\SubjectsDropdown.razor"
            }

#line default
#line hidden
#nullable disable
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(11, "\r\n    ");
            __builder.OpenElement(12, "div");
            __builder.AddAttribute(13, "class", "col-1 text-center");
            __builder.OpenElement(14, "a");
            __builder.AddAttribute(15, "href", "");
            __builder.AddAttribute(16, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 20 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\SubjectsDropdown.razor"
                             () => { IsAddSubjectOpened = true; }

#line default
#line hidden
#nullable disable
            ));
            __builder.AddEventPreventDefaultAttribute(17, "onclick", true);
            __builder.AddMarkupContent(18, "<i class=\"fas fa-plus\" style=\"color: green; font-size: 25px\"></i>");
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(19, "\r\n\r\n");
            __builder.OpenComponent<MatBlazor.MatDialog>(20);
            __builder.AddAttribute(21, "IsOpen", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Boolean>(
#nullable restore
#line 26 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\SubjectsDropdown.razor"
                          IsAddSubjectOpened

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(22, "IsOpenChanged", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.Boolean>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.Boolean>(this, Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.CreateInferredEventCallback(this, __value => IsAddSubjectOpened = __value, IsAddSubjectOpened))));
            __builder.AddAttribute(23, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.OpenComponent<MatBlazor.MatDialogTitle>(24);
                __builder2.AddAttribute(25, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder3) => {
                    __builder3.AddMarkupContent(26, "<div class=\"col-12 text-left\"><p style=\"font-size: 20px\">Добавить дисциплину</p></div>");
                }
                ));
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(27, "\r\n    ");
                __builder2.OpenComponent<MatBlazor.MatDialogContent>(28);
                __builder2.AddAttribute(29, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder3) => {
                    __builder3.OpenElement(30, "input");
                    __builder3.AddAttribute(31, "placeholder", "Название дисциплины");
                    __builder3.AddAttribute(32, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 33 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\SubjectsDropdown.razor"
                                                              SubjectName

#line default
#line hidden
#nullable disable
                    ));
                    __builder3.AddAttribute(33, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => SubjectName = __value, SubjectName));
                    __builder3.SetUpdatesAttributeName("value");
                    __builder3.CloseElement();
                }
                ));
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(34, "\r\n    ");
                __builder2.OpenComponent<MatBlazor.MatDialogActions>(35);
                __builder2.AddAttribute(36, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder3) => {
                    __builder3.OpenComponent<MatBlazor.MatButton>(37);
                    __builder3.AddAttribute(38, "Class", "btn-primary");
                    __builder3.AddAttribute(39, "Unelevated", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Boolean>(
#nullable restore
#line 36 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\SubjectsDropdown.razor"
                                                   true

#line default
#line hidden
#nullable disable
                    ));
                    __builder3.AddAttribute(40, "Type", "button");
                    __builder3.AddAttribute(41, "OnClick", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<Microsoft.AspNetCore.Components.Web.MouseEventArgs>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 36 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\SubjectsDropdown.razor"
                                                                                  AddSubject

#line default
#line hidden
#nullable disable
                    )));
                    __builder3.AddAttribute(42, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder4) => {
                        __builder4.AddMarkupContent(43, "Добавить");
                    }
                    ));
                    __builder3.CloseComponent();
                    __builder3.AddMarkupContent(44, "\r\n        ");
                    __builder3.OpenComponent<MatBlazor.MatButton>(45);
                    __builder3.AddAttribute(46, "Class", "btn-danger");
                    __builder3.AddAttribute(47, "Unelevated", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Boolean>(
#nullable restore
#line 37 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\SubjectsDropdown.razor"
                                                  true

#line default
#line hidden
#nullable disable
                    ));
                    __builder3.AddAttribute(48, "Type", "button");
                    __builder3.AddAttribute(49, "OnClick", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<Microsoft.AspNetCore.Components.Web.MouseEventArgs>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 37 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\SubjectsDropdown.razor"
                                                                                 e => { IsAddSubjectOpened = false; }

#line default
#line hidden
#nullable disable
                    )));
                    __builder3.AddAttribute(50, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder4) => {
                        __builder4.AddMarkupContent(51, "Закрыть");
                    }
                    ));
                    __builder3.CloseComponent();
                }
                ));
                __builder2.CloseComponent();
            }
            ));
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
#nullable restore
#line 41 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\SubjectsDropdown.razor"
       

    [Parameter]
    public User CurrentUser { get; set; }

    int SubjectId { get; set; }

    List<Subject> AvailableSubjects { get; set; }

    public bool IsAddSubjectOpened { get; set; }

    string SubjectName { get; set; }

    protected override void OnInitialized()
    {
        Task.WaitAll(
            Task.Run(FillAvailableSubjects)
            );
    }

    async Task FillAvailableSubjects()
    {
        AvailableSubjects = new();
        AvailableSubjects.Add(new Subject() { Name = "-" });

        // Добавляем все имеющиеся дисциплины
        var subjects = await GroupService.GetUserSubjects(CurrentUser);
    }

    async Task AddSubject()
    {
        await GroupService.AddSubject(SubjectName, CurrentUser);
    }

    public Task SelectedSubjectChanged(ChangeEventArgs e)
    {
        SubjectId = ((Subject)e.Value).Id;

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
