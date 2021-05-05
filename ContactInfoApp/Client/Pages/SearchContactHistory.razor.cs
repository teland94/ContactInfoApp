using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactInfoApp.Client.HttpClients;
using ContactInfoApp.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace ContactInfoApp.Client.Pages
{
    public partial class SearchContactHistory : ComponentBase
    {
        [Inject] private SearchContactHistoryHttpClient SearchContactHistoryHttpClient { get; set; }
        [Inject] private SettingsHttpClient SettingsHttpClient { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] private DialogService DialogService { get; set; }

        private IEnumerable<SearchContactHistoryModel> _searchContactHistoryRawItems;
        private IEnumerable<SearchContactHistoryModel> _searchContactHistoryItems;
        private bool _searchContactHistoryPageNavigationEnabled;

        private string _searchQuery;
        private DataGridSelectionMode _selectionMode = DataGridSelectionMode.Single;
        private object _selectedSearchContactHistoryItems;

        private RadzenTextBox _textBox;
        private RadzenGrid<SearchContactHistoryModel> _radzenGrid;

        protected override async Task OnParametersSetAsync()
        {
            var uiSettings = await SettingsHttpClient.GetUiSettingsAsync();
            _searchContactHistoryPageNavigationEnabled = uiSettings.SearchContactHistoryPageNavigationEnabled;
            if (!uiSettings.SearchContactHistoryPageNavigationEnabled)
            {
                NavigationManager.NavigateTo("/");
            }
        }

        protected override async Task OnInitializedAsync()
        {
            _searchContactHistoryRawItems = await SearchContactHistoryHttpClient.GetAsync();
            _searchContactHistoryItems = _searchContactHistoryRawItems;
        }

        private void RowRender(RowRenderEventArgs<SearchContactHistoryModel> args)
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
            _selectedSearchContactHistoryItems = _selectionMode == DataGridSelectionMode.Multiple ? new[] { (SearchContactHistoryModel)_selectedSearchContactHistoryItems } : null;
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
                _searchContactHistoryRawItems = _searchContactHistoryRawItems.Where(schr => selectedContactHistoryItems.All(sch => sch.Id != schr.Id));
                CheckSearchContactHistoryFilter();
                ClearSelection();
            }
        }

        private void CheckSearchContactHistoryFilter()
        {
            if (string.IsNullOrEmpty(_searchQuery))
            {
                _searchContactHistoryItems = _searchContactHistoryRawItems;
                return;
            }
            _searchContactHistoryItems = _searchContactHistoryRawItems
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

        private static IList<SearchContactHistoryModel> GetSelectedItems(object selectedSearchContactHistoryItems)
        {
            var selectedContactHistoryItems = new List<SearchContactHistoryModel>();
            switch (selectedSearchContactHistoryItems)
            {
                case SearchContactHistoryModel searchContactHistoryItem:
                {
                    selectedContactHistoryItems.Add(searchContactHistoryItem);
                    break;
                }
                case IEnumerable<SearchContactHistoryModel> searchContactHistoryItems:
                {
                    selectedContactHistoryItems.AddRange(searchContactHistoryItems);
                    break;
                }
            }
            return selectedContactHistoryItems;
        }
    }
}
