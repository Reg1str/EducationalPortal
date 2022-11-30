namespace EducationPortal.EntityFramework.Mappings
{
    using EducationPortal.Domain.Core.Mappings;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserCourseOwnerMapping : IEntityTypeConfiguration<UserCourseOwner>
    {
        public void Configure(EntityTypeBuilder<UserCourseOwner> builder)
        {
            builder.HasKey(userCourseOwner => new { userCourseOwner.CourseId, userCourseOwner.UserId });

            builder.HasOne(userCourseOwner => userCourseOwner.Course)
                   .WithMany(course => course.UserCourseOwners)
                   .HasForeignKey(userCourseOwner => userCourseOwner.CourseId)
                   .HasPrincipalKey(course => course.Id);

            builder.HasOne(userCourseOwner => userCourseOwner.User)
                   .WithMany(user => user.UserCourseOwners)
                   .HasForeignKey(userCourseOwner => userCourseOwner.UserId)
                   .HasPrincipalKey(user => user.Id);
        }
    }
}
