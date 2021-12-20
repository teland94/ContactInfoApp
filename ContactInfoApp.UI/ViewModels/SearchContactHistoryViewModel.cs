using System;
using System.Collections.Generic;

namespace ContactInfoApp.UI.ViewModels
{
    public class SearchContactHistoryViewModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string IpAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string DisplayName { get; set; }

        public bool IsSpam { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public int? TagCount { get; set; }

        public int DuplicateSequenceNumber { get; set; }
    }
}
