using EnglishMaster.Client.Services.Interfaces;

namespace EnglishMaster.Client.Services
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
