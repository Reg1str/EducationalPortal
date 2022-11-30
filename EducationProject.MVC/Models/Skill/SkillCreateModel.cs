using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationProject.MVC.Models.Skill
{
    public class SkillCreateModel
    {
        public string Name { get; set; }

        public int Penalty { get; set; }

        public int MaxLevel { get; set; }
    }
}
