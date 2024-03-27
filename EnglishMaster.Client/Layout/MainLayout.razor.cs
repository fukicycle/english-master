using Microsoft.AspNetCore.Components.Authorization;

namespace EnglishMaster.Client.Layout
{
    public partial class MainLayout
    {
        private string GetProfilePicture(AuthenticationState context)
        {
            return context.User.Claims.FirstOrDefault(a => a.Type == "picture")?.Value ?? "";
        }
    }
}