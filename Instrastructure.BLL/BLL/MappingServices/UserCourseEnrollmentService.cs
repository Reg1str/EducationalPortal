namespace EducationPortal.Infrastructure.BLL.MappingServices
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using EducationPortal.Application.Services.MappingInterfaces;
    using EducationPortal.Application.Services.Validation;
    using EducationPortal.Domain.Core.Mappings;
    using EducationPortal.Domain.Services.Interfaces;
    using EducationPortal.Infrastructure.BLL.Services;
    using Microsoft.Extensions.Logging;

    public class UserCourseEnrollmentService : GenericService<UserCourseEnrollment>, IUserCourseEnrollmentService
    {
        private readonly IUserCourseEnrollmentValidator userCourseEnrollmentValidator;

        public UserCourseEnrollmentService(
            IRepository<UserCourseEnrollment> repository,
            ILogger<UserCourseEnrollmentService> logger,
            IUserCourseEnrollmentValidator userCourseEnrollmentValidator)
            : base(repository, logger)
        {
            this.userCourseEnrollmentValidator = userCourseEnrollmentValidator;
        }

        public async Task UpdateProgressPercentage(int userId, int courseId, int progressToAdd)
        {
            try
            {
                var enrollmentToUpdate =
                    (await this.repository.GetAsync(x => x.UserId == userId && x.CourseId == courseId)).FirstOrDefault();

                if (enrollmentToUpdate != null)
                {
                    enrollmentToUpdate.ProgressPercentage += progressToAdd;

                    if (await this.userCourseEnrollmentValidator.IsValidProgressPercentageAsync(enrollmentToUpdate))
                    {
                        await this.repository.UpdateAsync(enrollmentToUpdate);
                        await this.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
