using System;

namespace ContactInfoApp.Server.Persistence.Entities
{
    public class SearchContactHistory
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string IpAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string DisplayName { get; set; }

        public bool IsSpam { get; set; }

        public string Tags { get; set; }

        public int? TagCount { get; set; }
    }
}
