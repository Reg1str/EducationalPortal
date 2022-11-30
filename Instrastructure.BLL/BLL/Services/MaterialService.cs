namespace EducationPortal.Infrastructure.BLL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using EducationPortal.Application.Services.DTO;
    using EducationPortal.Application.Services.Interfaces;
    using EducationPortal.Application.Services.MappingInterfaces;
    using EducationPortal.Domain.Core.Common;
    using EducationPortal.Domain.Core.Entities;
    using EducationPortal.Domain.Core.Mappings;
    using EducationPortal.Domain.Services.Interfaces;
    using Microsoft.Extensions.Logging;

    public class MaterialService : GenericService<Material>, IMaterialService
    {
        private readonly ICourseMaterialService courseMaterialService;
        private readonly IUserMaterialsService userMaterialsService;
        private readonly IArticleService articleService;
        private readonly IPrintedMaterialService printedMaterialService;
        private readonly IVideoService videoService;
        private readonly IUserCourseEnrollmentService userCourseEnrollmentService;

        public MaterialService(
            IRepository<Material> materialRepository,
            ILogger<MaterialService> logger,
            IVideoService videoService,
            IArticleService articleService,
            IPrintedMaterialService printedMaterialService,
            ICourseMaterialService courseMaterialService,
            IUserMaterialsService userMaterialsService,
            IUserCourseEnrollmentService userCourseEnrollmentService)
            : base(materialRepository, logger)
        {
            this.courseMaterialService = courseMaterialService;
            this.userMaterialsService = userMaterialsService;
            this.videoService = videoService;
            this.printedMaterialService = printedMaterialService;
            this.articleService = articleService;
            this.userCourseEnrollmentService = userCourseEnrollmentService;
        }

        public async Task<ServiceResult> FinishMaterial(int materialId, int userId)
        {
            try
            {
                var userMaterialResult =
                    await this.userMaterialsService.GetAsync(x => x.MaterialId == materialId && x.UserId == userId);

                if (userMaterialResult.IsSuccessful)
                {
                    var userMaterial = userMaterialResult.Value.FirstOrDefault();

                    userMaterial.IsFinished = true;

                    await this.userMaterialsService.UpdateAsync(userMaterial);
                    await this.userMaterialsService.SaveChangesAsync();
                }

                return new ServiceResult();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult(ex);
            }
        }

        public async Task<ServiceResult> CreateMaterialAsync(MaterialInfoDTO materialInfoDTO) 
        {
            try
            {
                var materialExists =
                    await this.repository.ExistsAsync(x => x.Title == materialInfoDTO.Title);

                if (materialExists)
                {
                    return new ServiceResult(new Exception("Material with the same Title already exists"));
                }

                switch(materialInfoDTO.Type)
                {
                    case "Article":
                        await this.articleService.CreateArticleAsync((ArticleDTO)materialInfoDTO);
                        break;
                    case "PrintedMaterial":
                        await this.printedMaterialService.CreatePrintedMaterialAsync((PrintedMaterialDTO)materialInfoDTO);
                        break;
                    case "Video":
                        await this.videoService.CreateVideoAsync((VideoDTO)materialInfoDTO);
                        break;
                }

                return new ServiceResult();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult(ex);
            }
        }

        public async Task<ServiceResult<MaterialInfoDTO>> GetMaterialInfoAsync(int materialId, int userId)
        {
            try
            {
                var userMaterial =
                    (await this.userMaterialsService.GetAsync(x => x.MaterialId == materialId && x.UserId == userId)).Value.FirstOrDefault();

                var userEnrollment =
                    (await this.userCourseEnrollmentService.ExistsAsync(
                        x => x.Course.CourseMaterials.Select(y => y.MaterialId).Contains(materialId) && x.UserId == userId)).Value;

                var materialInfoDTO =
                    (await this.repository.GetAsync(
                        x => x.Id == materialId,
                        x => new MaterialInfoDTO() 
                        { 
                            Id = x.Id,
                            Title = x.Title,
                            Description = x.Description,
                            Type = x.Type,
                        })).FirstOrDefault();

                switch (materialInfoDTO.Type)
                {
                    case "Article":
                        var getArticleResult =
                            await this.articleService.GetAsync(
                                x => x.Id == materialId,
                                x => new ArticleDTO()
                                {
                                    Id = x.Id,
                                    Title = x.Title,
                                    Description = x.Description,
                                    Type = x.Type,
                                    SourceUrl = x.SourceUrl,
                                    PublishedDate = x.PublishedDate
                                });

                        if (getArticleResult.IsSuccessful)
                        {
                            materialInfoDTO = getArticleResult.Value.FirstOrDefault();
                        }

                        break;
                    case "Video":
                        var getVideoResult =
                            await this.videoService.GetAsync(
                                x => x.Id == materialId,
                                x => new VideoDTO()
                                {
                                    Id = x.Id,
                                    Title = x.Title,
                                    Description = x.Description,
                                    Type = x.Type,
                                    Length = x.Length,
                                    Quality = x.Quality
                                });

                        if (getVideoResult.IsSuccessful)
                        {
                            materialInfoDTO = getVideoResult.Value.FirstOrDefault();
                        }
                        break;
                    case "PrintedMaterial":
                        var getPrintedMaterialResult =
                            await this.printedMaterialService.GetAsync(
                                x => x.Id == materialId,
                                x => new PrintedMaterialDTO()
                                {
                                    Id = x.Id,
                                    Title = x.Title,
                                    Description = x.Description,
                                    Type = x.Type,
                                    PagesCount = x.PagesCount,
                                    AuthorNames = x.AuthorPrintedMaterials
                                        .Select(y => y.Author.Name).ToList()
                                });

                        if (getPrintedMaterialResult.IsSuccessful)
                        {
                            materialInfoDTO = getPrintedMaterialResult.Value.FirstOrDefault();
                        }
                        break;
                }
                materialInfoDTO.IsEnrolled = userEnrollment;
                materialInfoDTO.IsFinished = userMaterial == null ? false : userMaterial.IsFinished;

                return new ServiceResult<MaterialInfoDTO>(materialInfoDTO);
            }   
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult<MaterialInfoDTO>(ex);
            }
        }

        public async Task<ServiceResult<MaterialPageDTO>> GetAvailableMaterialsAsync(int courseId, PageInfo pageInfo)
        {
            try
            {
                var materialInfoDTOs =
                    await this.repository.GetPageAsync(
                        x => !x.CourseMaterials.Select(y => y.CourseId).Contains(courseId),
                        x => new MaterialInfoDTO()
                        {
                            Id = x.Id,
                            Title = x.Title,
                            Description = x.Description,
                            Type = x.Type
                        },
                        pageInfo);

                var lastPageNumber = (int)Math.Ceiling(
                    (decimal)await this.repository.Count(x => true) / pageInfo.PageSize);

                var materialPageDTO = new MaterialPageDTO()
                {
                    materialInfoDTOs = materialInfoDTOs,
                    LastPageNumber = lastPageNumber > 0 ? lastPageNumber : 1,
                    PageNumber = pageInfo.PageNumber
                };

                return new ServiceResult<MaterialPageDTO>(materialPageDTO);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult<MaterialPageDTO>(ex);
            }
        }

        public async Task<ServiceResult<MaterialPageDTO>> GetMaterialPageAsync(PageInfo pageInfo)
        {
            try
            {
                var materialInfoDTOs =
                    await this.repository.GetPageAsync(
                        x => true,
                        x => new MaterialInfoDTO()
                        {
                            Id = x.Id,
                            Title = x.Title,
                            Description = x.Description,
                            Type = x.Type
                        },
                        pageInfo);

                var lastPageNumber = (int)Math.Ceiling(
                    (decimal)await this.repository.Count(x => true) / pageInfo.PageSize);

                var materialPageDTO = new MaterialPageDTO()
                {
                    materialInfoDTOs = materialInfoDTOs,
                    LastPageNumber = lastPageNumber > 0 ? lastPageNumber : 1,
                    PageNumber = pageInfo.PageNumber
                };

                return new ServiceResult<MaterialPageDTO>(materialPageDTO);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult<MaterialPageDTO>(ex);
            }
        }

        public async Task<ServiceResult<IList<UserCourseMaterialDTO>>> GetUserCourseMaterialsAsync(int courseId, int userId)
        {
            try
            {
                var materialsForCourseServiceResult =
                    await this.courseMaterialService.GetAsync(
                        x => x.CourseId == courseId,
                        x => x.Material);

                
                if (materialsForCourseServiceResult.IsSuccessful)
                {
                    var materialsForCourse =
                        materialsForCourseServiceResult.Value;

                    var materialsForUserServiceResult =
                        await this.userMaterialsService.GetAsync(
                            x => materialsForCourse.Select(material => material.Id).Contains(x.MaterialId) && x.UserId == userId,
                            x => x);

                    if (materialsForUserServiceResult.IsSuccessful)
                    {
                        var materialsForUser = materialsForUserServiceResult.Value;
                        var userCourseMaterialDTOs = new List<UserCourseMaterialDTO>();

                        foreach (var material in materialsForCourse)
                        {
                            userCourseMaterialDTOs.Add(this.MapUserCourseMaterialDTO(material, materialsForUser.Where(x => x.MaterialId == material.Id).FirstOrDefault()));
                        }

                        return new ServiceResult<IList<UserCourseMaterialDTO>>(userCourseMaterialDTOs);

                    }
                }

                return new ServiceResult<IList<UserCourseMaterialDTO>>(
                    new Exception("Failed to find materials for course and user"));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult<IList<UserCourseMaterialDTO>>(ex);
            }
        }

        public async Task<ServiceResult<IList<MaterialInfoDTO>>> GetMaterialInfosForCourse(int courseId)
        {
            try
            {
                var materialsForCourseServiceResult =
                    await this.courseMaterialService.GetAsync(
                        x => x.CourseId == courseId,
                        x => x.Material);

                if (materialsForCourseServiceResult.IsSuccessful)
                {
                    var materialsForCourse =
                        materialsForCourseServiceResult.Value;

                    var materialInfos = new List<MaterialInfoDTO>();

                    foreach (var material in materialsForCourse)
                    {
                        materialInfos.Add(this.MapMaterialInfo(material));
                    }

                    return new ServiceResult<IList<MaterialInfoDTO>>(materialInfos);
                }

                return new ServiceResult<IList<MaterialInfoDTO>>(
                    new Exception("Failed to find materials for course"));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult<IList<MaterialInfoDTO>>(ex);
            }
        }
        
        private UserCourseMaterialDTO MapUserCourseMaterialDTO(Material material, UserMaterials userMaterials)
        {
            return new UserCourseMaterialDTO()
            {
                Id = material.Id,
                Title = material.Title,
                Description = material.Description,
                IsFinished = userMaterials == null ? false : userMaterials.IsFinished
            };
        }

        private MaterialInfoDTO MapMaterialInfo(Material material)
        {
            return new MaterialInfoDTO()
            {
                Id = material.Id,
                Title = material.Title,
                Description = material.Description
            };
        }
    }
}
