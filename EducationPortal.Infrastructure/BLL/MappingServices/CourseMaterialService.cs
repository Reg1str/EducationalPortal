namespace EducationPortal.Infrastructure.BLL.MappingServices
{
    using EducationPortal.Application.Services.MappingInterfaces;
    using EducationPortal.Domain.Core.Mappings;
    using EducationPortal.Domain.Services.Interfaces;
    using EducationPortal.Infrastructure.BLL.Services;
    using Microsoft.Extensions.Logging;

    public class CourseMaterialService : GenericService<CourseMaterial>, ICourseMaterialService
    {
        public CourseMaterialService(IRepository<CourseMaterial> repository, ILogger<CourseMaterialService> logger)
            : base(repository, logger)
        {
        }
    }
}
