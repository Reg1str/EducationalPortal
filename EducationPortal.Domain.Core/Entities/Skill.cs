namespace EducationPortal.Domain.Core.Entities
{
    using System.Collections.Generic;
    using EducationPortal.Domain.Core.Mappings;

    public class Skill : BaseEntity
    {
        public Skill()
        {
            this.UserSkills = new List<UserSkills>();
            this.SkillCourses = new List<SkillCourse>();
        }

        public string Name { get; set; }

        public int ProgressPenalty { get; set; }

        public int MaxLevel { get; set; }

        public List<UserSkills> UserSkills { get; set; }

        public List<SkillCourse> SkillCourses { get; set; }
    }
}
