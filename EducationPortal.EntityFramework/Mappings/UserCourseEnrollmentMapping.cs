namespace EducationPortal.EntityFramework.Mappings
{
    using EducationPortal.Domain.Core.Mappings;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserCourseEnrollmentMapping : IEntityTypeConfiguration<UserCourseEnrollment>
    {
        public void Configure(EntityTypeBuilder<UserCourseEnrollment> builder)
        {
            builder.HasKey(enrollment => new { enrollment.CourseId, enrollment.UserId });

            builder.HasOne(enrollment => enrollment.Course)
                   .WithMany(course => course.UserCourseEnrollments)
                   .HasForeignKey(enrollment => enrollment.CourseId)
                   .HasPrincipalKey(course => course.Id);

            builder.HasOne(enrollment => enrollment.User)
                   .WithMany(user => user.UserCourseEnrollments)
                   .HasForeignKey(enrollment => enrollment.UserId)
                   .HasPrincipalKey(user => user.Id);
        }
    }
}
