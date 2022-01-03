using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        [Inject] private ISettingsService SettingsService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] private DialogService DialogService { get; set; }

        private IEnumerable<SearchContactHistoryViewModel> _searchContactHistoryRawViewItems;
        private IEnumerable<SearchContactHistoryViewModel> _searchContactHistoryItems;

        private bool _searchContactHistoryPageNavigationEnabled;

        private string _searchQuery;
        private DataGridSelectionMode _selectionMode = DataGridSelectionMode.Single;
        private object _selectedSearchContactHistoryItems;

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
                TagCount = sch.TagCount
            }).ToList();

            CalculateSearchContactHistoryItemsProperties();

            _searchContactHistoryItems = _searchContactHistoryRawViewItems;
        }

        private void RowRender(RowRenderEventArgs<SearchContactHistoryViewModel> args)
        {
            args.Expandable = args.Data.Tags != null && args.Data.Tags.Any();
        }

        private void SearchChange(string value)
        {
            _searchQuery = value;
            CheckSearchContactHistoryFilter();
        }

        private void ToggleSelectionModeClick(MouseEventArgs e)
        {
            _selectionMode = _selectionMode == DataGridSelectionMode.Single ? DataGridSelectionMode.Multiple : DataGridSelectionMode.Single;
            if (_selectedSearchContactHistoryItems == null) { return; }
            _selectedSearchContactHistoryItems = _selectionMode == DataGridSelectionMode.Multiple ? new[] { (SearchContactHistoryViewModel)_selectedSearchContactHistoryItems } : null;
        }

        private async Task DeleteSearchContactItemClick(MouseEventArgs e)
        {
            if (_selectedSearchContactHistoryItems == null) { return; }

            var selectedContactHistoryItems = GetSelectedItems(_selectedSearchContactHistoryItems);
            string confirmMessage;
            if (selectedContactHistoryItems.Count == 1)
            {
                var selectedContactHistoryItem = selectedContactHistoryItems.First();
                confirmMessage = $"Вы действительно хотите удалить запись: {selectedContactHistoryItem.DisplayName} - {selectedContactHistoryItem.PhoneNumber}?";
            }
            else
            {
                confirmMessage = "Вы действительно хотите удалить выбранные записи?";
            }

            var confirmResult = await DialogService.Confirm(confirmMessage, "Удаление", new ConfirmOptions { OkButtonText = "Да", CancelButtonText = "Нет" });
            if (confirmResult.HasValue && confirmResult.Value)
            {
                foreach (var selectedContactHistoryItem in selectedContactHistoryItems)
                {
                    await SearchContactHistoryHttpClient.DeleteAsync(selectedContactHistoryItem.Id);
                }
                _searchContactHistoryRawViewItems = _searchContactHistoryRawViewItems.Where(schr => selectedContactHistoryItems.All(sch => sch.Id != schr.Id));
                CalculateSearchContactHistoryItemsProperties();
                CheckSearchContactHistoryFilter();
                ClearSelection();
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
            _selectedSearchContactHistoryItems = null;
            _radzenGrid.SelectRow(null);
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
