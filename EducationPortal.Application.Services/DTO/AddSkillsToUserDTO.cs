namespace EducationPortal.Application.Services.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class AddSkillsToUserDTO
    {
        public int UserId { get; set; }

        public List<int> SkillIds { get; set; }
    }
}
