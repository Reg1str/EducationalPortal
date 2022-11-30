using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Application.Services.DTO
{
    public class ChangePasswordDTO
    {
        public int UserId { get; set; }

        public string NewPassword { get; set; }
    }
}
