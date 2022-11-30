using EducationPortal.Application.Services.MappingInterfaces;
using EducationPortal.Domain.Core.Mappings;
using EducationPortal.Domain.Services.Interfaces;
using EducationPortal.Infrastructure.BLL.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Instrastructure.BLL.BLL.MappingServices
{
    public class UserMaterialsService : GenericService<UserMaterials>, IUserMaterialsService
    {
        public UserMaterialsService(
            IRepository<UserMaterials> repository,
            ILogger<UserMaterialsService> logger)
            : base(repository, logger)
        {
        }
    }
}
