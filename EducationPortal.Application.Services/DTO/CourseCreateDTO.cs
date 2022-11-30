namespace EducationPortal.Application.Services.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class CourseCreateDTO
    {
        public int UserOwnerId { get; set; }

        public string CourseTitle { get; set; }

        public string CourseDescription { get; set; }

        public List<int> SkillIds { get; set; }

        public List<int> CourseMaterialIds { get; set; }
    }
}
