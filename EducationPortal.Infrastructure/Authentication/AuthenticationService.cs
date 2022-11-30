namespace EducationPortal.Infrastructure.Authentication
{
    using System.Threading.Tasks;
    using EducationPortal.Application.Services.DTO;
    using EducationPortal.Application.Services.Interfaces;
    using EducationPortal.Domain.Core.Entities;

    public class AuthenticationService : IAuthenticationService
    {
        private IUserService userService;
        private ISessionHandler sessionHandler;

        public AuthenticationService(IUserService userService, ISessionHandler sessionHandler)
        {
            this.userService = userService;
            this.sessionHandler = sessionHandler;
        }

        public bool UserExist(string email, string password)
        {
            return this.userService.GetUserBySessionAsync(new UserSession() { Email = email, Password = password }) != null;
        }

        public void LoginUser(string email, string password)
        {
            // TODO: Add hasher;
            var userAccount = new UserSession()
            {
                Email = email,
                Password = password
            };
            this.sessionHandler.CreateUserSession(userAccount);
        }

        public async Task RegisterUserAsync(RegisterUserDTO registerUserDTO)
        {
            // TODO: Add hasher;
            var user = new User()
            {
                Email = registerUserDTO.Email,
                Firstname = registerUserDTO.Firstname,
                Lastname = registerUserDTO.Lastname,
                Password = registerUserDTO.Password
            };

            await this.userService.CreateAsync(user);
        }
    }
}
