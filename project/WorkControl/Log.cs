using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using WorkControl.Reports;

namespace WorkControl
{
    /// <summary>
    /// Log realization
    /// </summary>
    class Log
    {
        /// <summary>
        /// List of all log items
        /// </summary>
        private List<LogItem> log;

        public Log()
        {
            log = new List<LogItem>();
        }
        /// <summary>
        /// Add one item to log
        /// </summary>
        /// <param name="item"></param>
        public void PutItem(LogItem item)
        {
            log.Add(item);
        }
        /// <summary>
        /// Add range of items to the log (for loading history)
        /// </summary>
        /// <param name="items"></param>
        public void AddRangeOfItems(List<LogItem> items)
        {
            if (log.Count>0)
            {
                throw new Exception("Log should be empty to do this");
            }
            log.AddRange(items);
        }
        /// <summary>
        /// Get number of seconds, spent to Work
        /// </summary>
        /// <returns></returns>
        public int GetWorkedSeconds()
        {
            /*string[] goodPrograms = new[] {"devenv"};
            //string[] badPrograms = new[] {"telegram"};

            int counter = log.Count(item => goodPrograms.Contains(item.activeWindowProcessName));*/
            //int counter = 0;
            //return counter;
            return log.Sum(e => e.IsActive() ? 1 : 0);
        }
        /// <summary>
        /// Get number of handled seconds
        /// </summary>
        /// <returns></returns>
        public int GetTotalSeconds()
        {
            return log.Count; //each item is a second
        }

        public bool IsActiveLast()
        {
            return log.Last().IsActive();
        }

        public Reports.ActivityReport GetActivityReport()
        {
            return ActivityReport.GenerateReport(log);
        }

        public Reports.ProcessesReport GetProcessesReport(bool onlyActive = false)
        {
            return ProcessesReport.GenerateReport(log,onlyActive);
        }

        public Reports.SitesReport GetSitesReport(bool onlyActive = false)
        {
            return SitesReport.GenerateReport(log, onlyActive);
        }

        public Reports.WorkReport GetWorkReport()
        {
            return WorkReport.GenerateReport(log);
        }
    }
}
