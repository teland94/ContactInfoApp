using ContactInfoApp.UI.Model;

namespace ContactInfoApp.UI.ViewModels
{
    public class GroupedPhoneNumberViewModel
    {
        public int Count { get; set; }

        public CallLogModel CallLog { get; set; }

        public bool IsModifiedByContactHistory { get; set; }
    }
}
