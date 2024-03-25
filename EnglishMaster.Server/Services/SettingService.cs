using EnglishMaster.Server.Services.Interfaces;

namespace EnglishMaster.Server.Services
{
    public sealed class SettingService : ISettingService
    {
        public bool IsMute { get; set; }
        public int NumberOfQuestion { get; set; }
        public string? JWTToken { get; set; }

        public void Reset()
        {
            IsMute = false;
            NumberOfQuestion = 0;
        }
    }
}
