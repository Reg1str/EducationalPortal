namespace EducationPortal.Infrastructure.DAL
{
    using EducationPortal.Domain.Core.Entities;
    using EducationPortal.Domain.Core.Mappings;
    using EducationPortal.Domain.Services.Interfaces;
    using EducationPortal.EntityFramework;
    using EducationPortal.Infrastructure.DAL.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyRegistratorDAL
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            DALExtensionsRegistration(services);
            DALRepositoriesAndContextRegistration(services, configuration);
        }

        private static void DALExtensionsRegistration(IServiceCollection services)
        {
            services.AddTransient<JsonFormatter>()
                    .AddTransient<JsonSerializer>()
                    .AddTransient<JsonReaderAndWriter>()
                    .AddTransient<JsonFormatter>()
                    .AddTransient<ISerializer, JsonSerializer>();
        }

        private static void DALRepositoriesAndContextRegistration(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IRepository<CourseMaterial>, GenericRepository<CourseMaterial>>()
                    .AddTransient<IRepository<Material>, GenericRepository<Material>>()
                    .AddTransient<IRepository<SkillCourse>, GenericRepository<SkillCourse>>()
                    .AddTransient<IRepository<Course>, GenericRepository<Course>>()
                    .AddTransient<IRepository<Material>, GenericRepository<Material>>()
                    .AddTransient<IRepository<User>, GenericRepository<User>>()
                    .AddTransient<IRepository<UserSkills>, GenericRepository<UserSkills>>()
                    .AddTransient<IRepository<Skill>, GenericRepository<Skill>>()
                    .AddTransient<IRepository<UserCourseOwner>, GenericRepository<UserCourseOwner>>()
                    .AddTransient<IRepository<UserCourseEnrollment>, GenericRepository<UserCourseEnrollment>>()
                    .AddTransient<IRepository<Video>, GenericRepository<Video>>()
                    .AddTransient<DbContext, EfContext>(provider =>
                        new EfContext(configuration.GetConnectionString("SqlDataBase")));
        }
    }
}
