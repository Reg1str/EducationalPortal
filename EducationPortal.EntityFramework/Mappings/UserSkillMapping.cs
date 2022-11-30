namespace EducationPortal.EntityFramework.Mappings
{
    using EducationPortal.Domain.Core.Mappings;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserSkillMapping : IEntityTypeConfiguration<UserSkills>
    {
        public void Configure(EntityTypeBuilder<UserSkills> builder)
        {
            builder.HasKey(userSkill => new { userSkill.SkillId, userSkill.UserId });

            builder.Property(userSkill => userSkill.CurrentProgress).IsRequired();
            builder.Property(userSkill => userSkill.CurrentLevel).IsRequired();

            builder.HasOne(userSkill => userSkill.User)
                   .WithMany(user => user.UserSkills)
                   .HasForeignKey(userSkill => userSkill.UserId)
                   .HasPrincipalKey(user => user.Id);

            builder.HasOne(userSkill => userSkill.Skill)
                   .WithMany(skill => skill.UserSkills)
                   .HasForeignKey(userSkill => userSkill.SkillId)
                   .HasPrincipalKey(skill => skill.Id);
        }
    }
}
