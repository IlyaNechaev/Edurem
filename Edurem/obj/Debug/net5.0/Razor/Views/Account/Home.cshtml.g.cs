#pragma checksum "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Views\Account\Home.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1fcb095a47f94fecef28a76cf272e2882a10acbb"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Account_Home), @"mvc.1.0.view", @"/Views/Account/Home.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Views\_ViewImports.cshtml"
using Edurem;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Views\_ViewImports.cshtml"
using Edurem.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1fcb095a47f94fecef28a76cf272e2882a10acbb", @"/Views/Account/Home.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d8dc01c1611c95810c04b897b325fe08080a1671", @"/Views/_ViewImports.cshtml")]
    public class Views_Account_Home : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Views\Account\Home.cshtml"
  
    var cookies = Context.Request.GetCookieValue(ClaimKey.CookiesId);
    ViewData["Title"] = CookieService.GetCookie(ClaimKey.Surname, cookies) + " " + CookieService.GetCookie(ClaimKey.Name, cookies);
    ViewData["ApplicationName"] = "Edurem";
    Layout = "_AccountLayout";

#line default
#line hidden
#nullable disable
            WriteLiteral("<div class=\"container-fluid\">\r\n</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public Edurem.Services.ICookieService CookieService { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
