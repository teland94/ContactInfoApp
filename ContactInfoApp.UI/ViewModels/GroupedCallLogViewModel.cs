using System;
using System.Collections.Generic;

namespace ContactInfoApp.UI.ViewModels
{
    public class GroupedCallLogViewModel
    {
        public DateTime Date { get; set; }

        public IEnumerable<GroupedPhoneNumberViewModel> CallLogs { get; set; }
    }
}
