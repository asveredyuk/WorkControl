using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime;
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

        public bool IsActiveLast()
        {
            const int LAST_NUM = 5;
            const int MIN_KEYS = 2;
            const int MIN_MOUSEDIF = 50;
            var item = log.Last();
            int keysPressed = item.GetPreviousElements(LAST_NUM).Sum(e => e.keypressCount);
            if (keysPressed >= MIN_KEYS)
            {
                return true;
            }
            Point mousepos = item.cursorPos;
            int mouseDif = 0;
            foreach (var previousElement in item.GetPreviousElements(LAST_NUM))
            {
                mouseDif += Math.Abs(mousepos.X - previousElement.cursorPos.X) +
                            Math.Abs(mousepos.Y - previousElement.cursorPos.Y);
                mousepos = previousElement.cursorPos;
            }
            if (mouseDif >= MIN_MOUSEDIF)
            {
                return true;
            }
            return false;

        }
    }
}
