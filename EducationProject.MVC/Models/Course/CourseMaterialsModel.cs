using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationProject.MVC.Models.Course
{
    public class CourseMaterialsModel
    {
        public int CourseId { get; set; }

        public List<int> MaterialIds { get; set; }
    }
}
