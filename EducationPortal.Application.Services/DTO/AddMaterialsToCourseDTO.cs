namespace EducationPortal.Application.Services.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class AddMaterialsToCourseDTO
    {
        public int CourseId { get; set; }

        public List<int> MaterialIds { get; set; }
    }
}
