namespace EducationPortal.Domain.Core.Entities
{
    using System.Collections.Generic;
    using EducationPortal.Domain.Core.Mappings;

    public class Course : BaseEntity
    {
        public Course()
        {
            this.UserCourseOwners = new List<UserCourseOwner>();
            this.SkillCourses = new List<SkillCourse>();
            this.CourseMaterials = new List<CourseMaterial>();
            this.UserCourseEnrollments = new List<UserCourseEnrollment>();
        }

        public string Title { get; set; }

        public string Description { get; set;  }

        public List<UserCourseOwner> UserCourseOwners { get; set; }

        public List<SkillCourse> SkillCourses { get; set; }

        public List<CourseMaterial> CourseMaterials { get; set; }

        public List<UserCourseEnrollment> UserCourseEnrollments { get; set; }
    }
}
