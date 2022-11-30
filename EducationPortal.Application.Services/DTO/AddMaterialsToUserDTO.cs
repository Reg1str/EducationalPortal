using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Application.Services.DTO
{
    public class AddMaterialsToUserDTO
    {
        public int UserId { get; set; }

        public List<int> MaterialIds { get; set; }
    }
}
