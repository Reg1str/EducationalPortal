namespace EducationPortal.Infrastructure.BLL.MappingServices
{
    using EducationPortal.Application.Services.MappingInterfaces;
    using EducationPortal.Domain.Core.Mappings;
    using EducationPortal.Domain.Services.Interfaces;
    using EducationPortal.Infrastructure.BLL.Services;
    using Microsoft.Extensions.Logging;

    public class UserCourseOwnerService : GenericService<UserCourseOwner>, IUserCourseOwnerService
    {
        public UserCourseOwnerService(
            IRepository<UserCourseOwner> repository,
            ILogger<UserCourseEnrollmentService> logger)
            : base(repository, logger)
        {
        }
    }
}
