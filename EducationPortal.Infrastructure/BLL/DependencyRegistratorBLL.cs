namespace EducationPortal.Infrastructure.BLL
{
    using EducationPortal.Application.Services.Interfaces;
    using EducationPortal.Application.Services.MappingInterfaces;
    using EducationPortal.Application.Services.Validation;
    using EducationPortal.Infrastructure.Authentication;
    using EducationPortal.Infrastructure.BLL.MappingServices;
    using EducationPortal.Infrastructure.BLL.Services;
    using EducationPortal.Infrastructure.BLL.Validators;
    using EducationPortal.Infrastructure.DAL;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyRegistratorBLL
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAuthenticationService, AuthenticationService>()
                    .AddTransient<ICourseService, CourseService>()
                    .AddTransient<ICourseMaterialService, CourseMaterialService>()
                    .AddTransient<ISessionHandler, SessionHandler>()
                    .AddTransient<ISkillCourseService, SkillCourseService>()
                    .AddTransient<ISkillService, SkillService>()
                    .AddTransient<IUserService, UserService>()
                    .AddTransient<IUserCourseEnrollmentService, UserCourseEnrollmentService>()
                    .AddTransient<IUserSkillsService, UserSkillsService>()
                    .AddTransient<IUserCourseOwnerService, UserCourseOwnerService>()
                    .AddTransient<IMaterialService, MaterialService>()
                    .AddTransient<IVideoService, VideoService>();

            RegisterValidators(services, configuration);
            DependencyRegistratorDAL.Register(services, configuration);
        }

        private static void RegisterValidators(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ICourseValidator, CourseValidator>(
                provider => new CourseValidator(
                    configuration.GetSection("CourseValidatorSettings:MinimalValidTitleLength").Get<int>(),
                    configuration.GetSection("CourseValidatorSettings:MinimalValidDescriptionLength").Get<int>()))
                    .AddTransient<IUserCourseEnrollmentValidator, UserCourseEnrollmentValidator>(
                provider => new UserCourseEnrollmentValidator(
                    configuration.GetSection("UserCourseEnrollmentValidatorSettings:MaximalProgressPercentage").Get<int>(),
                    configuration.GetSection("UserCourseEnrollmentValidatorSettings:MinimalProgressPercentage").Get<int>()));
        }
    }
}
