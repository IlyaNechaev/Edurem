#pragma checksum "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "dc4c2767ed15296f21a11704919223d0be3a134d"
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
#line 11 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\_Imports.razor"
using Edurem.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
using Edurem.ViewModels;

#line default
#line hidden
#nullable disable
    public partial class GroupsFilterSearch : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "row border-bottom mt-2 mb-2");
            __builder.OpenElement(2, "div");
            __builder.AddAttribute(3, "class", "col-12");
            __builder.OpenElement(4, "div");
            __builder.AddAttribute(5, "class", "row text-left align-items-center mb-2");
            __builder.OpenElement(6, "input");
            __builder.AddAttribute(7, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.ChangeEventArgs>(this, 
#nullable restore
#line 8 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                              e => GroupName = e.Value.ToString().ToLower()

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(8, "class", "col-4 col-md-5 col-xl-3 form-control");
            __builder.AddAttribute(9, "placeholder", "Название группы");
            __builder.CloseElement();
            __builder.AddMarkupContent(10, "\r\n            ");
            __builder.OpenElement(11, "div");
            __builder.AddAttribute(12, "class", "col-1");
            __builder.OpenElement(13, "a");
            __builder.AddAttribute(14, "href", "");
            __builder.AddAttribute(15, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 10 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                                     () => { IsFilterOpened = true; }

#line default
#line hidden
#nullable disable
            ));
            __builder.AddEventPreventDefaultAttribute(16, "onclick", 
#nullable restore
#line 10 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                                                                                                true

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(17, "<i class=\"fas fa-filter\" style=\"font-size: 20px\"></i>");
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(18, "\r\n        ");
            __builder.OpenElement(19, "div");
            __builder.AddAttribute(20, "class", "row text-left align-items-center mt-2 mb-2");
            __builder.OpenElement(21, "button");
            __builder.AddAttribute(22, "class", "col-2 col-md-2 col-xl-1 btn btn-primary text-center");
            __builder.AddAttribute(23, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 16 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                                                                                          SearchGroups

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(24, "Поиск");
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(25, "\r\n");
            __builder.OpenElement(26, "div");
            __builder.AddAttribute(27, "class", "row");
            __builder.OpenElement(28, "div");
            __builder.AddAttribute(29, "class", "col-12");
#nullable restore
#line 22 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
         if (GroupsList.GroupsForView.Count == 0)
        {

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(30, "<div class=\"row justify-content-center mt-4\"><h3 class=\"text-muted text-center mt-4\">Группы не найдены</h3></div>");
#nullable restore
#line 27 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 28 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
         foreach (var group in GroupsList.GroupsForView)
        {

#line default
#line hidden
#nullable disable
            __builder.OpenElement(31, "div");
            __builder.AddAttribute(32, "class", "row justify-content-center mt-4");
            __builder.OpenElement(33, "div");
            __builder.AddAttribute(34, "class", "col-12 col-lg-10 card");
            __builder.OpenElement(35, "div");
            __builder.AddAttribute(36, "class", "card-header");
            __builder.OpenElement(37, "a");
            __builder.AddAttribute(38, "href", "");
            __builder.AddAttribute(39, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 33 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                                               e => NavigationManager.NavigateTo($"group/{group.Id}", true)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddEventPreventDefaultAttribute(40, "onclick", 
#nullable restore
#line 33 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                                                                                                                                       true

#line default
#line hidden
#nullable disable
            );
            __builder.OpenElement(41, "h5");
            __builder.AddContent(42, 
#nullable restore
#line 34 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                                 group.Name

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(43, "\r\n                    ");
            __builder.OpenElement(44, "div");
            __builder.AddAttribute(45, "class", "card-body");
            __builder.OpenElement(46, "div");
            __builder.AddAttribute(47, "class", "row justify-content-center");
            __builder.OpenElement(48, "div");
            __builder.AddAttribute(49, "class", "col-11");
#nullable restore
#line 40 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                                 foreach (var info in group.GroupInfo)
                                {

#line default
#line hidden
#nullable disable
            __builder.OpenElement(50, "div");
            __builder.AddAttribute(51, "class", "row");
            __builder.OpenElement(52, "div");
            __builder.AddAttribute(53, "class", "col-4 text-right");
            __builder.OpenElement(54, "p");
            __builder.OpenElement(55, "b");
            __builder.AddContent(56, 
#nullable restore
#line 44 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                                                   info.Key

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddContent(57, ":");
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(58, "\r\n                                        ");
            __builder.OpenElement(59, "div");
            __builder.AddAttribute(60, "class", "col-8 text-left");
            __builder.OpenElement(61, "p");
            __builder.AddContent(62, 
#nullable restore
#line 47 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                                                info.Value

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
#nullable restore
#line 50 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                                }

#line default
#line hidden
#nullable disable
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.CloseElement();
#nullable restore
#line 56 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
        }

#line default
#line hidden
#nullable disable
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(63, "\r\n\r\n\r\n");
            __builder.OpenComponent<MatBlazor.MatThemeProvider>(64);
            __builder.AddAttribute(65, "Theme", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<MatBlazor.MatTheme>(
#nullable restore
#line 61 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                         theme

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(66, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.OpenComponent<MatBlazor.MatDialog>(67);
                __builder2.AddAttribute(68, "IsOpen", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Boolean>(
#nullable restore
#line 62 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                              IsFilterOpened

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(69, "IsOpenChanged", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.Boolean>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.Boolean>(this, Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.CreateInferredEventCallback(this, __value => IsFilterOpened = __value, IsFilterOpened))));
                __builder2.AddAttribute(70, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder3) => {
                    __builder3.OpenComponent<MatBlazor.MatDialogTitle>(71);
                    __builder3.AddAttribute(72, "Class", "text-left");
                    __builder3.AddAttribute(73, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder4) => {
                        __builder4.AddMarkupContent(74, "<p style=\"font-size: 30px\">Фильтр групп</p>");
                    }
                    ));
                    __builder3.CloseComponent();
                    __builder3.AddMarkupContent(75, "\r\n        ");
                    __builder3.OpenComponent<MatBlazor.MatDialogContent>(76);
                    __builder3.AddAttribute(77, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder4) => {
                        __builder4.OpenElement(78, "div");
                        __builder4.AddAttribute(79, "class", "row");
                        __builder4.OpenElement(80, "div");
                        __builder4.AddAttribute(81, "class", "col-6");
                        __builder4.AddMarkupContent(82, "<p style=\"font-size: 18px\">Минимальное число участников</p>\r\n                    ");
                        __Blazor.Edurem.Components.GroupsFilterSearch.TypeInference.CreateMatNumericUpDownField_0(__builder4, 83, 84, "Введите число", 85, 
#nullable restore
#line 72 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                                                          0

#line default
#line hidden
#nullable disable
                        , 86, 
#nullable restore
#line 73 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                                                    1

#line default
#line hidden
#nullable disable
                        , 87, 
#nullable restore
#line 73 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                                                                int.MaxValue

#line default
#line hidden
#nullable disable
                        , 88, 
#nullable restore
#line 71 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                                                        MinMembersCount

#line default
#line hidden
#nullable disable
                        , 89, Microsoft.AspNetCore.Components.EventCallback.Factory.Create(this, Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.CreateInferredEventCallback(this, __value => MinMembersCount = __value, MinMembersCount)), 90, () => MinMembersCount);
                        __builder4.CloseElement();
                        __builder4.AddMarkupContent(91, "\r\n                ");
                        __builder4.OpenElement(92, "div");
                        __builder4.AddAttribute(93, "class", "col-6");
                        __builder4.AddMarkupContent(94, "<p style=\"font-size: 18px\">Максимальное число участников</p>\r\n                    ");
                        __Blazor.Edurem.Components.GroupsFilterSearch.TypeInference.CreateMatNumericUpDownField_1(__builder4, 95, 96, "Введите число", 97, 
#nullable restore
#line 80 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                                                          0

#line default
#line hidden
#nullable disable
                        , 98, 
#nullable restore
#line 81 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                                                    1

#line default
#line hidden
#nullable disable
                        , 99, 
#nullable restore
#line 81 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                                                                int.MaxValue

#line default
#line hidden
#nullable disable
                        , 100, 
#nullable restore
#line 79 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                                                        MaxMembersCount

#line default
#line hidden
#nullable disable
                        , 101, Microsoft.AspNetCore.Components.EventCallback.Factory.Create(this, Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.CreateInferredEventCallback(this, __value => MaxMembersCount = __value, MaxMembersCount)), 102, () => MaxMembersCount);
                        __builder4.CloseElement();
                        __builder4.CloseElement();
                        __builder4.AddMarkupContent(103, "\r\n            ");
                        __builder4.OpenElement(104, "div");
                        __builder4.AddAttribute(105, "class", "row mt-2");
                        __builder4.OpenElement(106, "div");
                        __builder4.AddAttribute(107, "class", "col-6");
                        __builder4.AddMarkupContent(108, "<p class=\"mb-1\" style=\"font-size: 18px\">Роль в группе</p>\r\n                    ");
                        __builder4.OpenElement(109, "select");
                        __builder4.AddAttribute(110, "class", "form-control");
                        __builder4.AddAttribute(111, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.ChangeEventArgs>(this, 
#nullable restore
#line 88 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                                                            e => UserRole = (RoleInGroup)Enum.Parse(typeof(RoleInGroup), e.Value.ToString())

#line default
#line hidden
#nullable disable
                        ));
#nullable restore
#line 89 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                         foreach (var role in Roles)
                        {
                            

#line default
#line hidden
#nullable disable
#nullable restore
#line 91 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                             if (role.Role.Equals(UserRole))
                            {

#line default
#line hidden
#nullable disable
                        __builder4.OpenElement(112, "option");
                        __builder4.AddAttribute(113, "value", 
#nullable restore
#line 93 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                                                role.Role

#line default
#line hidden
#nullable disable
                        );
                        __builder4.AddAttribute(114, "selected");
                        __builder4.AddContent(115, 
#nullable restore
#line 93 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                                                                     role.Value

#line default
#line hidden
#nullable disable
                        );
                        __builder4.CloseElement();
#nullable restore
#line 94 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                            }
                            else
                            {

#line default
#line hidden
#nullable disable
                        __builder4.OpenElement(116, "option");
                        __builder4.AddAttribute(117, "value", 
#nullable restore
#line 97 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                                                role.Role

#line default
#line hidden
#nullable disable
                        );
                        __builder4.AddContent(118, 
#nullable restore
#line 97 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                                                            role.Value

#line default
#line hidden
#nullable disable
                        );
                        __builder4.CloseElement();
#nullable restore
#line 98 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                            }

#line default
#line hidden
#nullable disable
#nullable restore
#line 98 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                             
                        }

#line default
#line hidden
#nullable disable
                        __builder4.CloseElement();
                        __builder4.CloseElement();
                        __builder4.CloseElement();
                    }
                    ));
                    __builder3.CloseComponent();
                    __builder3.AddMarkupContent(119, "\r\n        ");
                    __builder3.OpenComponent<MatBlazor.MatDialogActions>(120);
                    __builder3.AddAttribute(121, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder4) => {
                        __builder4.OpenElement(122, "button");
                        __builder4.AddAttribute(123, "class", "btn btn-primary");
                        __builder4.AddAttribute(124, "type", "button");
                        __builder4.AddAttribute(125, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 105 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                                                                    (e => { IsFilterOpened = false; AcceptChanges(); })

#line default
#line hidden
#nullable disable
                        ));
                        __builder4.AddMarkupContent(126, "Готово");
                        __builder4.CloseElement();
                        __builder4.AddMarkupContent(127, "\r\n            ");
                        __builder4.OpenElement(128, "button");
                        __builder4.AddAttribute(129, "class", "btn btn-danger ml-2");
                        __builder4.AddAttribute(130, "type", "button");
                        __builder4.AddAttribute(131, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 106 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
                                                                        (e => { IsFilterOpened = false; CancelChanges(); })

#line default
#line hidden
#nullable disable
                        ));
                        __builder4.AddMarkupContent(132, "Отмена");
                        __builder4.CloseElement();
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
#line 111 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Components\GroupsFilterSearch.razor"
       

    [Parameter]
    public User CurrentUser { get; set; }

    GroupsListViewModel GroupsList { get; set; }

    bool IsFilterOpened { get; set; }

    GroupFilterOptions FilterOptions { get; set; }

    int MinMembersCount { get; set; }
    int MaxMembersCount { get; set; }
    RoleInGroup UserRole { get; set; }
    string GroupName { get; set; }

    MatTheme theme = new MatTheme
    {
        Primary = "#007bff", // Цвет кнопки
        Surface = "white" // Цвет фона диалогового окна
    };

    List<(RoleInGroup Role, string Value)> Roles;

    protected override void OnInitialized()
    {
        GroupName = string.Empty;
        Task.WaitAll(GetGroupViews());

        Roles = new()
        {
                (RoleInGroup.MEMBER | RoleInGroup.ADMIN, "-"),
                (RoleInGroup.ADMIN, "Администратор"),
                (RoleInGroup.MEMBER, "Участник")
            };

        FilterOptions = new()
        {
            MinMembersCount = 1,
            MaxMembersCount = int.MaxValue,
            UserRole = Roles[0].Role

        };

        IsFilterOpened = false;

        MinMembersCount = FilterOptions.MinMembersCount;
        MaxMembersCount = FilterOptions.MaxMembersCount;
        UserRole = FilterOptions.UserRole;
    }

    async Task GetGroupViews()
    {
        var groups = await GroupService.GetUserGroups(CurrentUser);

        GroupsList = new(groups);
    }

    void AcceptChanges()
    {
        FilterOptions.MinMembersCount = MinMembersCount;
        FilterOptions.MaxMembersCount = MaxMembersCount;
        FilterOptions.UserRole = UserRole;
    }

    void CancelChanges()
    {
        MinMembersCount = FilterOptions.MinMembersCount;
        MaxMembersCount = FilterOptions.MaxMembersCount;
        UserRole = FilterOptions.UserRole;
    }

    void SearchGroups()
    {
        GroupsList.FilterGroups(FilterOptions);
        GroupsList.GroupsForView = GroupsList.GroupsForView.Where(group => group.Name.ToLower().Contains(GroupName)).ToList();
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private NavigationManager NavigationManager { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private Edurem.Services.IGroupService GroupService { get; set; }
    }
}
namespace __Blazor.Edurem.Components.GroupsFilterSearch
{
    #line hidden
    internal static class TypeInference
    {
        public static void CreateMatNumericUpDownField_0<TValue>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, global::System.String __arg0, int __seq1, global::System.Int32 __arg1, int __seq2, TValue __arg2, int __seq3, TValue __arg3, int __seq4, TValue __arg4, int __seq5, global::Microsoft.AspNetCore.Components.EventCallback<TValue> __arg5, int __seq6, global::System.Linq.Expressions.Expression<global::System.Func<TValue>> __arg6)
        {
        __builder.OpenComponent<global::MatBlazor.MatNumericUpDownField<TValue>>(seq);
        __builder.AddAttribute(__seq0, "Label", __arg0);
        __builder.AddAttribute(__seq1, "DecimalPlaces", __arg1);
        __builder.AddAttribute(__seq2, "Minimum", __arg2);
        __builder.AddAttribute(__seq3, "Maximum", __arg3);
        __builder.AddAttribute(__seq4, "Value", __arg4);
        __builder.AddAttribute(__seq5, "ValueChanged", __arg5);
        __builder.AddAttribute(__seq6, "ValueExpression", __arg6);
        __builder.CloseComponent();
        }
        public static void CreateMatNumericUpDownField_1<TValue>(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder, int seq, int __seq0, global::System.String __arg0, int __seq1, global::System.Int32 __arg1, int __seq2, TValue __arg2, int __seq3, TValue __arg3, int __seq4, TValue __arg4, int __seq5, global::Microsoft.AspNetCore.Components.EventCallback<TValue> __arg5, int __seq6, global::System.Linq.Expressions.Expression<global::System.Func<TValue>> __arg6)
        {
        __builder.OpenComponent<global::MatBlazor.MatNumericUpDownField<TValue>>(seq);
        __builder.AddAttribute(__seq0, "Label", __arg0);
        __builder.AddAttribute(__seq1, "DecimalPlaces", __arg1);
        __builder.AddAttribute(__seq2, "Minimum", __arg2);
        __builder.AddAttribute(__seq3, "Maximum", __arg3);
        __builder.AddAttribute(__seq4, "Value", __arg4);
        __builder.AddAttribute(__seq5, "ValueChanged", __arg5);
        __builder.AddAttribute(__seq6, "ValueExpression", __arg6);
        __builder.CloseComponent();
        }
    }
}
#pragma warning restore 1591
