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
        private readonly IUserCourseEnrollmentService userCourseEnrollmentService;
        private readonly IUserCourseOwnerService userCourseOwnerService;
        private readonly IUserMaterialsService userMaterialsService;

        public UserService(
            IRepository<User> userRepository,
            ILogger<UserService> logger,
            IUserSkillsService userSkillService,
            ISkillService skillService,
            IUserCourseEnrollmentService userCourseEnrollmentService,
            IUserCourseOwnerService userCourseOwnerService,
            IUserMaterialsService userMaterialsService,
            IMaterialService materialService)
            : base(userRepository, logger)
        {
            this.userSkillService = userSkillService;
            this.skillService = skillService;
            this.materialService = materialService;
            this.userCourseEnrollmentService = userCourseEnrollmentService;
            this.userCourseOwnerService = userCourseOwnerService;
            this.userMaterialsService = userMaterialsService;
        }


        public async Task<ServiceResult<bool>> IsOwnerAsync(int userId, int courseId)
        {
            try
            {
                var userOwnerResult =
                    await this.userCourseOwnerService.GetAsync(
                        x => x.CourseId == courseId && x.UserId == userId);

                if (userOwnerResult.IsSuccessful)
                {
                    var userOwner = userOwnerResult.Value.FirstOrDefault();
                    return new ServiceResult<bool>(!(userOwner == null));
                }

                return new ServiceResult<bool>(new Exception("Failed to get IsOwner"));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult<bool>(ex);
            }
        }

        public async Task<ServiceResult<bool>> IsEnrolledAsync(int userId, int courseId)
        {
            try
            {
                var userEnrolledResult =
                    await this.userCourseEnrollmentService.GetAsync(
                        x => x.CourseId == courseId && x.UserId == userId);

                if (userEnrolledResult.IsSuccessful)
                {
                    var userEnrolled = userEnrolledResult.Value.FirstOrDefault();
                    return new ServiceResult<bool>(!(userEnrolled == null));
                }

                return new ServiceResult<bool>(new Exception("Failed to get IsOwner"));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult<bool>(ex);
            }
        }

        public async Task<ServiceResult<UserInfoDTO>> GetUserInfoAsync(int userId)
        {
            try
            {
                var courseInfo =
                    (await this.repository.GetAsync(
                        x => x.Id == userId,
                        x => new UserInfoDTO()
                        {
                            FirstName = x.Firstname,
                            LastName = x.Lastname,
                            Email = x.Email
                        })).FirstOrDefault();

                return new ServiceResult<UserInfoDTO>(courseInfo);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult<UserInfoDTO>(ex);
            }
        }

        public async Task<ServiceResult> UpdateUsersOnSkillsAddedAsync(AddSkillsToCourseDTO addSkillsToCourseDTO)
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
                        userIdsToUpdateServiceResult.Value;

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

                return new ServiceResult();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult(ex);
            }
        }

        public async Task<ServiceResult<IList<CourseInfoDTO>>> GetCoursesInfoForUserAsync(UserInfoDTO userInfoDTO)
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
                        coursesForUserServiceResult.Value.FirstOrDefault();

                    var courseInfoDTOs = new List<CourseInfoDTO>();

                    foreach (var course in coursesForUser)
                    {
                        courseInfoDTOs.Add(await this.CreateCourseInfoDTO(course, userInfoDTO));
                    }

                    return new ServiceResult<IList<CourseInfoDTO>>(courseInfoDTOs);
                }

                return new ServiceResult<IList<CourseInfoDTO>>(new Exception("Failed to find courses for user"));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult<IList<CourseInfoDTO>>(ex);
            }
        }

        public async Task<ServiceResult> UpdateUsersOnSkillRemovedAsync(int skillId)
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
                        userIdsToUpdateServiceResult.Value.ToList();

                    var removeSkillFromUsersDTO = new SkillUsersDTO()
                    {
                        SkillId = skillId,
                        UserIds = userIdsToUpdate
                    };

                    await this.RemoveSkillFromUsersAsync(removeSkillFromUsersDTO);
                }

                return new ServiceResult();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult(ex);
            }
        }

        public async Task<ServiceResult> AddSkillsToUserAsync(AddSkillsToUserDTO addSkillsToUserDTO)
        {
            if (addSkillsToUserDTO != null && addSkillsToUserDTO.SkillIds.Any())
            {
                var userToUpdate =
                       (await this.repository.GetAsync(
                           x => x.Id == addSkillsToUserDTO.UserId)).FirstOrDefault();

                var userSkills =
                    (await this.userSkillService.GetAsync(
                        x => x.UserId == addSkillsToUserDTO.UserId,
                        x => x.SkillId)).Value;

                var skillsToAddServiceResult =
                    await this.skillService.GetAsync(
                        x => addSkillsToUserDTO.SkillIds.Contains(x.Id));

                if (userToUpdate != null && skillsToAddServiceResult.IsSuccessful)
                {
                    var skillsToAdd =
                        skillsToAddServiceResult.Value;

                    foreach (var skill in skillsToAdd)
                    {
                        if (!userSkills.Contains(skill.Id))
                        {
                            var userSkillRelation = new UserSkills()
                            {
                                SkillId = skill.Id,
                                Skill = skill,
                                UserId = userToUpdate.Id,
                                User = userToUpdate
                            };
                        
                            userToUpdate.UserSkills.Add(userSkillRelation);
                        }
                    }

                    await this.repository.UpdateAsync(userToUpdate);
                    await this.SaveChangesAsync();
                }

                return new ServiceResult();
            }

            return new ServiceResult(new ArgumentNullException("AddSkillsToUserDTO was null"));
        }

        public async Task<ServiceResult> AddSkillToUsersAsync(SkillUsersDTO skillUsersDTO)
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
                        skillToAddServiceResult.Value.FirstOrDefault();

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

                    await this.SaveChangesAsync();
                }

                return new ServiceResult();
            }

            return new ServiceResult(new ArgumentNullException("AddSkillToUsersDTO was null"));
        }

        public async Task<ServiceResult> RemoveSkillFromUsersAsync(SkillUsersDTO skillUsersDTO)
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
                        userSkillRelationsToRemoveServiceResult.Value;

                    foreach (var userSkillRelation in userSkillRelationsToRemove)
                    {
                        await this.userSkillService.DeleteAsync(userSkillRelation);
                    }

                    await this.SaveChangesAsync();
                }

                return new ServiceResult();
            }

            return new ServiceResult(new ArgumentNullException("RemoveSkillFromUsersDTO was null"));
        }

        public async Task<ServiceResult<UserInfoDTO>> GetUserBySessionAsync(UserSession userSession)
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

                return new ServiceResult<UserInfoDTO>(userInfoDTO);
            }

            return new ServiceResult<UserInfoDTO>(new ArgumentNullException("UserSession was null"));
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
                CourseOwnerEmail = userInfoDTO.Email
            };
        }

        public async Task<ServiceResult> RegisterUserAsync(UserRegisterDTO userRegisterDTO)
        {
            try
            {
                var userExists =
                    await this.repository.ExistsAsync(x => x.Email == userRegisterDTO.Email);
                
                if (!userExists)
                {
                    var userEntity = new User()
                    {
                        Email = userRegisterDTO.Email,
                        Password = userRegisterDTO.Password,
                        Firstname = userRegisterDTO.FirstName,
                        Lastname = userRegisterDTO.LastName,
                        UserType = "Default"
                    };

                    await this.repository.CreateAsync(userEntity);
                    await this.repository.SaveAsync();
                }
                else
                {
                    return new ServiceResult(new Exception("User with this email already exists"));
                }

                return new ServiceResult();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult(ex);
            }
        }

        public async Task<ServiceResult> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO)
        {
            try
            {
                return new ServiceResult();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult(ex);
            }
        }

        public async Task<ServiceResult> AddMaterialsToUserAsync(AddMaterialsToUserDTO addMaterialsToUserDTO)
        {
            if (addMaterialsToUserDTO != null && addMaterialsToUserDTO.MaterialIds.Any())
            {
                var userToUpdate =
                       (await this.repository.GetAsync(
                           x => x.Id == addMaterialsToUserDTO.UserId)).FirstOrDefault();

                var userMaterials =
                    (await this.userMaterialsService.GetAsync(
                        x => x.UserId == addMaterialsToUserDTO.UserId,
                        x => x.MaterialId)).Value;

                var materialsToAddResult =
                    await this.materialService.GetAsync(
                        x => addMaterialsToUserDTO.MaterialIds.Contains(x.Id));

                if (userToUpdate != null && materialsToAddResult.IsSuccessful)
                {
                    var materialsToAdd =
                        materialsToAddResult.Value;

                    foreach (var material in materialsToAdd)
                    {
                        if (!userMaterials.Contains(material.Id))
                        {
                            var userMaterialsRelation = new UserMaterials()
                            {
                                MaterialId = material.Id,
                                Material = material,
                                UserId = userToUpdate.Id,
                                User = userToUpdate
                            };

                            userToUpdate.UserMaterials.Add(userMaterialsRelation);
                        }
                    }

                    await this.repository.UpdateAsync(userToUpdate);
                    await this.SaveChangesAsync();
                }

                return new ServiceResult();
            }

            return new ServiceResult(new ArgumentNullException("AddSkillsToUserDTO was null"));
        }

        public async Task<ServiceResult> AddMaterialToUsersAsync(AddMaterialToUsersDTO addMaterialToUsersDTO)
        {
            try
            {
                var usersToUpdate =
                    await this.repository.GetAsync(user => addMaterialToUsersDTO.UserIds.Contains(user.Id));

                var materialToAddResult =
                     await this.materialService.GetAsync(x => x.Id == addMaterialToUsersDTO.MaterialId);

                if (materialToAddResult.IsSuccessful)
                {
                    var materialToAdd =
                        materialToAddResult.Value.FirstOrDefault();

                    foreach (var user in usersToUpdate)
                    {
                        var userMaterialRelation = new UserMaterials()
                        {
                            MaterialId = materialToAdd.Id,
                            Material = materialToAdd,
                            UserId = user.Id,
                            User = user
                        };

                        user.UserMaterials.Add(userMaterialRelation);
                    }

                    await this.SaveChangesAsync();
                }


                return new ServiceResult();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult();
            }
        }
    }
}
