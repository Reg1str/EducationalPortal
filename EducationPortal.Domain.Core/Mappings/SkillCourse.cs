namespace EducationPortal.Domain.Core.Mappings
{
    using EducationPortal.Domain.Core.Entities;

    public class SkillCourse
    {
        public int SkillId { get; set; }

        public Skill Skill { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }
    }
}
