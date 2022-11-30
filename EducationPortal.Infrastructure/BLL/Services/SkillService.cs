namespace EducationPortal.Infrastructure.BLL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EducationPortal.Application.Services.Interfaces;
    using EducationPortal.Application.Services.MappingInterfaces;
    using EducationPortal.Domain.Core.Common;
    using EducationPortal.Domain.Core.Entities;
    using EducationPortal.Domain.Services.Interfaces;
    using Microsoft.Extensions.Logging;

    public class SkillService : GenericService<Skill>, ISkillService
    {
        private readonly IUserSkillsService userSkillService;
        private readonly ISkillCourseService skillCourseService;

        public SkillService(
            IRepository<Skill> skillRepository,
            ILogger<SkillService> logger,
            IUserSkillsService userSkillService,
            ISkillCourseService skillCourseService)
            : base(skillRepository, logger)
        {
            this.userSkillService = userSkillService;
            this.skillCourseService = skillCourseService;
        }

        public async Task<IServiceResult> GetSkillNamesForCourse(int courseId)
        {
            try
            {
                var skillNamesServiceResult =
                    await this.skillCourseService.GetAsync(
                        x => x.CourseId == courseId,
                        x => x.Skill.Name);

                if (skillNamesServiceResult.IsSuccessful)
                {
                    return new ServiceResult().SetValue<List<string>>(
                        skillNamesServiceResult.GetValue<List<string>>());
                }

                return new ServiceResult().SetException(new Exception("Failed to find skills for courseid"));
            }
            catch (Exception ex)
            {
                await Task.Run(() => this.logger.LogError(ex.Message));
                return new ServiceResult().SetException(ex);
            }
        }

        public async Task<IServiceResult> GetAcomplishedSkillsForUser(int userId)
        {
            return await this.userSkillService.GetAsync(
                x => x.UserId == userId && x.CurrentLevel == x.Skill.MaxLevel, x => x.Skill);
        }
    }
}
