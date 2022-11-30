namespace EducationPortal.Infrastructure.Authentication
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using EducationPortal.Application.Services.DTO;
    using EducationPortal.Domain.Core.Common;

    public class MVCAuthenticationService : IAuthenticationService
    {
        public MVCAuthenticationService()
        {

        }

        public Task<IServiceResult> LoginUser(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResult> RegisterUserAsync(RegisterUserDTO registerUserDTO)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UserExist(string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
