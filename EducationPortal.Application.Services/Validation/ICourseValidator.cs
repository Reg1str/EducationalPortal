namespace EducationPortal.Application.Services.Validation
{
    using System.Threading.Tasks;
    using EducationPortal.Domain.Core.Entities;

    public interface ICourseValidator : IGenericValidator<Course>
    {
        Task<bool> IsTitleValidAsync(string title);

        Task<bool> IsDescriptionValidAsync(string title);
    }
}
