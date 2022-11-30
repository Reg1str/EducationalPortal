namespace EducationPortal.Application.Services.MappingInterfaces
{
    using EducationPortal.Application.Services.Interfaces;
    using EducationPortal.Domain.Core.Common;
    using EducationPortal.Domain.Core.Mappings;
    using System.Threading.Tasks;

    public interface ICourseMaterialService : IGenericService<CourseMaterial>
    {
        Task<ServiceResult<int>> CountMaterialsForCourse(int courseId);
    }
}
