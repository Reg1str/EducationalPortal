namespace EducationPortal.Infrastructure.BLL.MappingServices
{
    using EducationPortal.Application.Services.MappingInterfaces;
    using EducationPortal.Domain.Core.Mappings;
    using EducationPortal.Domain.Services.Interfaces;
    using EducationPortal.Infrastructure.BLL.Services;
    using Microsoft.Extensions.Logging;

    public class SkillCourseService : GenericService<SkillCourse>, ISkillCourseService
    {
        public SkillCourseService(IRepository<SkillCourse> repository, ILogger<SkillCourseService> logger)
            : base(repository, logger)
        {
        }
    }
}
