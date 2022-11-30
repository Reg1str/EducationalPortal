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

        public async Task<IServiceResult> CreateCourseAsync(CourseCreateDTO courseCreateDTO)
        {
            try
            {
                var courseEntity = new Course()
                {
                    Title = courseCreateDTO.CourseTitle,
                    Description = courseCreateDTO.CourseDescription
                };

                await this.CreateAsync(courseEntity);

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

                return new ServiceResult().SetSuccessful();
            }
            catch (Exception ex)
            {
                await Task.Run(() => this.logger.LogError(ex.Message));
                return new ServiceResult().SetException(ex);
            }
        }

        public async Task<IServiceResult> AddOwnerToCourseAsync(OwnerCourseDTO ownerCourseDTO)
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
                    var userOwner = userOwnerServiceResult.GetValue<IList<User>>().FirstOrDefault();

                    var userCourseOwner = new UserCourseOwner()
                    {
                        UserId = userOwner.Id,
                        User = userOwner,
                        CourseId = courseCreated.Id,
                        Course = courseCreated
                    };

                    courseCreated.UserCourseOwners.Add(userCourseOwner);
                    await this.repository.UpdateAsync(courseCreated);
                }

                return new ServiceResult().SetSuccessful();
            }
            catch (Exception ex)
            {
                await Task.Run(() => this.logger.LogError(ex.Message));
                return new ServiceResult().SetException(ex);
            }
        }

        public async Task<IServiceResult> RemoveOwnerFromCourseAsync(OwnerCourseDTO ownerCourseDTO)
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
                        userCourseOwnerServiceResult.GetValue<IList<UserCourseOwner>>().FirstOrDefault();

                    courseToUpdate.UserCourseOwners.Remove(userCourseOwner);
                    await this.repository.UpdateAsync(courseToUpdate);
                }

                return new ServiceResult().SetSuccessful();
            }
            catch (Exception ex)
            {
                await Task.Run(() => this.logger.LogError(ex.Message));
                return new ServiceResult().SetException(ex);
            }
        }

        public async Task<IServiceResult> AddSkillToCourseAsync(SkillCourseDTO skillCourseDTO)
        {
            try
            {
                var courseToUpdate =
                    (await this.repository.GetAsync(
                        x => x.Id == skillCourseDTO.CourseId)).FirstOrDefault();

                var skillToAddServiceResult =
                     await this.skillService.GetAsync(
                        x => x.Id == skillCourseDTO.SkillId);

                var userIdsToUpdate = courseToUpdate.UserCourseEnrollments.Select(x => x.UserId).ToList();

                var addSkillToUsersDTO = new SkillUsersDTO()
                {
                    SkillId = skillCourseDTO.SkillId,
                    UserIds = userIdsToUpdate
                };

                await this.userService.AddSkillToUsersAsync(addSkillToUsersDTO);

                if (skillToAddServiceResult.IsSuccessful)
                {
                    var skillToAdd =
                        skillToAddServiceResult.GetValue<IList<Skill>>().FirstOrDefault();

                    var skillCourseRelation = new SkillCourse()
                    {
                        CourseId = courseToUpdate.Id,
                        Course = courseToUpdate,
                        SkillId = skillToAdd.Id,
                        Skill = skillToAdd
                    };

                    courseToUpdate.SkillCourses.Add(skillCourseRelation);
                    await this.repository.UpdateAsync(courseToUpdate);
                }

                return new ServiceResult().SetSuccessful();
            }
            catch (Exception ex)
            {
                await Task.Run(() => this.logger.LogError(ex.Message));
                return new ServiceResult().SetException(ex);
            }
        }

        public async Task<IServiceResult> RemoveSkillFromCourseAsync(SkillCourseDTO skillCourseDTO)
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
                        skillCourseRelationServiceResult.GetValue<IList<SkillCourse>>().FirstOrDefault();

                    await this.skillCourseService.DeleteAsync(skillCourseRelation);
                }

                return new ServiceResult().SetSuccessful();
            }
            catch (Exception ex)
            {
                await Task.Run(() => this.logger.LogError(ex.Message));
                throw;
            }
        }

        public async Task<IServiceResult> AddSkillsToCourseAsync(AddSkillsToCourseDTO addSkillsToCourseDTO)
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
                        skillsToAddServiceResult.GetValue<IList<Skill>>().ToList();

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
                }

                return new ServiceResult().SetSuccessful();
            }
            catch (Exception ex)
            {
                await Task.Run(() => this.logger.LogError(ex.Message));
                return new ServiceResult().SetException(ex);
            }
        }

        public async Task<IServiceResult> AddUserEnrollmentToCourse(UserEnrollmentDTO userEnrollmentDTO)
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
                    SkillIds = courseToUpdate.SkillCourses.Select(x => x.SkillId).ToList()
                };

                await this.userService.AddSkillsToUserAsync(addSkillsToUserDTO);

                if (userToAddServiceResult.IsSuccessful)
                {
                    var userToAdd =
                        userToAddServiceResult.GetValue<IList<User>>().FirstOrDefault();

                    var userCourseEnrollment = new UserCourseEnrollment()
                    {
                        CourseId = courseToUpdate.Id,
                        Course = courseToUpdate,
                        UserId = userToAdd.Id,
                        User = userToAdd
                    };

                    courseToUpdate.UserCourseEnrollments.Add(userCourseEnrollment);
                    await this.repository.UpdateAsync(courseToUpdate);
                }

                return new ServiceResult().SetSuccessful();
            }
            catch (Exception ex)
            {
                await Task.Run(() => this.logger.LogError(ex.Message));
                return new ServiceResult().SetException(ex);
            }
        }

        public async Task<IServiceResult> RemoveUserEnrollmentFromCourse(UserEnrollmentDTO userEnrollmentDTO)
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
                        userCourseEnrollmentServiceResult.GetValue<IList<UserCourseEnrollment>>().FirstOrDefault();

                    await this.userCourseEnrollmentService.DeleteAsync(userCourseEnrollment);
                }

                return new ServiceResult().SetSuccessful();
            }
            catch (Exception ex)
            {
                await Task.Run(() => this.logger.LogError(ex.Message));
                return new ServiceResult().SetException(ex);
            }
        }

        public async Task<IServiceResult> AddMaterialsToCourse(AddMaterialsToCourseDTO addMaterialToCourseDTO)
        {
            try
            {
                var courseToUpdate =
                    (await this.repository.GetAsync(
                        x => x.Id == addMaterialToCourseDTO.CourseId)).FirstOrDefault();

                var materialsToAddServiceResult =
                    await this.materialService.GetAsync(
                        x => addMaterialToCourseDTO.MaterialIds.Contains(x.Id));

                if (materialsToAddServiceResult.IsSuccessful)
                {
                    var materialsToAdd =
                        materialsToAddServiceResult.GetValue<IList<Material>>();

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
                }

                return new ServiceResult().SetSuccessful();
            }
            catch (Exception ex)
            {
                await Task.Run(() => this.logger.LogError(ex.Message));
                return new ServiceResult().SetException(ex);
            }
        }

        public async Task<IServiceResult> RemoveMaterialFromCourse(RemoveMaterialFromCourseDTO removeMaterialFromCourseDTO)
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
                        courseMaterialRelationServiceResult.GetValue<IList<CourseMaterial>>().FirstOrDefault();

                    await this.courseMaterialService.DeleteAsync(courseMaterialRelation);
                }

                return new ServiceResult().SetSuccessful();
            }
            catch (Exception ex)
            {
                await Task.Run(() => this.logger.LogError(ex.Message));
                return new ServiceResult().SetException(ex);
            }
        }
    }
}
