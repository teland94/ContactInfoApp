using System.Collections.Generic;

namespace ContactInfoApp.Shared.Request
{
    public class ContactHistoryPhoneNumberInfoRequestModel
    {
        public IEnumerable<string> PhoneNumbers { get; set; }
    }
}
