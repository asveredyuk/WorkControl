using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkControl
{
    class Log : ILog
    {
        private List<LogItem> log;

        public Log()
        {
            log = new List<LogItem>();
        }
        public void PutItem(LogItem item)
        {
            log.Add(item);
        }

        public void AddRangeOfItems(List<LogItem> items)
        {
            if (log.Count>0)
            {
                throw new Exception("Log should be empty to do this");
            }
            log.AddRange(items);
        }

        public int GetWorkedSeconds()
        {
            string[] goodPrograms = new[] {"devenv"};
            //string[] badPrograms = new[] {"telegram"};

            int counter = log.Count(item => goodPrograms.Contains(item.ActiveWindowProcessName));
            return counter;
        }

        public int GetTotalSeconds()
        {
            return log.Count; //each item is a second
        }
    }
}
