using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Application.Services.DTO
{
    public class CourseInfoPagedMaterialsDTO
    {
        public int Id { get; set; }

        public string CourseOwnerEmail { get; set; }

        public string CourseTitle { get; set; }

        public string CourseDescription { get; set; }

        public List<UserCourseMaterialDTO> UserCourseMaterialDTOs { get; set; }

        public List<SkillInfoDTO> SkillInfoDTOs { get; set; }

        public int LastPageNumber { get; set; }

        public int PageNumber { get; set; }

        public bool IsOwner { get; set; }

        public bool IsEnrolled { get; set; }
    }
}
