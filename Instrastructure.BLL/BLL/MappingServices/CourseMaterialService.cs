namespace EducationPortal.Infrastructure.BLL.MappingServices
{
    using EducationPortal.Application.Services.MappingInterfaces;
    using EducationPortal.Domain.Core.Common;
    using EducationPortal.Domain.Core.Mappings;
    using EducationPortal.Domain.Services.Interfaces;
    using EducationPortal.Infrastructure.BLL.Services;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    public class CourseMaterialService : GenericService<CourseMaterial>, ICourseMaterialService
    {
        public CourseMaterialService(
            IRepository<CourseMaterial> repository, 
            ILogger<CourseMaterialService> logger)
            : base(repository, logger)
        {
        }

        public async Task<ServiceResult<int>> CountMaterialsForCourse(int courseId)
        {
            try
            {
                var materialsCount =
                    await this.repository.Count(x => x.CourseId == courseId);

                return new ServiceResult<int>(materialsCount);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult<int>(ex);
            }
        }
    }
}
