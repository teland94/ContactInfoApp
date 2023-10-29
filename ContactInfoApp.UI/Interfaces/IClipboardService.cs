using System.Threading.Tasks;

namespace ContactInfoApp.UI.Interfaces;

public interface IClipboardService
{
    Task<bool> IsSupportedAsync();

    Task<string> ReadTextAsync();

    Task WriteTextAsync(string text);
}