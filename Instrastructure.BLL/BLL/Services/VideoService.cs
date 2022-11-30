using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EducationPortal.Application.Services.DTO;
using EducationPortal.Application.Services.Interfaces;
using EducationPortal.Domain.Core.Common;
using EducationPortal.Domain.Core.Entities;
using EducationPortal.Domain.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EducationPortal.Infrastructure.BLL.Services
{
    public class VideoService : GenericService<Video>, IVideoService
    {
        public VideoService(IRepository<Video> videoRepository, ILogger<VideoService> logger)
            : base(videoRepository, logger)
        {
        }

        public async Task<ServiceResult> CreateVideoAsync(VideoDTO videoDTO)
        {
            try
            {
                var videoEntity = new Video()
                {
                    Title = videoDTO.Title,
                    Description = videoDTO.Description,
                    Length = videoDTO.Length,
                    Quality = videoDTO.Quality,
                    Type = videoDTO.Type
                };

                await this.repository.CreateAsync(videoEntity);
                await this.SaveChangesAsync();
                return new ServiceResult();
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new ServiceResult(ex);
            }
        }
    }
}
