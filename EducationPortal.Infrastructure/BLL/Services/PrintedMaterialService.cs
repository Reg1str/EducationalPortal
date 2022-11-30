namespace EducationPortal.Infrastructure.BLL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using EducationPortal.Application.Services.Interfaces;
    using EducationPortal.Domain.Core.Entities;
    using EducationPortal.Domain.Services.Interfaces;
    using Microsoft.Extensions.Logging;

    public class PrintedMaterialService : GenericService<PrintedMaterial>, IPrintedMaterialService
    {
        public PrintedMaterialService(
            IRepository<PrintedMaterial> printedMaterialRepository,
            ILogger<PrintedMaterialService> logger)
            : base(printedMaterialRepository, logger)
        {

        }
    }
}
