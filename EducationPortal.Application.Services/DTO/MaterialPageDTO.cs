using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Application.Services.DTO
{
    public class MaterialPageDTO
    {
        public IList<MaterialInfoDTO> materialInfoDTOs { get; set; }

        public int PageNumber { get; set; }

        public int LastPageNumber { get; set; }
    }
}
