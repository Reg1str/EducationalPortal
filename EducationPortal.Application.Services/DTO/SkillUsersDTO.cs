namespace EducationPortal.Application.Services.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class SkillUsersDTO
    {
        public int SkillId { get; set; }

        public List<int> UserIds { get; set; }
    }
}
