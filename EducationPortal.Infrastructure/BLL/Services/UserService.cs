namespace EducationPortal.Infrastructure.BLL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using EducationPortal.Application.Services.DTO;
    using EducationPortal.Application.Services.Interfaces;
    using EducationPortal.Application.Services.MappingInterfaces;
    using EducationPortal.Domain.Core.Common;
    using EducationPortal.Domain.Core.Entities;
    using EducationPortal.Domain.Core.Mappings;
    using EducationPortal.Domain.Services.Interfaces;
    using Microsoft.Extensions.Logging;

    public class UserService : GenericService<User>, IUserService
    {
        private readonly IUserSkillsService userSkillService;
        private readonly ISkillService skillService;
        private readonly IMaterialService materialService;

        public UserService(
            IRepository<User> userRepository,
            ILogger<UserService> logger,
            IUserSkillsService userSkillService,
            ISkillService skillService,
            IMaterialService materialService)
            : base(userRepository, logger)
        {
            this.userSkillService = userSkillService;
            this.skillService = skillService;
            this.materialService = materialService;
        }

        public async Task<IServiceResult> UpdateUsersOnSkillsAddedAsync(AddSkillsToCourseDTO addSkillsToCourseDTO)
        {
            try
            {
                var userIdsToUpdateServiceResult =
                    await this.GetAsync(
                        x => x.UserCourseEnrollments.Select(
                            enrollment => enrollment.CourseId).Contains(addSkillsToCourseDTO.CourseId),
                        x => x.Id);

                if (userIdsToUpdateServiceResult.IsSuccessful)
                {
                    var userIdsToUpdate =
                        userIdsToUpdateServiceResult.GetValue<IList<int>>().ToList();

                    foreach (var userIdToUpdate in userIdsToUpdate)
                    {
                        var addSkillsToUserDTO = new AddSkillsToUserDTO()
                        {
                            UserId = userIdToUpdate,
                            SkillIds = addSkillsToCourseDTO.SkillIds
                        };

                        await this.AddSkillsToUserAsync(addSkillsToUserDTO);
                    }
                }

                return new ServiceResult().SetSuccessful();
            }
            catch (Exception ex)
            {
                await Task.Run(() => this.logger.LogError(ex.Message));
                return new ServiceResult().SetException(ex);
            }
        }

        public async Task<IServiceResult> GetCoursesInfoForUserAsync(UserInfoDTO userInfoDTO)
        {
            try
            {
                var coursesForUserServiceResult =
                    await this.GetAsync(
                        x => x.Id == userInfoDTO.Id,
                        x => x.UserCourseEnrollments.Select(x => x.Course));

                if (coursesForUserServiceResult.IsSuccessful)
                {
                    var coursesForUser =
                        coursesForUserServiceResult.GetValue<IList<Course>>();

                    var courseInfoDTOs = new List<CourseInfoDTO>();

                    foreach (var course in coursesForUser)
                    {
                        courseInfoDTOs.Add(await this.CreateCourseInfoDTO(course, userInfoDTO));
                    }

                    return new ServiceResult().SetValue<List<CourseInfoDTO>>(courseInfoDTOs);
                }

                return new ServiceResult().SetException(new Exception("Failed to find courses for user"));
            }
            catch (Exception ex)
            {
                await Task.Run(() => this.logger.LogError(ex.Message));
                return new ServiceResult().SetException(ex);
            }
        }

        public async Task<IServiceResult> UpdateUsersOnSkillRemovedAsync(int skillId)
        {
            try
            {
                var userIdsToUpdateServiceResult =
                     await this.GetAsync(
                        x => x.UserCourseEnrollments.SelectMany(
                            enrollment => enrollment.Course.SkillCourses.Select(y => y.SkillId))
                                                    .Contains(skillId),
                        x => x.Id);

                if (userIdsToUpdateServiceResult.IsSuccessful)
                {
                    var userIdsToUpdate =
                        userIdsToUpdateServiceResult.GetValue<IList<int>>().ToList();

                    var removeSkillFromUsersDTO = new SkillUsersDTO()
                    {
                        SkillId = skillId,
                        UserIds = userIdsToUpdate
                    };

                    await this.RemoveSkillFromUsersAsync(removeSkillFromUsersDTO);
                }

                return new ServiceResult().SetSuccessful();
            }
            catch (Exception ex)
            {
                await Task.Run(() => this.logger.LogError(ex.Message));
                return new ServiceResult().SetException(ex);
            }
        }

        public async Task<IServiceResult> AddSkillsToUserAsync(AddSkillsToUserDTO addSkillsToUserDTO)
        {
            if (addSkillsToUserDTO != null && addSkillsToUserDTO.SkillIds.Any())
            {
                var userToUpdate =
                       (await this.repository.GetAsync(
                           x => x.Id == addSkillsToUserDTO.UserId)).FirstOrDefault();

                var skillsToAddServiceResult =
                    await this.skillService.GetAsync(
                        x => addSkillsToUserDTO.SkillIds.Contains(x.Id));

                if (userToUpdate != null && skillsToAddServiceResult.IsSuccessful)
                {
                    var skillsToAdd =
                        skillsToAddServiceResult.GetValue<IList<Skill>>();

                    foreach (var skill in skillsToAdd)
                    {
                        var userSkillRelation = new UserSkills()
                        {
                            SkillId = skill.Id,
                            Skill = skill,
                            UserId = userToUpdate.Id,
                            User = userToUpdate
                        };

                        userToUpdate.UserSkills.Add(userSkillRelation);
                        await this.repository.UpdateAsync(userToUpdate);
                    }
                }

                return new ServiceResult().SetSuccessful();
            }

            return new ServiceResult().SetException(
                new ArgumentNullException("AddSkillsToUserDTO was null"));
        }

        public async Task<IServiceResult> AddSkillToUsersAsync(SkillUsersDTO skillUsersDTO)
        {
            if (skillUsersDTO != null && skillUsersDTO.UserIds.Any())
            {
                var usersToUpdate =
                    await this.repository.GetAsync(user => skillUsersDTO.UserIds.Contains(user.Id));

                var skillToAddServiceResult =
                     await this.skillService.GetAsync(x => x.Id == skillUsersDTO.SkillId);

                if (skillToAddServiceResult.IsSuccessful)
                {
                    var skillToAdd =
                        skillToAddServiceResult.GetValue<IList<Skill>>().FirstOrDefault();

                    foreach (var user in usersToUpdate)
                    {
                        var userSkillRelation = new UserSkills()
                        {
                            SkillId = skillToAdd.Id,
                            Skill = skillToAdd,
                            UserId = user.Id,
                            User = user
                        };

                        user.UserSkills.Add(userSkillRelation);
                    }

                    await this.repository.SaveAsync();
                }

                return new ServiceResult().SetSuccessful();
            }

            return new ServiceResult().SetException(
                new ArgumentNullException("AddSkillToUsersDTO was null"));
        }

        public async Task<IServiceResult> RemoveSkillFromUsersAsync(SkillUsersDTO skillUsersDTO)
        {
            if (skillUsersDTO != null)
            {
                var userSkillRelationsToRemoveServiceResult =
                    await this.userSkillService.GetAsync(
                        x => skillUsersDTO.UserIds.Contains(x.UserId)
                        && x.SkillId == skillUsersDTO.SkillId);

                if (userSkillRelationsToRemoveServiceResult.IsSuccessful)
                {
                    var userSkillRelationsToRemove =
                        userSkillRelationsToRemoveServiceResult.GetValue<IList<UserSkills>>();

                    foreach (var userSkillRelation in userSkillRelationsToRemove)
                    {
                        await this.userSkillService.DeleteAsync(userSkillRelation);
                    }
                }

                return new ServiceResult().SetSuccessful();
            }

            return new ServiceResult().SetException(
                new ArgumentNullException("RemoveSkillFromUsersDTO was null"));
        }

        public async Task<IServiceResult> GetUserBySessionAsync(UserSession userSession)
        {
            if (userSession != null)
            {
                var userEntity =
                    (await this.repository.GetAsync(user => user.Email == userSession.Email
                        && user.Password == userSession.Password)).FirstOrDefault();

                var userInfoDTO = new UserInfoDTO()
                {
                    Id = userEntity.Id,
                    FirstName = userEntity.Firstname,
                    LastName = userEntity.Lastname,
                    Email = userEntity.Email,
                    UserType = userEntity.UserType
                };

                return new ServiceResult().SetValue<UserInfoDTO>(userInfoDTO);
            }

            return new ServiceResult().SetException(
                new ArgumentNullException("UserSession was null"));
        }

        private async Task<CourseInfoDTO> CreateCourseInfoDTO(Course course, UserInfoDTO userInfoDTO)
        {
            var materialsInfoServiceResult =
                            await this.materialService.GetMaterialInfosForCourse(course.Id);

            var skillNamesServiceResult =
                await this.skillService.GetSkillNamesForCourse(course.Id);

            return new CourseInfoDTO()
            {
                CourseTitle = course.Title,
                CourseDescription = course.Description,
                CourseOwnerEmail = userInfoDTO.Email,
                CourseOwnerFullName = $"{userInfoDTO.FirstName} {userInfoDTO.LastName}",
                MaterialInfos = materialsInfoServiceResult.IsSuccessful
                    ? materialsInfoServiceResult.GetValue<List<MaterialInfoDTO>>()
                    : new List<MaterialInfoDTO>(),
                SkillNames = skillNamesServiceResult.IsSuccessful
                    ? skillNamesServiceResult.GetValue<List<string>>()
                    : new List<string>()
            };
        }
    }
}
