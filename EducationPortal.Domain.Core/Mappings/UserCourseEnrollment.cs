
namespace EducationPortal.Domain.Core.Mappings
{
    using EducationPortal.Domain.Core.Entities;

    public class UserCourseEnrollment
    {
        public int UserId { get; set; }

        public User User { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public int ProgressPercentage { get; set; }
    }
}
