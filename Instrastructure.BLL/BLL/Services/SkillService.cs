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

    public class SkillService : GenericService<Skill>, ISkillService
    {
        private readonly IUserSkillsService userSkillService;
        private readonly ISkillCourseService skillCourseService;
        private readonly IUserCourseEnrollmentService userCourseEnrollmentService;

        public SkillService(
            IRepository<Skill> skillRepository,
            ILogger<SkillService> logger,
            IUserSkillsService userSkillService,
            ISkillCourseService skillCourseService,
            IUserCourseEnrollmentService userCourseEnrollmentService)
            : base(skillRepository, logger)
        {
            this.userSkillService = userSkillService;
            this.skillCourseService = skillCourseService;
            this.userCourseEnrollmentService = userCourseEnrollmentService;
        }

        public async Task<ServiceResult> UpdateSkillProgressAsync(int materialId, int userId)
        {
            try
            {
                var coursesWithMaterial=
                    (await this.userCourseEnrollmentService.GetAsync(
                        x => x.UserId == userId && x.Course.CourseMaterials.Select(y => y.MaterialId).Contains(materialId),
                        x => x.Course.Id)).Value;

                var skillCourses =
                    (await this.skillCourseService.GetAsync(x => coursesWithMaterial.Contains(x.CourseId), x => x.SkillId)).Value;

                var userSkillsResult =
                    await this.userSkillService.GetAsync(
                        x => x.UserId == userId && skillCourses.Contains(x.SkillId),
                        x => new UserSkills()
                        {
                            SkillId = x.SkillId,
                            Skill = x.Skill,
                            User = x.User,
                            UserId = x.UserId,
                            CurrentProgress = x.CurrentProgress
                        });

                if (userSkillsResult.IsSuccessful)
                {
                    var userSkills = userSkillsResult.Value;

                    foreach (var userSkill in userSkills)
                    {
                        userSkill.CurrentProgress = userSkill.CurrentProgress + userSkill.Skill.ProgressPenalty <= 100
                            ? userSkill.CurrentProgress + userSkill.Skill.ProgressPenalty : 100;

                        await this.userSkillService.UpdateAsync(userSkill);
                    }

                    await this.userSkillService.SaveChangesAsync();
                }

                return new ServiceResult();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult(ex);
            }
        }

        public async Task<ServiceResult<IList<SkillInfoDTO>>> GetSkillInfoDTOs(int courseId, int userId, bool IsAuthorized)
        {
            try
            {
                var skillDTOsResult =
                await this.skillCourseService.GetAsync(
                    x => x.CourseId == courseId,
                    x => new SkillInfoDTO()
                    {
                        Id = x.SkillId,
                        Name = x.Skill.Name,
                        LevelThreshold = x.Skill.MaxLevel,
                    });

                if (skillDTOsResult.IsSuccessful)
                {
                    var skillDtos = skillDTOsResult.Value;

                    if (IsAuthorized)
                    {
                        var userSkillResult =
                        await this.userSkillService.GetAsync(
                            x => x.UserId == userId,
                            x => x);

                        if (userSkillResult.IsSuccessful)
                        {
                            foreach (var skill in skillDtos)
                            {
                                var userSkill = userSkillResult.Value.Where(x => x.SkillId == skill.Id).FirstOrDefault();
                                skill.UserProgress = userSkill == null ? 0 : userSkill.CurrentProgress;
                            }
                        }
                    }
                    return new ServiceResult<IList<SkillInfoDTO>>(skillDtos);
                }

                return new ServiceResult<IList<SkillInfoDTO>>(new Exception("Failed to get SkillInfoDTOs"));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult<IList<SkillInfoDTO>>(ex);
            } 
        }

        public async Task<ServiceResult<SkillPageDTO>> GetSkillsAvailableForCourseAsync(int courseId, PageInfo pageInfo)
        {
            try
            {
                var skillInfoDTOs =
                    await this.repository.GetPageAsync(
                        x => !x.SkillCourses.Select(y => y.CourseId).Contains(courseId),
                        x => new SkillInfoDTO()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            LevelThreshold = x.MaxLevel
                        },
                        pageInfo);

                var lastPageNumber = (int)Math.Ceiling(
                    (decimal)await this.repository.Count(x => true) / pageInfo.PageSize);

                var skillPageDTO = new SkillPageDTO()
                {
                    SkillInfoDTOs = skillInfoDTOs,
                    LastPageNumber = lastPageNumber > 0 ? lastPageNumber : 1,
                    PageNumber = pageInfo.PageNumber
                };

                return new ServiceResult<SkillPageDTO>(skillPageDTO);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult<SkillPageDTO>(ex);
            }
        }

        public async Task<ServiceResult<SkillPageDTO>> GetSkillPage(PageInfo pageInfo)
        {
            try
            {
                var skillInfoDTOs =
                    await this.repository.GetPageAsync(
                        x => true,
                        x => new SkillInfoDTO()
                        {
                            Id = x.Id,
                            Name = x.Name
                        },
                        pageInfo);

                var lastPageNumber = (int)Math.Ceiling(
                    (decimal)await this.repository.Count(x => true) / pageInfo.PageSize);

                var skillPageDTO = new SkillPageDTO()
                {
                    SkillInfoDTOs = skillInfoDTOs,
                    LastPageNumber = lastPageNumber > 0 ? lastPageNumber : 1,
                    PageNumber = pageInfo.PageNumber
                }; 

                return new ServiceResult<SkillPageDTO>(skillPageDTO);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult<SkillPageDTO>(ex);
            }
        }

        public async Task<ServiceResult<IList<string>>> GetSkillNamesForCourse(int courseId)
        {
            try
            {
                var skillNamesServiceResult =
                    await this.skillCourseService.GetAsync(
                        x => x.CourseId == courseId,
                        x => x.Skill.Name);

                if (skillNamesServiceResult.IsSuccessful)
                {
                    return new ServiceResult<IList<string>>(skillNamesServiceResult.Value);
                }

                return new ServiceResult<IList<string>>(new Exception("Failed to find skills for courseid"));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult<IList<string>>(ex);
            }
        }

        public async Task<ServiceResult<IList<SkillInfoDTO>>> GetSkillsForUserAsync(int userId)
        {
            try
            {
                return await this.userSkillService.GetAsync(
                    x => x.UserId == userId, 
                    x => new SkillInfoDTO()
                    {
                        Id = x.Skill.Id,
                        Name = x.Skill.Name,
                        UserProgress = x.CurrentProgress
                    });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult<IList<SkillInfoDTO>>(ex);
            }
        }

        public async Task<ServiceResult> CreateSkillAsync(SkillCreateDTO skillCreateDTO)
        {
            try
            {
                var skillEntity = new Skill()
                {
                    Name = skillCreateDTO.Name,
                    ProgressPenalty = skillCreateDTO.Penalty,
                    MaxLevel = skillCreateDTO.MaxLevel
                };

                await this.repository.CreateAsync(skillEntity);
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
