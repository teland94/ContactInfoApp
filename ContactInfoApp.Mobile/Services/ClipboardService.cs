using ContactInfoApp.UI.Interfaces;

namespace ContactInfoApp.Mobile.Services
{
    public class ClipboardService : IClipboardService
    {
        public Task<bool> IsSupportedAsync()
        {
            return Task.FromResult(true);
        }

        public Task<string> ReadTextAsync()
        {
            return Clipboard.GetTextAsync();
        }

        public Task WriteTextAsync(string text)
        {
            return Clipboard.SetTextAsync(text);
        }
    }
}
