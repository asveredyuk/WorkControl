using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkControl.Reports
{
    class SitesReport
    {
        /// <summary>
        /// Times for sites (collapsed for known domains)
        /// </summary>
        private Dictionary<string, int> times;
        /// <summary>
        /// Times for full domains
        /// </summary>
        private Dictionary<string, int> fullTimes;

        public IReadOnlyDictionary<string, int> Times => times;
        public IReadOnlyDictionary<string, int> FullTimes => fullTimes;

        private SitesReport(Dictionary<string, int> times, Dictionary<string, int> fullTimes)
        {
            this.times = times;
            this.fullTimes = fullTimes;
        }

        public static SitesReport GenerateReport(List<LogItem> items, bool onlyActive = false)
        {
            Dictionary<string, int> times = new Dictionary<string, int>();
            Dictionary<string, int> fullTimes = new Dictionary<string, int>();
            foreach (var item in items)
            {
                if(!item.IsBrowserProcess)
                    continue;
                if(onlyActive && !item.IsActive())
                    continue;
                string host = item.GetSiteHost();
                if(host == null)
                    continue;                                           //this site is not valid
                string smarthost = Settings.Self.ScoreLists.GetKnownHostName(host);
                if (smarthost != null)
                {
                    if (!times.ContainsKey(smarthost))
                        times[smarthost] = 0;
                    times[smarthost]++;
                }
                else
                {
                    if (!times.ContainsKey(host))
                        times[host] = 0;
                    times[host]++;
                }
                if (!fullTimes.ContainsKey(host))
                    fullTimes[host] = 0;
                fullTimes[host]++;
            }
            return new SitesReport(times,fullTimes);
        }
    }
}
