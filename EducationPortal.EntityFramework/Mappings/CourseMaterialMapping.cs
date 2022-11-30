namespace EducationPortal.EntityFramework.Mappings
{
    using EducationPortal.Domain.Core.Mappings;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CourseMaterialMapping : IEntityTypeConfiguration<CourseMaterial>
    {
        public void Configure(EntityTypeBuilder<CourseMaterial> builder)
        {
            builder.HasKey(courseMaterial =>
                new { courseMaterial.CourseId, courseMaterial.MaterialId });

            builder.HasOne(courseMaterial => courseMaterial.Course)
                   .WithMany(course => course.CourseMaterials)
                   .HasForeignKey(courseMaterial => courseMaterial.CourseId)
                   .HasPrincipalKey(course => course.Id);

            builder.HasOne(courseMaterial => courseMaterial.Material)
                   .WithMany(material => material.CourseMaterials)
                   .HasForeignKey(courseMaterial => courseMaterial.MaterialId)
                   .HasPrincipalKey(material => material.Id);
        }
    }
}
