using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationProject.MVC.Models.Course
{
    public class CourseCreateModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public List<int> SkillIds { get; set; }

        public List<int> MaterialIds { get; set; }
    }
}
