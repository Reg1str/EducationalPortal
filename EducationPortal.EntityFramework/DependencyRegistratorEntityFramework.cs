namespace EducationPortal.EntityFramework
{
    using EducationPortal.Domain.Services.Interfaces;
    using EducationPortal.EntityFramework.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyRegistratorEntityFramework
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            DALRepositoriesAndContextRegistration(services, configuration);
        }

        private static void DALRepositoriesAndContextRegistration(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(typeof(IRepository<>), typeof(GenericRepository<>))
                    .AddTransient<DbContext, EfContext>(provider =>
                        new EfContext(configuration.GetConnectionString("SqlDataBase")));
        }
    }
}
