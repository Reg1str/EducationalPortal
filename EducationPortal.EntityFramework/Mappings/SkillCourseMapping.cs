namespace EducationPortal.EntityFramework.Mappings
{
    using EducationPortal.Domain.Core.Mappings;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class SkillCourseMapping : IEntityTypeConfiguration<SkillCourse>
    {
        public void Configure(EntityTypeBuilder<SkillCourse> builder)
        {
            builder.HasKey(skillCourse => new { skillCourse.CourseId, skillCourse.SkillId });

            builder.HasOne(skillCourse => skillCourse.Course)
                   .WithMany(course => course.SkillCourses)
                   .HasForeignKey(skillCourse => skillCourse.CourseId)
                   .HasPrincipalKey(course => course.Id);

            builder.HasOne(skillCourse => skillCourse.Skill)
                   .WithMany(skill => skill.SkillCourses)
                   .HasForeignKey(skillCourse => skillCourse.SkillId)
                   .HasPrincipalKey(skill => skill.Id);
        }
    }
}
