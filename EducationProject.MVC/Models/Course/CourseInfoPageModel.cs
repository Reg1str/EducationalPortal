using EducationPortal.Application.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationProject.MVC.Models.Course
{
    public class CourseInfoPageModel
    {
        public string CourseOwnerEmail { get; set; }

        public string CourseOwnerFullName { get; set; }

        public string CourseTitle { get; set; }

        public string CourseDescription { get; set; }

        public List<MaterialInfoDTO> MaterialInfos { get; set; }

        public List<string> SkillNames { get; set; }

        public int PageNumber { get; set; }

        public int LastPageNumber { get; set; }
    }
}
