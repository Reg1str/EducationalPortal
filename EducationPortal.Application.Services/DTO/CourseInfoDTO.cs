namespace EducationPortal.Application.Services.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class CourseInfoDTO
    {
        public int Id { get; set; }

        public string CourseOwnerEmail { get; set; }

        public int CourseOwnerId { get; set; }

        public string CourseTitle { get; set; }

        public string CourseDescription { get; set; }
    }
}
