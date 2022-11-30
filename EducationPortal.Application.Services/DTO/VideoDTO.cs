using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Application.Services.DTO
{
    public class VideoDTO : MaterialInfoDTO
    {
        public int Length { get; set; }

        public int Quality { get; set; }
    }
}
