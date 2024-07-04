namespace EnglishMaster.Client.Services.Interfaces
{
    public interface ISettingService
    {
        Task ResetAsync();
        Task SaveAsync(UserSettings userSettings);
        Task<UserSettings> LoadAsync();
    }
}
