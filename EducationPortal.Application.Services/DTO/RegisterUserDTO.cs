namespace EducationPortal.Application.Services.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class RegisterUserDTO
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
