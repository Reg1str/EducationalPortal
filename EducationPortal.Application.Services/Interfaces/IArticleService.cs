namespace EducationPortal.Application.Services.Interfaces
{
    using EducationPortal.Application.Services.DTO;
    using EducationPortal.Domain.Core.Common;
    using EducationPortal.Domain.Core.Entities;
    using System.Threading.Tasks;

    public interface IArticleService : IGenericService<Article>
    {
        Task<ServiceResult> CreateArticleAsync(ArticleDTO articleDTO);
    }
}
