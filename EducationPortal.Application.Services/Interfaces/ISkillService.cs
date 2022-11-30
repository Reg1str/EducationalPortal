namespace EducationPortal.Application.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using EducationPortal.Application.Services.DTO;
    using EducationPortal.Domain.Core.Common;
    using EducationPortal.Domain.Core.Entities;

    public interface ISkillService : IGenericService<Skill>
    {
        Task<ServiceResult> CreateSkillAsync(SkillCreateDTO skillCreateDTO);

        Task<ServiceResult> UpdateSkillProgressAsync(int materialId, int userId);

        Task<ServiceResult<SkillPageDTO>> GetSkillsAvailableForCourseAsync(int courseId, PageInfo pageInfo);

        Task<ServiceResult<IList<SkillInfoDTO>>> GetSkillInfoDTOs(int courseId, int userId, bool IsAuthorized);

        Task<ServiceResult<SkillPageDTO>> GetSkillPage(PageInfo pageInfo);

        Task<ServiceResult<IList<SkillInfoDTO>>> GetSkillsForUserAsync(int userId);

        Task<ServiceResult<IList<string>>> GetSkillNamesForCourse(int courseId);
    }
}
