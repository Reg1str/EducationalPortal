namespace EducationPortal.Infrastructure.Authentication
{
    using System.Threading.Tasks;
    using EducationPortal.Application.Services.DTO;

    public interface IAuthenticationService
    {
        bool UserExist(string email, string password);

        void LoginUser(string email, string password);

        Task RegisterUserAsync(RegisterUserDTO registerUserDTO);
    }
}
