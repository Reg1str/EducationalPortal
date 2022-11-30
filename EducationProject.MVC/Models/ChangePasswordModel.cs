using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationProject.MVC.Models
{
    public class ChangePasswordModel
    {
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
