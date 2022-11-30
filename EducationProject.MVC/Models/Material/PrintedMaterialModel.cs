using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationProject.MVC.Models.Material
{
    public class PrintedMaterialModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int PagesCount { get; set; }

        public string AuthorName { get; set; }
    }
}
