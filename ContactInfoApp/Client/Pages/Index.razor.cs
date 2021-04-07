using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using ContactInfoApp.Client.Dialogs;
using ContactInfoApp.Client.Exceptions;
using ContactInfoApp.Client.HttpClients;
using ContactInfoApp.Shared;
using CurrieTechnologies.Razor.Clipboard;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace ContactInfoApp.Client.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject] private NotificationService NotificationService { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private ILocalStorageService LocalStorageService { get; set; }
        [Inject] private ClipboardService ClipboardService { get; set; }

        [Inject] private ContactHttpClient ContactHttpClient { get; set; }

        private readonly Regex _phoneRegex = new("\\+?\\d{10,11}", RegexOptions.Compiled);

        private string _phoneNumber;
        private Contact _contact;

        private RadzenTextBox _textBox;

        private bool _isLoading;

        protected override async Task OnInitializedAsync()
        {
            _phoneNumber = await LocalStorageService.GetItemAsync<string>("phoneNumber");
        }

        private async Task PasteClick(MouseEventArgs e)
        {
            if (await ClipboardService.IsSupportedAsync())
            {
                _phoneNumber = await ClipboardService.ReadTextAsync();
            }
        }

        private async Task SearchClick(MouseEventArgs e)
        {
            var trimmedPhoneNumber = Regex.Replace(_phoneNumber, @"\s+", "");
            await ProcessGetContact(trimmedPhoneNumber);
        }

        private async Task ProcessGetContact(string phoneNumber)
        {
            try
            {
                _isLoading = true;
                _contact = await ContactHttpClient.GetContactAsync(phoneNumber);
                await LocalStorageService.SetItemAsync("phoneNumber", phoneNumber);
            }
            catch (ContactRequestException ex)
            {
                var errorResult = ex.ErrorResult;
                if (errorResult == null)
                {
                    var errorMessage = ex.StatusCode switch
                    {
                        HttpStatusCode.NotFound => "Контакт не найден",
                        _ => ex.Message
                    };
                    NotificationService.Notify(NotificationSeverity.Error, errorMessage, duration: 5000);
                    return;
                }
                if (ex.StatusCode == HttpStatusCode.Forbidden)
                {
                    var isVerifiedCode = await VerifyCode(errorResult.Image);
                    if (isVerifiedCode)
                    {
                        await ProcessGetContact(phoneNumber);
                    }
                }
            }
            finally
            {
                _isLoading = false;
            }
        }

        private async Task<bool> VerifyCode(string image)
        {
            if (string.IsNullOrEmpty(image)) { return false; }
            var dialogResult = await DialogService.OpenAsync<VerificationCodeDialog>("Верификация",
                new Dictionary<string, object>
                {
                    { "image", image }
                }, new DialogOptions
                {
                    Width = "300px"
                });
            if (dialogResult != null)
            {
                try
                {
                    return await ContactHttpClient.VerifyCodeAsync(dialogResult);
                }
                catch (ContactRequestException e)
                {
                    return await VerifyCode(e.ErrorResult.Image);
                }
            }
            return false;
        }
    }
}
