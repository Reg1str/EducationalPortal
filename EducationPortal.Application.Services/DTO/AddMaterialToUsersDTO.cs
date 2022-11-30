using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Application.Services.DTO
{
    public class AddMaterialToUsersDTO
    {
        public int MaterialId { get; set; }

        public List<int> UserIds { get; set; }
    }
}
