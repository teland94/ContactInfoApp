using System.Collections.Generic;

namespace ContactInfoApp.Shared
{
    public class Contact
    {
        public string DisplayName { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public int? TagCount { get; set; }
    }
}
