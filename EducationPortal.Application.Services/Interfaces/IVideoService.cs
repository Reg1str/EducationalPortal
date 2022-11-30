namespace EducationPortal.Application.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using EducationPortal.Application.Services.DTO;
    using EducationPortal.Domain.Core.Common;
    using EducationPortal.Domain.Core.Entities;

    public interface IVideoService : IGenericService<Video>
    {
        Task<ServiceResult> CreateVideoAsync(VideoDTO videoDTO);
    }
}
