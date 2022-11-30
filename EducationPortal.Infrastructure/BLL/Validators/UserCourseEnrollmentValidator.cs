namespace EducationPortal.Infrastructure.BLL.Validators
{
    using System.Threading.Tasks;
    using EducationPortal.Application.Services.Validation;
    using EducationPortal.Domain.Core.Mappings;

    public class UserCourseEnrollmentValidator : IUserCourseEnrollmentValidator
    {

        private readonly int maximalProgressPercentage;
        private readonly int minimalProgressPercentage;

        public UserCourseEnrollmentValidator(
            int maximalProgressPercentage,
            int minimalProgressPercentage)
        {
            this.maximalProgressPercentage = maximalProgressPercentage;
            this.minimalProgressPercentage = minimalProgressPercentage;
        }

        public async Task<bool> IsValidAsync(UserCourseEnrollment entity)
        {
            if (entity != null && await this.IsValidProgressPercentageAsync(entity))
            {
                return true;
            }

            return false;
        }

        public async Task<bool> IsValidProgressPercentageAsync(UserCourseEnrollment enrollment)
        {
            return await Task.Run(() => this.IsValidProgressPercentage(enrollment));
        }

        private bool IsValidProgressPercentage(UserCourseEnrollment enrollment)
        {
            return enrollment.ProgressPercentage >= this.minimalProgressPercentage &&
                enrollment.ProgressPercentage <= this.maximalProgressPercentage;
        }
    }
}
