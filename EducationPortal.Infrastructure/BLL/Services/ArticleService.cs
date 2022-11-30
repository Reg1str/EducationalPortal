namespace EducationPortal.Infrastructure.BLL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using EducationPortal.Application.Services.Interfaces;
    using EducationPortal.Domain.Core.Entities;
    using EducationPortal.Domain.Services.Interfaces;
    using Microsoft.Extensions.Logging;

    public class ArticleService : GenericService<Article>, IArticleService
    {
        public ArticleService(IRepository<Article> articleRepository, ILogger<ArticleService> logger)
            : base(articleRepository, logger)
        {
        }
    }
}
