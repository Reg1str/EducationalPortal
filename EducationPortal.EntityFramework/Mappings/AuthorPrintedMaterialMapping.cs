namespace EducationPortal.EntityFramework.Mappings
{
    using EducationPortal.Domain.Core.Mappings;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AuthorPrintedMaterialMapping : IEntityTypeConfiguration<AuthorPrintedMaterial>
    {
        public void Configure(EntityTypeBuilder<AuthorPrintedMaterial> builder)
        {
            builder.HasKey(authorPrintedMaterial =>
                new { authorPrintedMaterial.AuthorId, authorPrintedMaterial.PrintedMaterialId });

            builder.HasOne(authorPrintedMaterial => authorPrintedMaterial.Author)
                   .WithMany(author => author.AuthorPrintedMaterials)
                   .HasForeignKey(authorPrintedMaterial => authorPrintedMaterial.AuthorId)
                   .HasPrincipalKey(author => author.Id);

            builder.HasOne(authorPrintedMaterial => authorPrintedMaterial.PrintedMaterial)
                   .WithMany(printedMaterial => printedMaterial.AuthorPrintedMaterials)
                   .HasForeignKey(authorPrintedMaterial => authorPrintedMaterial.PrintedMaterialId)
                   .HasPrincipalKey(printedMaterial => printedMaterial.Id);
        }
    }
}
