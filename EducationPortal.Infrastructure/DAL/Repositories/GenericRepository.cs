namespace EducationPortal.Infrastructure.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using EducationPortal.Domain.Services.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class GenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly DbContext dataContext;

        public GenericRepository(DbContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public virtual async Task CreateAsync(TEntity entity)
        {
            await this.dataContext.Set<TEntity>().AddAsync(entity);
            await this.SaveAsync();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            this.dataContext.Set<TEntity>().Remove(entity);
            await this.SaveAsync();
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

        public virtual async Task SaveAsync()
        {
            await this.dataContext.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            this.dataContext.Set<TEntity>().Update(entity);
            await this.SaveAsync();
        }
    }
}
