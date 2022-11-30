using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationProject.MVC.Models.Material
{
    public class ArticleModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime PublishedDate { get; set; }

        public string SourceUrl { get; set; }
    }
}
