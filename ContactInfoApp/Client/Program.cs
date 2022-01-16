using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using ContactInfoApp.UI.HttpClients;
using ContactInfoApp.UI.Interfaces;
using ContactInfoApp.UI.Services;
using CurrieTechnologies.Razor.Clipboard;
using Radzen;
using ClipboardService = ContactInfoApp.Client.Services.ClipboardService;

namespace ContactInfoApp.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            var services = builder.Services;

            var baseAddress = builder.HostEnvironment.BaseAddress;
            services.AddHttpClient<ContactHttpClient>(client =>
                client.BaseAddress = new Uri($"{baseAddress}api/contact/"));
            services.AddHttpClient<ComputerVisionHttpClient>(client =>
                client.BaseAddress = new Uri($"{baseAddress}api/computervision/"));
            services.AddHttpClient<SearchContactHistoryHttpClient>(client =>
                client.BaseAddress = new Uri($"{baseAddress}api/searchcontacthistory/"));
            services.AddHttpClient<SearchContactHistoryCommentHttpClient>(client =>
                client.BaseAddress = new Uri($"{baseAddress}api/searchcontacthistorycomment/"));
            services.AddHttpClient<SettingsHttpClient>(client =>
                client.BaseAddress = new Uri($"{baseAddress}api/settings/"));

            services.AddScoped<NotificationService>();
            services.AddScoped<DialogService>();
            services.AddBlazoredLocalStorage();
            services.AddClipboard();

            services.AddScoped<IClipboardService, ClipboardService>();

            services.AddScoped<ISettingsService, SettingsService>();

            await builder.Build().RunAsync();
        }
    }
}
