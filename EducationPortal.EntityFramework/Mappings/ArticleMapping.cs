namespace EducationPortal.EntityFramework.Mappings
{
    using EducationPortal.Domain.Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ArticleMapping : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.ToTable("Article");

            builder.Property(article => article.SourceUrl).IsRequired();
        }
    }
}
