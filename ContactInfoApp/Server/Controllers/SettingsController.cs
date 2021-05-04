using Microsoft.AspNetCore.Mvc;
using ContactInfoApp.Server.Configuration;
using ContactInfoApp.Shared.Models;
using Microsoft.Extensions.Options;

namespace ContactInfoApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private UiSettings UiSettings { get; }

        public SettingsController(IOptions<UiSettings> uiSettingsAccessor)
        {
            UiSettings = uiSettingsAccessor.Value;
        }

        [HttpGet(nameof(GetUiSettings))]
        public ActionResult<UiSettingsModel> GetUiSettings()
        {
            return new UiSettingsModel
            {
                SearchContactHistoryPageMenuVisible = UiSettings.SearchContactHistoryPageMenuVisible,
                SearchContactHistoryPageNavigationEnabled = UiSettings.SearchContactHistoryPageNavigationEnabled
            };
        }
    }
}
