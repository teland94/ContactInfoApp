using System.Collections.Generic;
using ContactInfoApp.UI.Model;

namespace ContactInfoApp.UI.Interfaces
{
    public interface ICallLogService
    {
        public IEnumerable<CallLogModel> GetCallLogs();
    }
}
