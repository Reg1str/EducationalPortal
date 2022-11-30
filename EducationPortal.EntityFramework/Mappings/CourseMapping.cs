namespace EducationPortal.EntityFramework.Mappings
{
    using EducationPortal.Domain.Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CourseMapping : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(course => course.Id);

            builder.Property(course => course.Title).IsRequired();
        }
    }
}
