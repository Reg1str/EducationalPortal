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

    public class CourseService : GenericService<Course>, ICourseService
    {
        private readonly ISkillService skillService;
        private readonly IUserService userService;
        private readonly IMaterialService materialService;
        private readonly IUserCourseOwnerService userCourseOwnerService;
        private readonly ISkillCourseService skillCourseService;
        private readonly IUserCourseEnrollmentService userCourseEnrollmentService;
        private readonly ICourseMaterialService courseMaterialService;

        public CourseService(
            IRepository<Course> courseRepository,
            ILogger<CourseService> logger,
            ISkillService skillService,
            IUserService userService,
            IMaterialService materialService,
            IUserCourseOwnerService userCourseOwnerService,
            ISkillCourseService skillCourseService,
            IUserCourseEnrollmentService userCourseEnrollmentService,
            ICourseMaterialService courseMaterialService)
            : base(courseRepository, logger)
        {
            this.skillService = skillService;
            this.userService = userService;
            this.materialService = materialService;
            this.userCourseOwnerService = userCourseOwnerService;
            this.skillCourseService = skillCourseService;
            this.userCourseEnrollmentService = userCourseEnrollmentService;
            this.courseMaterialService = courseMaterialService;
        }

        public async Task<ServiceResult<IList<CourseInfoDTO>>> GetCoursesForUser(int userId)
        {
            try
            {
                var courseInfoDtos =
                    await this.repository.GetAsync(
                         x => x.UserCourseEnrollments.Select(y => y.UserId).Contains(userId),
                         x => new CourseInfoDTO()
                         {
                             Id = x.Id,
                             CourseTitle = x.Title,
                             CourseDescription = x.Description
                         });

                return new ServiceResult<IList<CourseInfoDTO>>(courseInfoDtos);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult<IList<CourseInfoDTO>>(ex);
            }
        }

        public async Task<ServiceResult<CourseEditDTO>> GetCourseEditDtoAsync(int courseId)
        {
            try
            {
                var courseEntity =
                    (await this.repository.GetAsync(
                        x => x.Id == courseId)).FirstOrDefault();

                var courseEditDTO = new CourseEditDTO()
                {
                    Id = courseEntity.Id,
                    Title = courseEntity.Title,
                    Description = courseEntity.Description,
                };

                return new ServiceResult<CourseEditDTO>(courseEditDTO);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult<CourseEditDTO>(ex);
            }
        }

        public async Task<ServiceResult<CourseInfoPagedMaterialsDTO>> GetCourseInfoForUserAsync(int courseId, int userId, bool IsAuthorized, PageInfo pageInfo)
        {
            try
            {
                var materialsCountResult =
                    await this.courseMaterialService.CountMaterialsForCourse(courseId);

                var userCourseMaterialsResult =
                    await this.materialService.GetUserCourseMaterialsAsync(courseId, userId);

                var skillInfoResult =
                    await this.skillService.GetSkillInfoDTOs(courseId, userId, IsAuthorized);

                var getCourseResult =
                    await this.GetAsync(x => x.Id == courseId);

                var isEnrolledResult =
                    await this.userService.IsEnrolledAsync(userId, courseId);

                var isOwnerResult =
                    await this.userService.IsOwnerAsync(userId, courseId);

                if (materialsCountResult.IsSuccessful 
                    && userCourseMaterialsResult.IsSuccessful
                    && skillInfoResult.IsSuccessful
                    && getCourseResult.IsSuccessful
                    && isEnrolledResult.IsSuccessful
                    && isOwnerResult.IsSuccessful)
                {
                    var courseInfo = getCourseResult.Value.FirstOrDefault();
                    var materialsCount = materialsCountResult.Value;

                    var numberOfPages = (int)Math.Ceiling(
                        (decimal)materialsCount / pageInfo.PageSize);

                    var courseInfoPagedMaterialsDTO = new CourseInfoPagedMaterialsDTO()
                    {
                        Id = courseId,
                        CourseTitle = courseInfo.Title,
                        CourseDescription = courseInfo.Description,
                        UserCourseMaterialDTOs = userCourseMaterialsResult.Value.ToList(),
                        SkillInfoDTOs = skillInfoResult.Value.ToList(),
                        LastPageNumber = numberOfPages > 0 ? numberOfPages : 1,
                        PageNumber = pageInfo.PageNumber,
                        IsEnrolled = isEnrolledResult.Value,
                        IsOwner = isOwnerResult.Value
                    };

                    return new ServiceResult<CourseInfoPagedMaterialsDTO>(courseInfoPagedMaterialsDTO);
                }

                return new ServiceResult<CourseInfoPagedMaterialsDTO>(new Exception("Failed to get CourseInfo with paged Materials"));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult<CourseInfoPagedMaterialsDTO>(ex);
            }
        }

        public async Task<ServiceResult<CourseInfoPageDTO>> GetCoursePage(PageInfo pageInfo)
        {
            try
            {
                var courseInfoPage =
                   await this.repository.GetPageAsync(
                       x => true,
                       x => new CourseInfoDTO()
                       {
                           Id = x.Id,
                           CourseTitle = x.Title,
                           CourseDescription = x.Description,
                           CourseOwnerEmail = x.UserCourseOwners.FirstOrDefault().User.Email,
                           CourseOwnerId = x.UserCourseOwners.FirstOrDefault().User.Id
                       },
                       pageInfo);

                var numberOfPages = (int)Math.Ceiling(
                    (decimal)await this.repository.Count(x => true) / pageInfo.PageSize);

                var courseInfoPageDTO = new CourseInfoPageDTO()
                {
                    CourseInfoDTOs = courseInfoPage,
                    LastPageNumber = numberOfPages > 0 ? numberOfPages : 1
                };

                return new ServiceResult<CourseInfoPageDTO>(courseInfoPageDTO);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult<CourseInfoPageDTO>(ex);
            }
        }

        public async Task<ServiceResult> CreateCourseAsync(CourseCreateDTO courseCreateDTO)
        {
            try
            {
                var courseEntity = new Course()
                {
                    Title = courseCreateDTO.CourseTitle,
                    Description = courseCreateDTO.CourseDescription
                };

                await this.CreateAsync(courseEntity);
                await this.SaveChangesAsync();

                var addOwnerToCourseDTO = new OwnerCourseDTO()
                {
                    CourseId = courseEntity.Id,
                    OwnerId = courseCreateDTO.UserOwnerId
                };

                var addSkillsToCourseDTO = new AddSkillsToCourseDTO()
                {
                    CourseId = courseEntity.Id,
                    SkillIds = courseCreateDTO.SkillIds
                };

                var addMaterialsToCourseDTO = new AddMaterialsToCourseDTO()
                {
                    CourseId = courseEntity.Id,
                    MaterialIds = courseCreateDTO.CourseMaterialIds
                };

                await this.AddOwnerToCourseAsync(addOwnerToCourseDTO);
                await this.AddSkillsToCourseAsync(addSkillsToCourseDTO);
                await this.AddMaterialsToCourse(addMaterialsToCourseDTO);
                await this.SaveChangesAsync();

                return new ServiceResult();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult(ex);
            }
        }

        public async Task<ServiceResult> AddOwnerToCourseAsync(OwnerCourseDTO ownerCourseDTO)
        {
            try
            {
                var courseCreated =
                    (await this.repository.GetAsync(
                        x => x.Id == ownerCourseDTO.CourseId)).FirstOrDefault();

                var userOwnerServiceResult =
                    await this.userService.GetAsync(
                        x => x.Id == ownerCourseDTO.OwnerId);

                if (userOwnerServiceResult.IsSuccessful)
                {
                    var userOwner = userOwnerServiceResult.Value.FirstOrDefault();

                    var userCourseOwner = new UserCourseOwner()
                    {
                        UserId = userOwner.Id,
                        User = userOwner,
                        CourseId = courseCreated.Id,
                        Course = courseCreated
                    };

                    courseCreated.UserCourseOwners.Add(userCourseOwner);
                    await this.repository.UpdateAsync(courseCreated);
                    await this.SaveChangesAsync();
                }

                return new ServiceResult();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult(ex);
            }
        }

        public async Task<ServiceResult> RemoveOwnerFromCourseAsync(OwnerCourseDTO ownerCourseDTO)
        {
            try
            {
                var courseToUpdate =
                    (await this.repository.GetAsync(
                        x => x.Id == ownerCourseDTO.CourseId)).FirstOrDefault();

                var userCourseOwnerServiceResult =
                     await this.userCourseOwnerService.GetAsync(
                        x => x.CourseId == ownerCourseDTO.CourseId
                        && x.UserId == ownerCourseDTO.OwnerId);

                if (userCourseOwnerServiceResult.IsSuccessful)
                {
                    var userCourseOwner =
                        userCourseOwnerServiceResult.Value.FirstOrDefault();

                    courseToUpdate.UserCourseOwners.Remove(userCourseOwner);
                    await this.repository.UpdateAsync(courseToUpdate);
                    await this.repository.SaveAsync();
                }

                return new ServiceResult();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult(ex);
            }
        }

        public async Task<ServiceResult> AddSkillToCourseAsync(SkillCourseDTO skillCourseDTO)
        {
            try
            {
                var courseToUpdate =
                    (await this.repository.GetAsync(
                        x => x.Id == skillCourseDTO.CourseId)).FirstOrDefault();

                var skillToAddServiceResult =
                     await this.skillService.GetAsync(
                        x => x.Id == skillCourseDTO.SkillId);

                var userIdsToUpdate = 
                    (await this.userCourseEnrollmentService.GetAsync(
                        x => x.CourseId == skillCourseDTO.CourseId,
                        x => x.UserId)).Value.ToList();

                var addSkillToUsersDTO = new SkillUsersDTO()
                {
                    SkillId = skillCourseDTO.SkillId,
                    UserIds = userIdsToUpdate
                };

                await this.userService.AddSkillToUsersAsync(addSkillToUsersDTO);

                if (skillToAddServiceResult.IsSuccessful)
                {
                    var skillToAdd =
                        skillToAddServiceResult.Value.FirstOrDefault();

                    var skillCourseRelation = new SkillCourse()
                    {
                        CourseId = courseToUpdate.Id,
                        Course = courseToUpdate,
                        SkillId = skillToAdd.Id,
                        Skill = skillToAdd
                    };

                    courseToUpdate.SkillCourses.Add(skillCourseRelation);
                    await this.repository.UpdateAsync(courseToUpdate);
                    await this.SaveChangesAsync();
                }

                return new ServiceResult();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult(ex);
            }
        }

        public async Task<ServiceResult> RemoveSkillFromCourseAsync(SkillCourseDTO skillCourseDTO)
        {
            try
            {
                var skillCourseRelationServiceResult =
                     await this.skillCourseService.GetAsync(
                        x => x.CourseId == skillCourseDTO.CourseId
                        && x.SkillId == skillCourseDTO.SkillId);

                var updateUserServiceResult =
                    await this.userService.UpdateUsersOnSkillRemovedAsync(skillCourseDTO.SkillId);

                if (skillCourseRelationServiceResult.IsSuccessful
                    && updateUserServiceResult.IsSuccessful)
                {
                    var skillCourseRelation =
                        skillCourseRelationServiceResult.Value.FirstOrDefault();

                    await this.skillCourseService.DeleteAsync(skillCourseRelation);
                    await this.skillCourseService.SaveChangesAsync();
                }

                return new ServiceResult();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult(ex);
            }
        }

        public async Task<ServiceResult> AddSkillsToCourseAsync(AddSkillsToCourseDTO addSkillsToCourseDTO)
        {
            try
            {
                var courseToUpdate =
                    (await this.repository.GetAsync(
                        x => x.Id == addSkillsToCourseDTO.CourseId)).FirstOrDefault();

                var skillsToAddServiceResult =
                    await this.skillService.GetAsync(
                        x => addSkillsToCourseDTO.SkillIds.Contains(x.Id));

                var userUpdateServiceResult =
                    await this.userService.UpdateUsersOnSkillsAddedAsync(addSkillsToCourseDTO);

                if (skillsToAddServiceResult.IsSuccessful && userUpdateServiceResult.IsSuccessful)
                {
                    var skillsToAdd =
                        skillsToAddServiceResult.Value.ToList();

                    foreach (var skillToAdd in skillsToAdd)
                    {
                        var skillCourseRelation = new SkillCourse()
                        {
                            CourseId = courseToUpdate.Id,
                            Course = courseToUpdate,
                            SkillId = skillToAdd.Id,
                            Skill = skillToAdd
                        };
                        courseToUpdate.SkillCourses.Add(skillCourseRelation);
                    }

                    await this.repository.UpdateAsync(courseToUpdate);
                    await this.SaveChangesAsync();
                }

                return new ServiceResult();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult(ex);
            }
        }

        public async Task<ServiceResult> AddUserEnrollmentToCourse(UserEnrollmentDTO userEnrollmentDTO)
        {
            try
            {
                var courseToUpdate =
                    (await this.repository.GetAsync(
                        x => x.Id == userEnrollmentDTO.CourseId)).FirstOrDefault();

                var userToAddServiceResult =
                     await this.userService.GetAsync(
                        x => x.Id == userEnrollmentDTO.UserId);

                var addSkillsToUserDTO = new AddSkillsToUserDTO()
                {
                    UserId = userEnrollmentDTO.UserId,
                    SkillIds =
                        (await this.skillCourseService.GetAsync(
                            x => x.CourseId == userEnrollmentDTO.CourseId,
                            x => x.SkillId)).Value.ToList()
                };

                var addMaterialsToUserDTO = new AddMaterialsToUserDTO()
                {
                    UserId = userEnrollmentDTO.UserId,
                    MaterialIds =
                        (await this.courseMaterialService.GetAsync(
                            x => x.CourseId == userEnrollmentDTO.CourseId,
                            x => x.MaterialId)).Value.ToList()
                };

                await this.userService.AddMaterialsToUserAsync(addMaterialsToUserDTO);
                await this.userService.AddSkillsToUserAsync(addSkillsToUserDTO);

                if (userToAddServiceResult.IsSuccessful)
                {
                    var userToAdd =
                        userToAddServiceResult.Value.FirstOrDefault();

                    var userCourseEnrollment = new UserCourseEnrollment()
                    {
                        CourseId = courseToUpdate.Id,
                        UserId = userToAdd.Id
                    };

                    var enrollmentExists =
                        (await this.userCourseEnrollmentService.ExistsAsync(
                            x => x.CourseId == userCourseEnrollment.CourseId
                            && x.UserId == userCourseEnrollment.UserId)).Value;

                    if (!enrollmentExists)
                    {
                        courseToUpdate.UserCourseEnrollments.Add(userCourseEnrollment);
                    }

                    await this.repository.UpdateAsync(courseToUpdate);
                    await this.repository.SaveAsync();
                }

                return new ServiceResult();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult(ex);
            }
        }

        public async Task<ServiceResult> RemoveUserEnrollmentFromCourse(UserEnrollmentDTO userEnrollmentDTO)
        {
            try
            {
                var courseToUpdate =
                    (await this.repository.GetAsync(
                        x => x.Id == userEnrollmentDTO.CourseId)).FirstOrDefault();

                var userCourseEnrollmentServiceResult =
                     await this.userCourseEnrollmentService.GetAsync(
                        x => x.CourseId == userEnrollmentDTO.CourseId
                        && x.UserId == userEnrollmentDTO.UserId);

                if (userCourseEnrollmentServiceResult.IsSuccessful)
                {
                    var userCourseEnrollment =
                        userCourseEnrollmentServiceResult.Value.FirstOrDefault();

                    await this.userCourseEnrollmentService.DeleteAsync(userCourseEnrollment);
                    await this.userCourseEnrollmentService.SaveChangesAsync();
                }

                return new ServiceResult();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult(ex);
            }
        }

        public async Task<ServiceResult> AddMaterialsToCourse(AddMaterialsToCourseDTO addMaterialToCourseDTO)
        {
            try
            {
                var courseToUpdate =
                    (await this.repository.GetAsync(
                        x => x.Id == addMaterialToCourseDTO.CourseId)).FirstOrDefault();

                var materialsToAddServiceResult =
                    await this.materialService.GetAsync(
                        x => addMaterialToCourseDTO.MaterialIds.Contains(x.Id));

                var usersToUpdate =
                    (await this.userCourseEnrollmentService.GetAsync(x => x.CourseId == addMaterialToCourseDTO.CourseId)).Value;

                if (addMaterialToCourseDTO.MaterialIds.Any() && usersToUpdate.Any())
                {
                    var addMaterialToUsersDTO = new AddMaterialToUsersDTO()
                    {
                        MaterialId = addMaterialToCourseDTO.MaterialIds.FirstOrDefault(),
                        UserIds = usersToUpdate.Select(x => x.UserId).ToList()
                    };

                    var usersUpdateResult =
                        await this.userService.AddMaterialToUsersAsync(addMaterialToUsersDTO);
                }

                if (materialsToAddServiceResult.IsSuccessful)
                {
                    var materialsToAdd =
                        materialsToAddServiceResult.Value;

                    foreach (var materialToAdd in materialsToAdd)
                    {
                        var courseMaterialRelation = new CourseMaterial()
                        {
                            CourseId = courseToUpdate.Id,
                            Course = courseToUpdate,
                            MaterialId = materialToAdd.Id,
                            Material = materialToAdd
                        };

                        courseToUpdate.CourseMaterials.Add(courseMaterialRelation);
                    }

                    await this.repository.UpdateAsync(courseToUpdate);
                    await this.SaveChangesAsync();
                }

                return new ServiceResult();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult(ex);
            }
        }

        public async Task<ServiceResult> RemoveMaterialFromCourse(RemoveMaterialFromCourseDTO removeMaterialFromCourseDTO)
        {
            try
            {
                var courseToUpdate =
                    (await this.repository.GetAsync(
                        x => x.Id == removeMaterialFromCourseDTO.CourseId)).FirstOrDefault();

                var courseMaterialRelationServiceResult =
                     await this.courseMaterialService.GetAsync(
                        x => x.CourseId == removeMaterialFromCourseDTO.CourseId
                        && x.MaterialId == removeMaterialFromCourseDTO.MaterialId);

                if (courseMaterialRelationServiceResult.IsSuccessful)
                {
                    var courseMaterialRelation =
                        courseMaterialRelationServiceResult.Value.FirstOrDefault();

                    await this.courseMaterialService.DeleteAsync(courseMaterialRelation);
                    await this.courseMaterialService.SaveChangesAsync();
                }

                return new ServiceResult();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult(ex);
            }
        }

        public async Task<ServiceResult> EditCourseInfo(CourseEditDTO courseEditDTO)
        {
            try
            {
                var courseEntity = new Course()
                {
                    Id = courseEditDTO.Id,
                    Title = courseEditDTO.Title,
                    Description = courseEditDTO.Description
                };

                await this.repository.UpdateAsync(courseEntity);
                await this.repository.SaveAsync();
                return new ServiceResult();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult(ex);
            }
        }

        public async Task<ServiceResult> UpdateCourseProgressAsync(int materialId, int userId)
        {
            try
            {
                return new ServiceResult();
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult(ex);
            }
        }

        public async Task<ServiceResult> DeleteCourseAsync(int courseId)
        {
            try
            {
                var courseEntity = new Course()
                {
                    Id = courseId
                };

                await this.repository.DeleteAsync(courseEntity);
                await this.repository.SaveAsync();

                return new ServiceResult();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult(ex);
            }
        }
    }
}
