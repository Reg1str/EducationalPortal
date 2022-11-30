namespace EducationPortal.EntityFramework.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using EducationPortal.Domain.Core.Common;
    using EducationPortal.Domain.Services.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class GenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly DbContext dataContext;
        protected readonly ILogger logger;

        public GenericRepository(
            DbContext dataContext,
            ILogger<GenericRepository<TEntity>> logger)
        {
            this.dataContext = dataContext;
            this.logger = logger;
        }

        public virtual async Task<int> Count(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await this.dataContext.Set<TEntity>().CountAsync(predicate);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                throw;
            }
        }

        public virtual async Task CreateAsync(TEntity entity)
        {
            await this.dataContext.Set<TEntity>().AddAsync(entity);
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            await Task.Run(() =>
            {
                this.dataContext.Set<TEntity>().Remove(entity);
            });
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await this.dataContext.Set<TEntity>().AnyAsync(predicate);
        }

        public virtual async Task<IList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await this.dataContext.Set<TEntity>()
                .AsQueryable<TEntity>()
                .Where(predicate)
                .ToListAsync();
        }

        public virtual async Task<IList<TResult>> GetAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> selector)
        {
            return await this.dataContext.Set<TEntity>()
                .AsQueryable<TEntity>()
                .Where(predicate)
                .Select(selector)
                .ToListAsync();
        }

        public async Task<IList<TEntity>> GetPageAsync(
            Expression<Func<TEntity, bool>> predicate,
            PageInfo pageInfo)
        {
            var skip = pageInfo.PageSize * (pageInfo.PageNumber - 1);
            return await this.dataContext.Set<TEntity>()
                                         .Where(predicate)
                                         .Skip(skip).Take(pageInfo.PageSize).ToListAsync();
        }

        public async Task<IList<TResult>> GetPageAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> selector,
            PageInfo pageInfo)
        {
            var skip = pageInfo.PageSize * (pageInfo.PageNumber - 1);
            return await this.dataContext.Set<TEntity>()
                                         .Where(predicate)
                                         .Select(selector)
                                         .Skip(skip).Take(pageInfo.PageSize).ToListAsync();
        }

        public virtual async Task SaveAsync()
        {
            await this.dataContext.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            this.dataContext.Set<TEntity>().Update(entity);
        }
    }
}
