using EnglishMaster.Client.Entities;
using Microsoft.AspNetCore.Components.Authorization;

namespace EnglishMaster.Client.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> IsAuthenticatedAsync();

        Task<LoginUser?> GetLoginUserAsync();

        Task<string?> GetIconUrlAsync();

        Task<string?> GetFirstNameAsync();

        Task<string?> GetLastNameAsync();

        Task<string?> GetEmailAsync();

        Task<string?> GetSubAsPasswordAsync();
    }
}
