namespace EducationPortal.EntityFramework.Mappings
{
    using EducationPortal.Domain.Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(user => user.Id);

            builder.Property(user => user.Firstname).IsRequired();
            builder.Property(user => user.Firstname).HasMaxLength(30);

            builder.Property(user => user.Lastname).IsRequired();
            builder.Property(user => user.Lastname).HasMaxLength(30);

            builder.Property(user => user.UserType).IsRequired();

            builder.Property(user => user.Email).IsRequired();
            builder.Property(user => user.Email).HasMaxLength(50);

            builder.Property(user => user.Password).IsRequired();
            builder.Property(user => user.Password).HasMaxLength(25);
        }
    }
}
