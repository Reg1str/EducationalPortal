using EducationPortal.Application.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationProject.MVC.Models.Skill
{
    public class SkillPageModel
    {
        public IList<SkillInfoDTO> SkillInfoDTOs { get; set; }

        public int PageNumber { get; set; }

        public int LastPageNumber { get; set; }
    }
}
