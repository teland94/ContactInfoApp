using Android.Webkit;
using Blazored.LocalStorage;
using ContactInfoApp.Mobile.Platforms.Android.Services;
using ContactInfoApp.UI.HttpClients;
using ContactInfoApp.UI.Interfaces;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Radzen;
using ClipboardService = ContactInfoApp.Mobile.Services.ClipboardService;

namespace ContactInfoApp.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .RegisterBlazorMauiWebView()
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });
            builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<IBlazorWebView, MauiBlazorWebViewHandler>();
            });

            var services = builder.Services;

            builder.Services.AddBlazorWebView();

            const string baseAddress = "https://getcontact-info.azurewebsites.net/";
            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });
            services.AddHttpClient<ContactHttpClient>(client =>
                client.BaseAddress = new Uri($"{baseAddress}api/contact/"));
            services.AddHttpClient<ComputerVisionHttpClient>(client =>
                client.BaseAddress = new Uri($"{baseAddress}api/computervision/"));
            services.AddHttpClient<SearchContactHistoryHttpClient>(client =>
                client.BaseAddress = new Uri($"{baseAddress}api/searchcontacthistory/"));
            services.AddHttpClient<SettingsHttpClient>(client =>
                client.BaseAddress = new Uri($"{baseAddress}api/settings/"));

            services.AddScoped<NotificationService>();
            services.AddScoped<DialogService>();
            services.AddBlazoredLocalStorage();

            services.AddScoped<IClipboardService, ClipboardService>();
            services.AddScoped<ICallLogService, CallLogService>();

            return builder.Build();
        }
    }

    public class MauiWebChromeClient : WebChromeClient
    {
        public override void OnPermissionRequest(PermissionRequest request)
        {
            request.Grant(request.GetResources());
        }
    }

    public class MauiBlazorWebViewHandler : BlazorWebViewHandler
    {
        protected override WebChromeClient GetWebChromeClient()
        {
            return new MauiWebChromeClient();
        }
    }
}