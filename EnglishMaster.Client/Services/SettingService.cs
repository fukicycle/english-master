using Blazored.LocalStorage;
using EnglishMaster.Client.Services.Interfaces;
using EnglishMaster.Shared;
using Microsoft.AspNetCore.Authorization;

namespace EnglishMaster.Client.Services
{
    public sealed class SettingService : ISettingService
    {
        private readonly ILocalStorageService _localStorageService;
        private const string USER_SETTINGS_STORAGE_KEY = $"{ApplicationSettings.APPLICATION_ID}_SETTING";
        private readonly UserSettings _defaultSettings = new UserSettings("Samantha|en-US", 10, StudyMode.Choice);

        public SettingService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task<UserSettings> LoadAsync()
        {
            UserSettings? currentSettings = await _localStorageService.GetItemAsync<UserSettings>(USER_SETTINGS_STORAGE_KEY);
            return currentSettings ?? _defaultSettings;
        }

        public async Task ResetAsync()
        {
            await _localStorageService.SetItemAsync(USER_SETTINGS_STORAGE_KEY, _defaultSettings);
        }

        public async Task SaveAsync(UserSettings userSettings)
        {
            await _localStorageService.SetItemAsync(USER_SETTINGS_STORAGE_KEY, userSettings);
        }
    }
}
