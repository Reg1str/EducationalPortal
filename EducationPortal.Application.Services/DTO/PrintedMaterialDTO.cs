using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Application.Services.DTO
{
    public class PrintedMaterialDTO : MaterialInfoDTO
    {
        public int PagesCount { get; set; }

        public List<string> AuthorNames { get; set; }
    }
}
