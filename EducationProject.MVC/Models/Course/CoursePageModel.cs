using EducationPortal.Application.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationProject.MVC.Models.Course
{
    public class CoursePageModel
    {
        public IList<CourseInfoDTO> CourseInfoDTOs { get; set; }

        public int PageNumber { get; set; }

        public int LastPageNumber { get; set; }
    }
}
