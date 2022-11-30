namespace EducationPortal.Application.Services.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class MaterialInfoDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public bool IsFinished { get; set; }

        public bool IsEnrolled { get; set; }
    }
}
