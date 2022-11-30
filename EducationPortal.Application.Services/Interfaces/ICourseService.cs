namespace EducationPortal.Application.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EducationPortal.Application.Services.DTO;
    using EducationPortal.Domain.Core.Common;

    public interface ICourseService
    {
        Task<ServiceResult> DeleteCourseAsync(int courseId);

        Task<ServiceResult<IList<CourseInfoDTO>>> GetCoursesForUser(int userId);

        Task<ServiceResult> UpdateCourseProgressAsync(int materialId, int userId);

        Task<ServiceResult> EditCourseInfo(CourseEditDTO courseEditDTO);

        Task<ServiceResult<CourseEditDTO>> GetCourseEditDtoAsync(int courseId);

        Task<ServiceResult<CourseInfoPagedMaterialsDTO>> GetCourseInfoForUserAsync(int courseId, int userId, bool IsAuthorized, PageInfo pageInfo);

        Task<ServiceResult<CourseInfoPageDTO>> GetCoursePage(PageInfo pageInfo);

        Task<ServiceResult> CreateCourseAsync(CourseCreateDTO courseCreateDTO);

        Task<ServiceResult> AddOwnerToCourseAsync(OwnerCourseDTO ownerCourseDTO);

        Task<ServiceResult> RemoveOwnerFromCourseAsync(OwnerCourseDTO ownerCourseDTO);

        Task<ServiceResult> AddSkillToCourseAsync(SkillCourseDTO skillCourseDTO);

        Task<ServiceResult> RemoveSkillFromCourseAsync(SkillCourseDTO skillCourseDTO);

        Task<ServiceResult> AddSkillsToCourseAsync(AddSkillsToCourseDTO addSkillsToCourseDTO);

        Task<ServiceResult> AddUserEnrollmentToCourse(UserEnrollmentDTO userEnrollmentDTO);

        Task<ServiceResult> RemoveUserEnrollmentFromCourse(UserEnrollmentDTO userEnrollmentDTO);

        Task<ServiceResult> AddMaterialsToCourse(AddMaterialsToCourseDTO addMaterialToCourseDTO);

        Task<ServiceResult> RemoveMaterialFromCourse(RemoveMaterialFromCourseDTO removeMaterialFromCourseDTO);
    }
}
