namespace EducationPortal.Infrastructure.BLL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using EducationPortal.Application.Services.Interfaces;
    using EducationPortal.Domain.Core.Common;
    using EducationPortal.Domain.Services.Interfaces;
    using Microsoft.Extensions.Logging;

    public abstract class GenericService<TEntity> : IGenericService<TEntity>
        where TEntity : class
    {
        protected readonly IRepository<TEntity> repository;
        protected readonly ILogger logger;

        public GenericService(IRepository<TEntity> repository, ILogger logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public virtual async Task<ServiceResult> CreateAsync(TEntity entity)
        {
            try
            {
                await this.repository.CreateAsync(entity);
                return new ServiceResult();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult(ex);
            }
        }

        public virtual async Task<ServiceResult> DeleteAsync(TEntity entity)
        {
            try
            {
                await this.repository.DeleteAsync(entity);
                return new ServiceResult();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult(ex);
            }
        }

        public virtual async Task<ServiceResult<bool>> ExistsAsync(
            Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return new ServiceResult<bool>(
                    await this.repository.ExistsAsync(predicate));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult<bool>(ex);
            }
        }

        public virtual async Task<ServiceResult<IList<TEntity>>> GetAsync(
            Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return new ServiceResult<IList<TEntity>>(
                    await this.repository.GetAsync(predicate));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult<IList<TEntity>>(ex);
            }
        }

        public virtual async Task<ServiceResult<IList<TResult>>> GetAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> selector)
        {
            try
            {
                return new ServiceResult<IList<TResult>>(
                    await this.repository.GetAsync(predicate, selector));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult<IList<TResult>>(ex);
            }
        }

        public async Task<ServiceResult> SaveChangesAsync()
        {
            try
            {
                await this.repository.SaveAsync();
                return new ServiceResult();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult(ex);
            }
        }

        public virtual async Task<ServiceResult> UpdateAsync(TEntity entity)
        {
            try
            {
                await this.repository.UpdateAsync(entity);
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
