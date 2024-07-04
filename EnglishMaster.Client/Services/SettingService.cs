using Blazored.LocalStorage;
using EnglishMaster.Client.Services.Interfaces;
using EnglishMaster.Shared;
using Microsoft.AspNetCore.Authorization;

namespace EnglishMaster.Client.Services
{
    public sealed class SettingService : ISettingService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly SpeakService _speakService;
        private const string USER_SETTINGS_STORAGE_KEY = $"{ApplicationSettings.APPLICATION_ID}_SETTING";
        private readonly UserSettings _defaultSettings = new UserSettings("", 10, StudyMode.Choice);

        public SettingService(ILocalStorageService localStorageService, SpeakService speakService)
        {
            _localStorageService = localStorageService;
            _speakService = speakService;
        }

        public async Task<UserSettings> LoadAsync()
        {
            UserSettings? currentSettings = await _localStorageService.GetItemAsync<UserSettings>(USER_SETTINGS_STORAGE_KEY);
            if (currentSettings == null)
            {
                var voices = await _speakService.GetVoicesAsync();
                _defaultSettings.VoiceIdentity = voices.First().VoiceIdentity;
            }
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
