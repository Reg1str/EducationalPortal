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
    using EducationPortal.Domain.Services.Interfaces;
    using Microsoft.Extensions.Logging;

    public class ArticleService : GenericService<Article>, IArticleService
    {
        public ArticleService(IRepository<Article> articleRepository, ILogger<ArticleService> logger)
            : base(articleRepository, logger)
        {
        }

        public async Task<ServiceResult> CreateArticleAsync(ArticleDTO articleDTO)
        {
            try
            {
                var articleEntity = new Article()
                {
                    Title = articleDTO.Title,
                    Description = articleDTO.Description,
                    PublishedDate = articleDTO.PublishedDate,
                    SourceUrl = articleDTO.SourceUrl,
                    Type = articleDTO.Type
                };

                await this.repository.CreateAsync(articleEntity);
                await this.SaveChangesAsync();

                return new ServiceResult();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult(ex);
            }
        }
    }
}
