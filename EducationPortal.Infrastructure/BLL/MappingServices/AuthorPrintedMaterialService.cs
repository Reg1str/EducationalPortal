namespace EducationPortal.Infrastructure.BLL.MappingServices
{
    using EducationPortal.Application.Services.MappingInterfaces;
    using EducationPortal.Domain.Core.Mappings;
    using EducationPortal.Domain.Services.Interfaces;
    using EducationPortal.Infrastructure.BLL.Services;
    using Microsoft.Extensions.Logging;

    public class AuthorPrintedMaterialService : GenericService<AuthorPrintedMaterial>, IAuthorPrintedMaterialService
    {
        public AuthorPrintedMaterialService(
            IRepository<AuthorPrintedMaterial> repository,
            ILogger<AuthorPrintedMaterialService> logger)
            : base(repository, logger)
        {

        }
    }
}
