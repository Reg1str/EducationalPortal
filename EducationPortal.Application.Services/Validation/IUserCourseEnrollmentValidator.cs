namespace EducationPortal.Application.Services.Validation
{
    using EducationPortal.Domain.Core.Mappings;
    using System.Threading.Tasks;

    public interface IUserCourseEnrollmentValidator : IGenericValidator<UserCourseEnrollment>
    {
        Task<bool> IsValidProgressPercentageAsync(UserCourseEnrollment enrollment);
    }
}
