using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using Edurem.Data;
using Edurem.Models;
using Edurem.Services;
using Edurem.ViewModels;
using MatBlazor;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StardustDL.RazorComponents.Markdown;
using MimeKit;
using Pomelo.EntityFrameworkCore.MySql;
using System.Net.Http;
using Edurem.Providers;
using Blazored.LocalStorage;

namespace Edurem
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Configuration["ApplicationName"] = "Edurem";
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddServerSideBlazor();
            services.AddRazorPages();
            services.AddMarkdownComponent();
            services.AddHttpContextAccessor();
            services.AddBlazoredLocalStorage();


            // ��������� ������������ Toaster
            services.AddMatToaster(config =>
            {
                config.Position = MatToastPosition.BottomRight;
                config.PreventDuplicates = true;
                config.NewestOnTop = true;
                config.ShowCloseButton = true;
                config.ShowProgressBar = true;
                config.MaximumOpacity = 95;

                config.ShowTransitionDuration = 500;
                config.VisibleStateDuration = 5000;
                config.HideTransitionDuration = 1000;

                config.RequireInteraction = false;
            });

            // Replace with your connection string.
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            var serverVersion = ServerVersion.AutoDetect(connectionString);

            services.AddDbContext<DbContext, EduremDbContext>(options =>
                options.UseMySql(
                        serverVersion,
                        builder => builder.EnableRetryOnFailure()),
                    ServiceLifetime.Transient
            );

            services.AddTransient<ISecurityService, SecurityService>();
            services.AddTransient<IDbService, MySqlService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IGroupService, GroupService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IEmailService, MimeEmailService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<ICodeTestService, TestService>();
            services.AddTransient<IDockerService, ConsoleDockerService>();
            services.AddTransient<IMarkdownService, Services.Markdig>();
            services.AddTransient<ICookieService, CookieService>();
            services.AddTransient<LanguageTestProviderFactory>();
            services.AddScoped<IRepositoryFactory, RepositoryFactory>();

            services.AddHttpClient();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/login");
                    options.LogoutPath = new PathString("/logout");
                });

            services.AddAuthorization(options =>
                    {
                        options.AddPolicy("AccessDenied", policy => policy.RequireClaim(ClaimKey.Status, "REGISTERED")
                                                                          .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
                                                                          .Build());

                        options.AddPolicy("AuthenticatedOnly", policy => policy.RequireAuthenticatedUser()
                                                                               .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
                                                                               .Build());
                    });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(15);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapFallbackToController("Default", "Home");
                endpoints.MapBlazorHub();
                endpoints.MapRazorPages();
            });
        }
    }
}
