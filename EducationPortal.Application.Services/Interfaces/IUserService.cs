namespace EducationPortal.Application.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EducationPortal.Application.Services.DTO;
    using EducationPortal.Domain.Core.Common;
    using EducationPortal.Domain.Core.Entities;

    public interface IUserService : IGenericService<User>
    {
        Task<ServiceResult<UserInfoDTO>> GetUserInfoAsync(int userId);

        Task<ServiceResult<bool>> IsOwnerAsync(int userId, int courseId);

        Task<ServiceResult<bool>> IsEnrolledAsync(int userId, int courseId);

        Task<ServiceResult> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO);

        Task<ServiceResult> RegisterUserAsync(UserRegisterDTO userRegisterDTO);

        Task<ServiceResult<IList<CourseInfoDTO>>> GetCoursesInfoForUserAsync(UserInfoDTO userInfoDTO);

        Task<ServiceResult> UpdateUsersOnSkillsAddedAsync(AddSkillsToCourseDTO addSkillsToCourseDTO);

        Task<ServiceResult> UpdateUsersOnSkillRemovedAsync(int skillId);

        Task<ServiceResult> AddSkillsToUserAsync(AddSkillsToUserDTO addSkillsToUserDTO);

        Task<ServiceResult> AddSkillToUsersAsync(SkillUsersDTO skillUsersDTO);

        Task<ServiceResult> AddMaterialsToUserAsync(AddMaterialsToUserDTO addMaterialsToUserDTO);

        Task<ServiceResult> RemoveSkillFromUsersAsync(SkillUsersDTO skillUsersDTO);

        Task<ServiceResult<UserInfoDTO>> GetUserBySessionAsync(UserSession userSession);

        Task<ServiceResult> AddMaterialToUsersAsync(AddMaterialToUsersDTO addMaterialToUsersDTO);
    }
}
