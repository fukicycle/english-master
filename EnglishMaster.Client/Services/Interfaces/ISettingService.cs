namespace EnglishMaster.Client.Services.Interfaces
{
    public interface ISettingService
    {
        void Reset();

        bool IsMute { get; set; }
        int NumberOfQuestion { get; set; }
        string? JWTToken { get; set; }
    }
}
