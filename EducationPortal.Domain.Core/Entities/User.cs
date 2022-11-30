namespace EducationPortal.Domain.Core.Entities
{
    using System.Collections.Generic;
    using EducationPortal.Domain.Core.Mappings;

    public class User : BaseEntity
    {
        public User()
        {
            this.UserCourseOwners = new List<UserCourseOwner>();
            this.UserSkills = new List<UserSkills>();
            this.UserCourseEnrollments = new List<UserCourseEnrollment>();
            this.UserMaterials = new List<UserMaterials>();
        }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string UserType { get; set; }

        public List<UserCourseOwner> UserCourseOwners { get; set; }

        public List<UserSkills> UserSkills { get; set; }

        public List<UserCourseEnrollment> UserCourseEnrollments { get; set; }

        public List<UserMaterials> UserMaterials { get; set; }
    }
}
