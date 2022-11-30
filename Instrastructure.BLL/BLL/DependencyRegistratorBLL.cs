using EducationPortal.Application.Services.Interfaces;
using EducationPortal.Application.Services.MappingInterfaces;
using EducationPortal.Application.Services.Validation;
using EducationPortal.Infrastructure.Authentication;
using EducationPortal.Infrastructure.BLL.MappingServices;
using EducationPortal.Infrastructure.BLL.Services;
using EducationPortal.Infrastructure.BLL.Validators;
using EducationPortal.EntityFramework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Instrastructure.BLL.BLL.MappingServices;

namespace EducationPortal.Infrastructure.BLL
{
    public static class DependencyRegistratorBLL
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ICourseService, CourseService>()
                    .AddTransient<ICourseMaterialService, CourseMaterialService>()
                    .AddTransient<ISkillCourseService, SkillCourseService>()
                    .AddTransient<ISkillService, SkillService>()
                    .AddTransient<IVideoService, VideoService>()
                    .AddTransient<IPrintedMaterialService, PrintedMaterialService>()
                    .AddTransient<IArticleService, ArticleService>()
                    .AddTransient<IUserService, UserService>()
                    .AddTransient<IUserCourseEnrollmentService, UserCourseEnrollmentService>()
                    .AddTransient<IUserSkillsService, UserSkillsService>()
                    .AddTransient<IUserCourseOwnerService, UserCourseOwnerService>()
                    .AddTransient<IMaterialService, MaterialService>()
                    .AddTransient<IUserMaterialsService, UserMaterialsService>()
                    .AddTransient<IAuthorService, AuthorService>()
                    .AddTransient<IAuthorPrintedMaterialService, AuthorPrintedMaterialService>()
                    .AddTransient<IVideoService, VideoService>();

            RegisterValidators(services, configuration);
            DependencyRegistratorEntityFramework.Register(services, configuration);
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
