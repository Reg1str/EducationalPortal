namespace EducationPortal.Infrastructure.Authentication
{
    using EducationPortal.Domain.Core.Entities;

    public interface ISessionHandler
    {
        void CreateUserSession(UserSession userAccount);

        void CloseSession();

        UserSession GetCurrentSession();
    }
}
