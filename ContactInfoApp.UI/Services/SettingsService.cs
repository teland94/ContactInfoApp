using System;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using ContactInfoApp.Shared.Models;
using ContactInfoApp.UI.HttpClients;
using ContactInfoApp.UI.Interfaces;

namespace ContactInfoApp.UI.Services
{
    public class SettingsService : ISettingsService
    {
        private const string UiSettingsKey = "uiSettings";

        public SettingsHttpClient SettingsHttpClient { get; }

        public ILocalStorageService LocalStorageService { get; }

        public SettingsService(SettingsHttpClient settingsHttpClient,
            ILocalStorageService localStorageService)
        {
            SettingsHttpClient = settingsHttpClient;
            LocalStorageService = localStorageService;
        }

        public async Task<UiSettingsModel> GetUiSettingsAsync()
        {
            if (await LocalStorageService.ContainKeyAsync(UiSettingsKey))
            {
                return await LocalStorageService.GetItemAsync<UiSettingsModel>(UiSettingsKey);
            }

            var uiSettings = await SettingsHttpClient.GetUiSettingsAsync();

            await LocalStorageService.SetItemAsync(UiSettingsKey, uiSettings);

            return uiSettings;
        }
    }
}
