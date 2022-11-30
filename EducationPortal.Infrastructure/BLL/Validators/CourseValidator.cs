namespace EducationPortal.Infrastructure.BLL.Validators
{
    using System.Threading.Tasks;
    using EducationPortal.Application.Services.Validation;
    using EducationPortal.Domain.Core.Entities;

    public class CourseValidator : ICourseValidator
    {
        private readonly int minimalValidTitleLength;
        private readonly int minimalValidDescriptionLength;

        public CourseValidator(
            int minimalValidTitleLength,
            int minimalValidDescriptionLength)
        {
            this.minimalValidTitleLength = minimalValidTitleLength;
            this.minimalValidDescriptionLength = minimalValidDescriptionLength;
        }

        public async Task<bool> IsValidAsync(Course entity)
        {
            if (entity == null)
            {
                return false;
            }

            return await this.IsTitleValidAsync(entity.Title) &&
                   await this.IsDescriptionValidAsync(entity.Description);
        }

        public async Task<bool> IsTitleValidAsync(string title)
        {
            return await Task.Run(() => this.IsTitleValid(title));
        }

        public async Task<bool> IsDescriptionValidAsync(string description)
        {
            return await Task.Run(() => this.IsDescriptionValid(description));
        }

        private bool IsTitleValid(string title)
        {
            if (string.IsNullOrEmpty(title) && title.Length < this.minimalValidTitleLength)
            {
                return false;
            }

            return true;
        }

        private bool IsDescriptionValid(string description)
        {
            if (string.IsNullOrEmpty(description) && description.Length < this.minimalValidDescriptionLength)
            {
                return false;
            }

            return true;
        }
    }
}
