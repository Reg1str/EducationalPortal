using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Application.Services.DTO
{
    public class ArticleDTO : MaterialInfoDTO
    {
        public DateTime PublishedDate { get; set; }

        public string SourceUrl { get; set; }
    }
}
