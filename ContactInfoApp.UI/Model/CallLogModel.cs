using System;

namespace ContactInfoApp.UI.Model
{
    public class CallLogModel
    {
        public string CallName { get; set; }

        public string CallNumber { get; set; }

        public string CallDuration { get; set; }

        public string CallDurationFormat
        {
            get
            {
                var intDuration = Convert.ToInt32(CallDuration);
                var time = TimeSpan.FromSeconds(intDuration);

                //here backslash is must to tell that colon is
                //not the part of format, it just a character that we want in output
                return time.ToString(@"hh\:mm\:ss\:fff");
            }
        }

        public DateTime CallDate { get; set; }

        public string CallType { get; set; }
    }
}
    
