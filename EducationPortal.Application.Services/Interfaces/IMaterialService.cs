using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EducationPortal.Application.Services.DTO;
using EducationPortal.Domain.Core.Common;
using EducationPortal.Domain.Core.Entities;

namespace EducationPortal.Application.Services.Interfaces
{
    public interface IMaterialService : IGenericService<Material>
    {
        Task<ServiceResult> FinishMaterial(int materialId, int userId);

        Task<ServiceResult> CreateMaterialAsync(MaterialInfoDTO materialInfoDTO);

        Task<ServiceResult<MaterialInfoDTO>> GetMaterialInfoAsync(int materialId, int userId);

        Task<ServiceResult<MaterialPageDTO>> GetAvailableMaterialsAsync(int courseId, PageInfo pageInfo);

        Task<ServiceResult<MaterialPageDTO>> GetMaterialPageAsync(PageInfo pageInfo);

        Task<ServiceResult<IList<UserCourseMaterialDTO>>> GetUserCourseMaterialsAsync(int courseId, int userId);

        Task<ServiceResult<IList<MaterialInfoDTO>>> GetMaterialInfosForCourse(int courseId);
    }
}
