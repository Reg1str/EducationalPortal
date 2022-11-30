using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Application.Services.DTO
{
    public class SkillInfoDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int UserProgress { get; set; }

        public int LevelThreshold { get; set; }
    }
}
