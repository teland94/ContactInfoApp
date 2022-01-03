using System.Threading.Tasks;
using ContactInfoApp.Shared.Models;

namespace ContactInfoApp.UI.Interfaces
{
    public interface ISettingsService
    {
        public Task<UiSettingsModel> GetUiSettingsAsync();
    }
}
