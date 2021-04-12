using System.Collections.Generic;

namespace ContactInfoApp.Shared.Models
{
    public class ContactModel
    {
        public string PhoneNumber { get; set; }

        public string DisplayName { get; set; }

        public bool IsSpam { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public int? TagCount { get; set; }
    }
}
