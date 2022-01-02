using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using ContactInfoApp.Shared.Models;
using ContactInfoApp.UI.Dialogs;
using ContactInfoApp.UI.Exceptions;
using ContactInfoApp.UI.HttpClients;
using ContactInfoApp.UI.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace ContactInfoApp.UI.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject] private NotificationService NotificationService { get; set; }
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private ILocalStorageService LocalStorageService { get; set; }
        [Inject] private IClipboardService ClipboardService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] private ContactHttpClient ContactHttpClient { get; set; }

        private readonly Regex _phoneRegex = new("\\+?\\d{10,11}", RegexOptions.Compiled);
        private readonly Regex _phoneReplaceRegex = new(@"[\s\-]", RegexOptions.Compiled);

        private string _phoneNumber;
        private ContactModel _contact;
        private IEnumerable<string> _tags;

        private RadzenTextBox _textBox;

        private bool _isLoading;

        [Parameter]
        public string PhoneNumber { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(PhoneNumber) && _phoneRegex.IsMatch(PhoneNumber))
            {
                _phoneNumber = PhoneNumber;
                await Process();
            }
            else
            {
                _phoneNumber = await LocalStorageService.GetItemAsync<string>("phoneNumber");
            }
        }

        private async Task PasteClick(MouseEventArgs e)
        {
            if (await ClipboardService.IsSupportedAsync())
            {
                var text = await ClipboardService.ReadTextAsync();
                _phoneNumber = _phoneReplaceRegex.Replace(text, "");
            }
        }

        private async Task SearchClick(MouseEventArgs e)
        {
            await Process();
        }

        private async Task CopyLinkClick(MouseEventArgs e)
        {
            var baseUri = NavigationManager.BaseUri;
            await ClipboardService.WriteTextAsync($"{baseUri}home/{_phoneNumber.Replace("+", "")}");
            NotificationService.Notify(NotificationSeverity.Info, "Ссылка успешно скопирована");
        }

        private async Task Process()
        {
            var trimmedPhoneNumber = _phoneReplaceRegex.Replace(_phoneNumber, "");
            _tags = Enumerable.Empty<string>();
            var contactId = await ProcessSearch(trimmedPhoneNumber);
            if (contactId != null && !_contact.LimitedResult && _contact.TagCount > 0)
            {
                await ProcessNumberDetail(trimmedPhoneNumber);
            }
        }

        private async Task<int?> ProcessSearch(string phoneNumber)
        {
            try
            {
                _isLoading = true;

                _contact = await ContactHttpClient.SearchContactAsync(phoneNumber);

                await LocalStorageService.SetItemAsync("phoneNumber", phoneNumber);

                return _contact.Id;
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
                }
                if (ex.StatusCode == HttpStatusCode.Forbidden)
                {
                    var isVerifiedCode = await VerifyCode(errorResult.Image);
                    if (isVerifiedCode)
                    {
                        await ProcessSearch(phoneNumber);
                    }
                }

                return null;
            }
            finally
            {
                _isLoading = false;
            }
        }

        private async Task ProcessNumberDetail(string phoneNumber)
        {
            try
            {
                _isLoading = true;

                var numberDetail = await ContactHttpClient.GetNumberDetailAsync(phoneNumber, _contact.Id);
                _tags = numberDetail.Tags;
            }
            catch (ContactRequestException ex)
            {
                var errorResult = ex.ErrorResult;
                if (errorResult == null)
                {
                    var errorMessage = ex.StatusCode switch
                    {
                        HttpStatusCode.NotFound => "Теги не найдены",
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
                        await ProcessNumberDetail(phoneNumber);
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
