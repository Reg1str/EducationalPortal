namespace EducationPortal.Application.Services.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class AddSkillsToCourseDTO
    {
        public int CourseId { get; set; }

        public List<int> SkillIds { get; set; }
    }
}
