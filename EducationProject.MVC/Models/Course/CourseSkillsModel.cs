using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationProject.MVC.Models.Course
{
    public class CourseSkillsModel
    {
        public int CourseId { get; set; }

        public List<int> SkillIds { get; set; }
    }
}
