namespace EducationPortal.Domain.Services.Interfaces
{
    using EducationPortal.Domain.Core.Common;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IRepository<TEntity>
        where TEntity : class
    {
        Task CreateAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

        Task<IList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);

        Task<IList<TResult>> GetAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector);

        public Task<IList<TEntity>> GetPageAsync(Expression<Func<TEntity, bool>> predicate, PageInfo pageInfo);

        public Task<IList<TResult>> GetPageAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> selector,
            PageInfo pageInfo);

        Task<int> Count(Expression<Func<TEntity, bool>> predicate);

        Task SaveAsync();
    }
}
