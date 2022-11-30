using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Application.Services.DTO
{
    public class SkillPageDTO
    {
        public IList<SkillInfoDTO> SkillInfoDTOs { get; set; }

        public int PageNumber { get; set; }

        public int LastPageNumber { get; set; }
    }
}
