#pragma checksum "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fad5ad5f69bc050b37517fc2971da2e73bd53c65"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fad5ad5f69bc050b37517fc2971da2e73bd53c65", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d8dc01c1611c95810c04b897b325fe08080a1671", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Views\Home\Index.cshtml"
  
    ViewData["Title"] = "Главная";
    ViewData["ApplicationName"] = "Edurem";
    Layout = "_Layout";


#line default
#line hidden
#nullable disable
            WriteLiteral("<div class=\"container-fluid\">\r\n\r\n    <header class=\"text-center\">\r\n        <h1 class=\"display-4 mt-2\">");
#nullable restore
#line 12 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Views\Home\Index.cshtml"
                              Write(AppConfiguration["ApplicationName"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</h1>
    </header>
    <main role=""main"" id=""changableMark"" style=""background-image: url('/file_system/Images/computer_1.jpg')"" class=""pb-3 align-items-center d-flex mt-5"">
        <div class=""row justify-content-center align-content-center w-100"" style=""height: 45%"">
            <div class=""col-10 col-lg-5 col-xl-4 d-flex align-items-center rounded h-100 mb-2 mb-md-0"">
                <input type=""button"" value=""Войти в систему"" class=""btn btn-light btn-outline-primary w-100 h-50""
                       style=""font-size: 260%; font-family: Georgia""");
            BeginWriteAttribute("onclick", " onclick=\"", 893, "\"", 959, 3);
            WriteAttributeValue("", 903, "window.location=\'", 903, 17, true);
#nullable restore
#line 18 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Views\Home\Index.cshtml"
WriteAttributeValue("", 920, Url.Action("Login", "Administration"), 920, 38, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 958, "\'", 958, 1, true);
            EndWriteAttribute();
            WriteLiteral(@" />
            </div>
            <div class=""col-10 col-lg-5 col-xl-4 d-flex align-items-center rounded h-100"">
                <input type=""button"" value=""Регистрация"" class=""btn btn-light btn-outline-dark w-100 h-50""
                       style=""font-size: 260%; font-family: Georgia""");
            BeginWriteAttribute("onclick", " onclick=\"", 1253, "\"", 1322, 3);
            WriteAttributeValue("", 1263, "window.location=\'", 1263, 17, true);
#nullable restore
#line 22 "D:\Институт\8 семестр\ВКР\Edurem\Edurem\Views\Home\Index.cshtml"
WriteAttributeValue("", 1280, Url.Action("Register", "Administration"), 1280, 41, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 1321, "\'", 1321, 1, true);
            EndWriteAttribute();
            WriteLiteral(@" />
            </div>
        </div>
    </main>

    <script>
        (function () {
            window.onresize = changeMarkSize;
            window.onload = changeMarkSize;

            function changeMarkSize() {
                let markHeight = window.innerHeight * 0.65;

                document.getElementById(""changableMark"").style.height = markHeight + ""px"";
            };
        })();
    </script>
</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public Microsoft.Extensions.Configuration.IConfiguration AppConfiguration { get; private set; }
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
