using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkControl.Reports
{
    class ProcessesReport
    {
        private Dictionary<string, int> times;

        public IReadOnlyDictionary<string, int> Times
        {
            get { return times; }
        }
        private ProcessesReport(Dictionary<string, int> times)
        {
            this.times = times;
        }

        public static ProcessesReport GenerateReport(List<LogItem> log, bool onlyAcitve = false)
        {
            Dictionary<string,int> dic = new Dictionary<string, int>();
            foreach (var logItem in log)
            {
                if (onlyAcitve && !logItem.IsActive())
                {
                    continue;
                }
                if (!dic.ContainsKey(logItem.activeWindowProcessName))
                {
                    dic[logItem.activeWindowProcessName] = 0;
                }
                dic[logItem.activeWindowProcessName]++;
            }
            return new ProcessesReport(dic);
        }

    }
}
