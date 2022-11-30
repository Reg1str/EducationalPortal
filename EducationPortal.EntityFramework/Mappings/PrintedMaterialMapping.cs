namespace EducationPortal.EntityFramework.Mappings
{
    using EducationPortal.Domain.Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class PrintedMaterialMapping : IEntityTypeConfiguration<PrintedMaterial>
    {
        public void Configure(EntityTypeBuilder<PrintedMaterial> builder)
        {
            builder.ToTable("PrintedMaterial");
        }
    }
}
