namespace EducationPortal.EntityFramework.Mappings
{
    using EducationPortal.Domain.Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class SkillMapping : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder.HasKey(skill => skill.Id);

            builder.Property(skill => skill.Name).IsRequired();
            builder.Property(skill => skill.Name).HasMaxLength(100);

            builder.Property(skill => skill.ProgressPenalty).IsRequired();
        }
    }
}
