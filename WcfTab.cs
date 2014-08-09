using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.Wcf
{
    public class WcfTab : ITab, ITabSetup
    {
        public static volatile int MaxDetailedLogs = 1000;

        public string Name { get { return "WCF"; } }

        public RuntimeEvent ExecuteOn { get { return RuntimeEvent.EndRequest; } }

        public Type RequestContextType { get { return null; } }

        public object GetData(ITabContext context)
        {
            var logEntries = context.GetMessages<WcfEntry>(); 
            if (logEntries == null)
                return null;

            // Evaluate any expression trees
            logEntries = logEntries.ToArray();

            var data = new List<object[]> {new object[] {"Timestamp", "Elapsed", "Action", "Request", "Response"}};
            data.AddRange(logEntries.Select(log => new object[] {
                                    log.StartTime,
                                    log.Duration.Milliseconds, 
                                    log.EventName,
                                    log.RequestBody,
                                    log.ResponseBody
                                }));
            return data;
        }

        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<WcfEntry>();
        }

        public string DocumentationUri
        {
            get { return "http://github.com/stweb/glimpse.wcf"; }
        }
    }
}