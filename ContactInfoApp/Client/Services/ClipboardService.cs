using System;
using System.Threading.Tasks;
using ContactInfoApp.UI.Interfaces;

namespace ContactInfoApp.Client.Services
{
    public class ClipboardService : IClipboardService
    {
        private readonly CurrieTechnologies.Razor.Clipboard.ClipboardService _clipboardService;

        public ClipboardService(CurrieTechnologies.Razor.Clipboard.ClipboardService clipboardService)
        {
            _clipboardService = clipboardService;
        }

        public Task<bool> IsSupportedAsync()
        {
            return _clipboardService.IsSupportedAsync();
        }

        public Task<string> ReadTextAsync()
        {
            return _clipboardService.ReadTextAsync();
        }

        public Task WriteTextAsync(string text)
        {
            return _clipboardService.WriteTextAsync(text);
        }
    }
}
