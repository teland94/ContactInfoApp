using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using ContactInfoApp.Client.HttpClients;
using CurrieTechnologies.Razor.Clipboard;
using Radzen;

namespace ContactInfoApp.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            var services = builder.Services;

            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            services.AddHttpClient<ContactHttpClient>(client =>
                client.BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}api/contact/"));
            services.AddHttpClient<ComputerVisionHttpClient>(client =>
                client.BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}api/computervision/"));

            services.AddScoped<NotificationService>();
            services.AddScoped<DialogService>();
            services.AddBlazoredLocalStorage();
            services.AddClipboard();

            await builder.Build().RunAsync();
        }
    }
}
