namespace EducationPortal.Application.Services.Validation
{
    using System.Threading.Tasks;

    public interface IGenericValidator<TEntity>
    {
        Task<bool> IsValidAsync(TEntity entity);
    }
}
