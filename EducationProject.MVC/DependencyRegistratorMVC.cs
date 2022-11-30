using FluentValidation.AspNetCore;
using EducationPortal.Infrastructure.BLL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EducationProject.MVC.Managers;

namespace EducationProject.MVC
{
    public class DependencyRegistratorMVC
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<SignInManager>();
            services.AddControllersWithViews()
                    .AddFluentValidation(options =>
                    {
                        options.RegisterValidatorsFromAssemblyContaining<Startup>();
                    });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = new PathString("/Account/Login");
                    });
            DependencyRegistratorBLL.Register(services, configuration);
        }
    }
}
