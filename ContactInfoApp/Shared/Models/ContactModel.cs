using System;
using System.Collections.Generic;

namespace ContactInfoApp.Shared.Models
{
    public class ContactModel
    {
        public int Id { get; set; }

        public string PhoneNumber { get; set; }

        public string DisplayName { get; set; }

        public bool IsSpam { get; set; }

        public bool LimitedResult { get; set; }

        public int? TagCount { get; set; }

        public int CommentCount { get; set; }
    }
}
