namespace EducationPortal.Infrastructure.BLL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using EducationPortal.Application.Services.DTO;
    using EducationPortal.Application.Services.Interfaces;
    using EducationPortal.Domain.Core.Common;
    using EducationPortal.Domain.Core.Entities;
    using EducationPortal.Domain.Core.Mappings;
    using EducationPortal.Domain.Services.Interfaces;
    using Microsoft.Extensions.Logging;

    public class PrintedMaterialService : GenericService<PrintedMaterial>, IPrintedMaterialService
    {
        private readonly IAuthorService authorService;

        public PrintedMaterialService(
            IRepository<PrintedMaterial> printedMaterialRepository,
            ILogger<PrintedMaterialService> logger,
            IAuthorService authorService)
            : base(printedMaterialRepository, logger)
        {
            this.authorService = authorService;
        }

        public async Task<ServiceResult> CreatePrintedMaterialAsync(PrintedMaterialDTO printedMaterialDTO)
        {
            try
            {
                var printedMaterialEntity = new PrintedMaterial()
                {
                    Title = printedMaterialDTO.Title,
                    Description = printedMaterialDTO.Description,
                    PagesCount = printedMaterialDTO.PagesCount,
                    Type = printedMaterialDTO.Type
                };

                var author = new Author()
                {
                    Name = printedMaterialDTO.AuthorNames[0]
                };

                var authorPrintedMaterial = new AuthorPrintedMaterial()
                {
                    Author = author,
                    PrintedMaterial = printedMaterialEntity
                };

                printedMaterialEntity.AuthorPrintedMaterials.Add(authorPrintedMaterial);

                await this.repository.CreateAsync(printedMaterialEntity);
                await this.SaveChangesAsync();
                return new ServiceResult();
            }
            catch (Exception ex )
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult(ex);
            }
        }
    }
}
