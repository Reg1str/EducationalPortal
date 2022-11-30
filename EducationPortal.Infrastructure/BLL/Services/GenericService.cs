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

        public virtual async Task<IServiceResult> CreateAsync(TEntity entity)
        {
            try
            {
                await this.repository.CreateAsync(entity);
                return new ServiceResult().SetSuccessful();
            }
            catch (Exception ex)
            {
                await Task.Run(() => this.logger.LogError(ex.Message));
                return new ServiceResult().SetException(ex);
            }
        }

        public virtual async Task<IServiceResult> DeleteAsync(TEntity entity)
        {
            try
            {
                await this.repository.DeleteAsync(entity);
                return new ServiceResult().SetSuccessful();
            }
            catch (Exception ex)
            {
                await Task.Run(() => this.logger.LogError(ex.Message));
                return new ServiceResult().SetException(ex);
            }
        }

        public virtual async Task<IServiceResult> ExistsAsync(
            Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return new ServiceResult().SetValue<bool>(
                    await this.repository.ExistsAsync(predicate));
            }
            catch (Exception ex)
            {
                await Task.Run(() => this.logger.LogError(ex.Message));
                return new ServiceResult().SetException(ex);
            }
        }

        public virtual async Task<IServiceResult> GetAsync(
            Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return new ServiceResult().SetValue<IList<TEntity>>(
                    await this.repository.GetAsync(predicate));
            }
            catch (Exception ex)
            {
                await Task.Run(() => this.logger.LogError(ex.Message));
                throw;
            }
        }

        public virtual async Task<IServiceResult> GetAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> selector)
        {
            try
            {
                return new ServiceResult().SetValue<IList<TResult>>(
                    await this.repository.GetAsync(predicate, selector));
            }
            catch (Exception ex)
            {
                await Task.Run(() => this.logger.LogError(ex.Message));
                throw;
            }
        }

        public virtual async Task<IServiceResult> UpdateAsync(TEntity entity)
        {
            try
            {
                await this.repository.UpdateAsync(entity);
                return new ServiceResult().SetSuccessful();
            }
            catch (Exception ex)
            {
                await Task.Run(() => this.logger.LogError(ex.Message));
                return new ServiceResult().SetException(ex);
            }
        }
    }
}
