namespace EducationPortal.Infrastructure.BLL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EducationPortal.Application.Services.DTO;
    using EducationPortal.Application.Services.Interfaces;
    using EducationPortal.Application.Services.MappingInterfaces;
    using EducationPortal.Domain.Core.Common;
    using EducationPortal.Domain.Core.Entities;
    using EducationPortal.Domain.Services.Interfaces;
    using Microsoft.Extensions.Logging;

    public class MaterialService : GenericService<Material>, IMaterialService
    {
        private readonly ICourseMaterialService courseMaterialService;

        public MaterialService(
            IRepository<Material> materialRepository,
            ILogger<MaterialService> logger,
            ICourseMaterialService courseMaterialService)
            : base(materialRepository, logger)
        {
            this.courseMaterialService = courseMaterialService;
        }

        public async Task<IServiceResult> GetMaterialInfosForCourse(int courseId)
        {
            try
            {
                var materialsForCourseServiceResult =
                    await this.courseMaterialService.GetAsync(
                        x => x.CourseId == courseId,
                        x => x.Material);

                if (materialsForCourseServiceResult.IsSuccessful)
                {
                    var materialsForCourse =
                        materialsForCourseServiceResult.GetValue<IList<Material>>();

                    var materialInfos = new List<MaterialInfoDTO>();

                    foreach (var material in materialsForCourse)
                    {
                        materialInfos.Add(this.MapMaterialInfo(material));
                    }

                    return new ServiceResult().SetValue<List<MaterialInfoDTO>>(materialInfos);
                }

                return new ServiceResult().SetException(new Exception("Failed to find materials for course"));
            }
            catch (Exception ex)
            {
                await Task.Run(() => this.logger.LogError(ex.Message));
                return new ServiceResult().SetException(ex);
            }
        }

        private MaterialInfoDTO MapMaterialInfo(Material material)
        {
            return new MaterialInfoDTO()
            {
                Title = material.Title,
                Description = material.Description
            };
        }
    }
}
