using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactInfoApp.UI.Dialogs;
using ContactInfoApp.UI.HttpClients;
using ContactInfoApp.UI.Interfaces;
using ContactInfoApp.UI.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace ContactInfoApp.UI.Pages
{
    public partial class SearchContactHistory : ComponentBase
    {
        [Inject] private SearchContactHistoryHttpClient SearchContactHistoryHttpClient { get; set; }
        [Inject] private SearchContactHistoryCommentHttpClient SearchContactHistoryCommentHttpClient { get; set; }

        [Inject] private ISettingsService SettingsService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] private DialogService DialogService { get; set; }

        private IEnumerable<SearchContactHistoryViewModel> _searchContactHistoryRawViewItems;
        private IEnumerable<SearchContactHistoryViewModel> _searchContactHistoryItems;

        private bool _searchContactHistoryPageNavigationEnabled;

        private string _searchQuery;
        private DataGridSelectionMode _selectionMode = DataGridSelectionMode.Single;
        private object _selectedSearchContactHistoryItems;
        private IList<SearchContactHistoryViewModel> _selectedSearchContactHistoryItemsList;

        private RadzenTextBox _textBox;
        private RadzenGrid<SearchContactHistoryViewModel> _radzenGrid;

        protected override async Task OnParametersSetAsync()
        {
            var uiSettings = await SettingsService.GetUiSettingsAsync();
            _searchContactHistoryPageNavigationEnabled = uiSettings.SearchContactHistoryPageNavigationEnabled;
            if (!uiSettings.SearchContactHistoryPageNavigationEnabled)
            {
                NavigationManager.NavigateTo("/");
            }
        }

        protected override async Task OnInitializedAsync()
        { 
            var searchContactHistoryRawItems = await SearchContactHistoryHttpClient.GetAsync();

            _searchContactHistoryRawViewItems = searchContactHistoryRawItems.Select(sch => new SearchContactHistoryViewModel
            {
                Id = sch.Id,
                Date = sch.Date,
                IpAddress = sch.IpAddress,
                PhoneNumber = sch.PhoneNumber,
                DisplayName = sch.DisplayName,
                IsSpam = sch.IsSpam,
                Tags = sch.Tags,
                TagCount = sch.TagCount,
                HasComments = sch.HasComments
            }).ToList();

            CalculateSearchContactHistoryItemsProperties();

            _searchContactHistoryItems = _searchContactHistoryRawViewItems;
        }

        private void RowRender(RowRenderEventArgs<SearchContactHistoryViewModel> args)
        {
            args.Expandable = args.Data.Tags != null && args.Data.Tags.Any();
        }

        private void ValueChanged(object searchContactHistoryItems)
        {
            _selectedSearchContactHistoryItemsList = GetSelectedItems(searchContactHistoryItems);
            _selectedSearchContactHistoryItems = searchContactHistoryItems;
        }

        private void SearchChange(string value)
        {
            _searchQuery = value;
            CheckSearchContactHistoryFilter();
        }

        private void ToggleSelectionModeClick(MouseEventArgs e)
        {
            _selectionMode = _selectionMode == DataGridSelectionMode.Single ? DataGridSelectionMode.Multiple : DataGridSelectionMode.Single;
            if (_selectedSearchContactHistoryItemsList == null) { return; }
            if (_selectionMode == DataGridSelectionMode.Multiple)
            {
                _selectedSearchContactHistoryItems = _selectedSearchContactHistoryItemsList.FirstOrDefault();
            }
            else
            {
                ClearSelection();
            }
        }

        private async Task DeleteSearchContactItemClick(MouseEventArgs e)
        {
            if (_selectedSearchContactHistoryItemsList == null) { return; }

            string confirmMessage;
            if (_selectedSearchContactHistoryItemsList.Count == 1)
            {
                var selectedContactHistoryItem = _selectedSearchContactHistoryItemsList.First();
                confirmMessage = $"Вы действительно хотите удалить запись: {selectedContactHistoryItem.DisplayName} - {selectedContactHistoryItem.PhoneNumber}?";
            }
            else
            {
                confirmMessage = "Вы действительно хотите удалить выбранные записи?";
            }

            var confirmResult = await DialogService.Confirm(confirmMessage, "Удаление", new ConfirmOptions { OkButtonText = "Да", CancelButtonText = "Нет" });
            if (confirmResult.HasValue && confirmResult.Value)
            {
                foreach (var selectedContactHistoryItem in _selectedSearchContactHistoryItemsList)
                {
                    await SearchContactHistoryHttpClient.DeleteAsync(selectedContactHistoryItem.Id);
                }
                _searchContactHistoryRawViewItems = _searchContactHistoryRawViewItems.Where(schr => _selectedSearchContactHistoryItemsList.All(sch => sch.Id != schr.Id)).ToList();
                CalculateSearchContactHistoryItemsProperties();
                CheckSearchContactHistoryFilter();
                ClearSelection();
            }
        }

        private async Task ShowSearchContactItemCommentsClick(MouseEventArgs e)
        {
            var contactHistoryItem = _selectedSearchContactHistoryItemsList?.SingleOrDefault();
            if (contactHistoryItem != null)
            {
                var contactHistoryCommentItems = await SearchContactHistoryCommentHttpClient.GetBySearchContactHistoryId(contactHistoryItem.Id);
                var comments = contactHistoryCommentItems.Select(c => new CommentViewModel
                {
                    Author = c.Author,
                    AuthorImage = c.AuthorImage,
                    Body = c.Body,
                    Liked = c.Liked,
                    Disliked = c.Disliked,
                    Date = c.Date,
                });
                await DialogService.OpenAsync<ContactCommentsDialog>("Комментарии", new Dictionary<string, object>
                {
                    { "Comments", comments }
                });
            }
        }

        private void CalculateSearchContactHistoryItemsProperties()
        {
            foreach (var searchContactHistoryItemsGroup in _searchContactHistoryRawViewItems.GroupBy(x => x.PhoneNumber))
            {
                var counter = 0;
                foreach (var searchContactHistoryItem in searchContactHistoryItemsGroup.OrderBy(x => x.Date))
                {
                    searchContactHistoryItem.DuplicateSequenceNumber = counter++;
                }
            }
        }

        private void CheckSearchContactHistoryFilter()
        {
            if (string.IsNullOrEmpty(_searchQuery))
            {
                _searchContactHistoryItems = _searchContactHistoryRawViewItems;
                return;
            }
            _searchContactHistoryItems = _searchContactHistoryRawViewItems
                .Where(sch =>
                    sch.PhoneNumber.Contains(_searchQuery, StringComparison.OrdinalIgnoreCase)
                    || sch.DisplayName.Contains(_searchQuery, StringComparison.OrdinalIgnoreCase)
                    || sch.Tags != null && sch.Tags.Any(t => t.Contains(_searchQuery, StringComparison.OrdinalIgnoreCase)));
        }

        private void ClearSelection()
        {
            _selectedSearchContactHistoryItemsList = null;
            _selectedSearchContactHistoryItems = null;
        }

        private static IList<SearchContactHistoryViewModel> GetSelectedItems(object selectedSearchContactHistoryItems)
        {
            var selectedContactHistoryItems = new List<SearchContactHistoryViewModel>();
            switch (selectedSearchContactHistoryItems)
            {
                case SearchContactHistoryViewModel searchContactHistoryItem:
                {
                    selectedContactHistoryItems.Add(searchContactHistoryItem);
                    break;
                }
                case IEnumerable<SearchContactHistoryViewModel> searchContactHistoryItems:
                {
                    selectedContactHistoryItems.AddRange(searchContactHistoryItems);
                    break;
                }
            }
            return selectedContactHistoryItems;
        }
    }
}
