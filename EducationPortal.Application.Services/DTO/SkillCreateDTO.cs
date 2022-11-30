using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Application.Services.DTO
{
    public class SkillCreateDTO
    {
        public string Name { get; set; }

        public int Penalty { get; set; }

        public int MaxLevel { get; set; }
    }
}
