using EnglishMaster.Client.Entities;

namespace EnglishMaster.Client.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> IsAuthenticatedAsync();

        Task<LoginUser?> GetLoginUserAsync();
    }
}
