namespace EducationPortal.Infrastructure.BLL.MappingServices
{
    using EducationPortal.Application.Services.MappingInterfaces;
    using EducationPortal.Domain.Core.Mappings;
    using EducationPortal.Domain.Services.Interfaces;
    using EducationPortal.Infrastructure.BLL.Services;
    using Microsoft.Extensions.Logging;

    public class UserSkillsService : GenericService<UserSkills>, IUserSkillsService
    {
        public UserSkillsService(IRepository<UserSkills> repository, ILogger<UserSkillsService> logger)
            : base(repository, logger)
        {
        }
    }
}
