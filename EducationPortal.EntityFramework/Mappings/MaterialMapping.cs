namespace EducationPortal.EntityFramework.Mappings
{
    using EducationPortal.Domain.Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class MaterialMapping : IEntityTypeConfiguration<Material>
    {
        public void Configure(EntityTypeBuilder<Material> builder)
        {
            builder.HasKey(material => material.Id);

            builder.Property(material => material.Title).IsRequired();
            builder.Property(material => material.Title).HasMaxLength(100);

            builder.Property(material => material.Type);
        }
    }
}
