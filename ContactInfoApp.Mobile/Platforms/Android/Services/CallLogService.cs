using Android.Provider;
using ContactInfoApp.UI.Interfaces;
using Application = Android.App.Application;
using CallLogModel = ContactInfoApp.UI.Model.CallLogModel;

namespace ContactInfoApp.Mobile.Platforms.Android.Services
{
    internal class CallLogService : ICallLogService
    {
        public IEnumerable<CallLogModel> GetCallLogs()
        {
            // filter in desc order limit by no
            var querySorter = $"{CallLog.Calls.Date} desc ";

            using var phones = Application.Context.ApplicationContext.ContentResolver.Query(CallLog.Calls.ContentUri, null, null, null, querySorter);
            if (phones != null)
            {
                while (phones.MoveToNext())
                {
                    var callNumber = phones.GetString(phones.GetColumnIndex(CallLog.Calls.Number));
                    var callDuration = phones.GetString(phones.GetColumnIndex(CallLog.Calls.Duration));
                    var callDate = phones.GetLong(phones.GetColumnIndex(CallLog.Calls.Date));
                    var callName = phones.GetString(phones.GetColumnIndex(CallLog.Calls.CachedName));

                    var callTypeInt = phones.GetInt(phones.GetColumnIndex(CallLog.Calls.Type));
                    var callType = Enum.GetName(typeof(CallType), callTypeInt);

                    var log = new CallLogModel
                    {
                        CallName = callName,
                        CallNumber = callNumber,
                        CallDuration = callDuration,
                        CallDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(callDate),
                        CallType = callType
                    };

                    yield return log;
                }

                phones.Close();
            }
        }
    }
}
