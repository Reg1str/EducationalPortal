namespace EducationPortal.Domain.Core.Mappings
{
    using EducationPortal.Domain.Core.Entities;

    public class UserSkills
    {
        public int UserId { get; set; }

        public User User { get; set; }

        public int SkillId { get; set; }

        public Skill Skill { get; set; }

        public int CurrentProgress { get; set; }

        public int CurrentLevel { get; set; }
    }
}
