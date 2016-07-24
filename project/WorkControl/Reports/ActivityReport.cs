using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkControl.Reports
{
    class ActivityReport
    {
        public readonly int ActiveSeconds;
        public readonly int NonActiveSeconds;

        public int TotalSeconds
        {
            get { return ActiveSeconds + NonActiveSeconds; }
        }

        private ActivityReport(int activeSeconds, int nonActiveSeconds)
        {
            ActiveSeconds = activeSeconds;
            NonActiveSeconds = nonActiveSeconds;
        }

        public static ActivityReport GenerateReport(List<LogItem> log)
        {
            int active = log.Sum(e => e.IsActive() ? 1 : 0); ;
            int nonactive = log.Count-active;
            return new ActivityReport(active,nonactive);
            
        }
    }
}
