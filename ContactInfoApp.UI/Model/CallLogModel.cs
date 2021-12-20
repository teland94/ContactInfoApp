using System;
using System.Globalization;

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

        public long CallDateTick { get; set; }

        public DateTime CallDate =>
            new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(this.CallDateTick);

        public string CallType { get; set; }

        public string CallTitle => $"{CallNumber} - {CallName}";

        public string CallDescription =>
            $"{CallDate.ToString("g", CultureInfo.CreateSpecificCulture("en-us"))} - {CallType} | Duration: {CallDurationFormat}";
    }
}
    
