using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Application.Services.DTO
{
    public class CourseInfoPageDTO
    {
        public IList<CourseInfoDTO> CourseInfoDTOs { get; set; }

        public int LastPageNumber { get; set; }
    }
}
