using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restauracja.Utilities
{
    public class EventLogger : LogBase
    {
        public override void Log(Exception exception)
        {
            EventLog eventLog = new EventLog("RestauracjaLogs");
            eventLog.Source = "Restauracja";
            var logInfo = BuildLogs(exception);
            eventLog.WriteEntry(logInfo, EventLogEntryType.Error);
        }
    }
}
