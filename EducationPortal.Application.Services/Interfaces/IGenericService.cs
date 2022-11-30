namespace EducationPortal.Application.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using EducationPortal.Domain.Core.Common;

    public interface IGenericService<TEntity>
         where TEntity : class
    {
        Task<ServiceResult> CreateAsync(TEntity entity);

        Task<ServiceResult> UpdateAsync(TEntity entity);

        Task<ServiceResult> DeleteAsync(TEntity entity);

        Task<ServiceResult<bool>> ExistsAsync(
            Expression<Func<TEntity, bool>> predicate);

        Task<ServiceResult<IList<TEntity>>> GetAsync(
            Expression<Func<TEntity, bool>> predicate);

        Task<ServiceResult<IList<TResult>>> GetAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> selector);

        Task<ServiceResult> SaveChangesAsync();
    }
}
