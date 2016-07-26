using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkControl.Reports;

namespace WorkControl
{
    /// <summary>
    /// One log item for given time
    /// </summary>
    class LogItem
    {
        /// <summary>
        /// Unix timestamp of item captured
        /// </summary>
        public int time;
        /// <summary>
        /// Title of active window
        /// </summary>
        public string activeWindowTitle;
        /// <summary>
        /// Name of process-owner of active window
        /// </summary>
        public string activeWindowProcessName;
        /// <summary>
        /// Mouse position
        /// </summary>
        public Point cursorPos;
        /// <summary>
        /// Count of pressed keys on keyboard
        /// </summary>
        public int keypressCount;
        /// <summary>
        /// Count of actions by mouse (clicks, wheel moves)
        /// </summary>
        public int mouseActionsCount;
        /// <summary>
        /// Extra info (depends of process, e.g. for chrome this is current URL)
        /// </summary>
        public string extraInfo;
        /// <summary>
        /// Link to the previous element, if exitst
        /// </summary>
        public LogItem previus;
        /// <summary>
        /// Is this process browser (extraInfo would be url)
        /// </summary>
        public bool IsBrowserProcess
        {
            get { return Settings.Self.ScoreLists.IsBrowserProcess(activeWindowProcessName); }
        }

        public LogItem(int time, string activeWindowTitle, string activeWindowProcessName, Point cursorPos, int keypressCount, int mouseActionsCount)
        {
            this.time = time;
            if (activeWindowTitle == null)
                activeWindowTitle = "null";
            this.activeWindowTitle = activeWindowTitle.Replace(";","").Replace("\r","").Replace("\n","");
            this.activeWindowProcessName = activeWindowProcessName;
            this.cursorPos = cursorPos;
            this.keypressCount = keypressCount;
            this.extraInfo = "";
            this.mouseActionsCount = mouseActionsCount;
        }
        /// <summary>
        /// Add extra info
        /// </summary>
        /// <param name="info"></param>
        public void PutExtraInfo(string info)
        {
            if (info==null)
            {
                info = "null";
            }
            this.extraInfo = info.Replace(";", "").Replace("\r", "").Replace("\n", "");
        }

        public override string ToString()
        {
            return extraInfo; //keypressCount.ToString(); //cursorPos.ToString();
            //return $"Time: {UnixTimestamp.ConvertToDatetime(time).ToString("T")}, activeWindowTitle: {activeWindowTitle}, activeWindowProcessName: {activeWindowProcessName}";
        }
        /// <summary>
        /// Convert item to csv row
        /// </summary>
        /// <returns></returns>
        public string ToCSVRow()
        {
            return $"{time};{activeWindowTitle};{activeWindowProcessName};{cursorPos.X};{cursorPos.Y};{keypressCount};{mouseActionsCount};{extraInfo}";
        }
        /// <summary>
        /// Create new item from csv row (factory)
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static LogItem FromCSVRow(string row)
        {
            string[] arr = row.Split(';');
            var item = new LogItem(int.Parse(arr[0]),arr[1],arr[2],new Point(int.Parse(arr[3]), int.Parse(arr[4])),int.Parse(arr[5]), int.Parse(arr[6]));
            if (arr.Length > 7)
            {
                item.PutExtraInfo(arr[7]);
            }
            return item;
        }

        /// <summary>
        /// Get specified number of previous elements, if exists
        /// </summary>
        /// <param name="num">Number of elements</param>
        /// <returns></returns>
        public IEnumerable<LogItem> GetPreviousElements(int num)
        {
            if(num <= 0)
                yield break;
            if(previus == null)
                yield break;
            yield return previus;
            foreach (var previousElement in previus.GetPreviousElements(num-1))
            {
                yield return previousElement;
            }
        }
        /// <summary>
        /// Is item active, or computer stays alone
        /// </summary>
        /// <returns></returns>
        public bool IsActive()
        {
            const int LAST_NUM = 20;
            const float MIN_ACTIVITY_SCORE = 1f;

            const float KEYPRESS_DIVIDER = 5;
            const float MOUSE_ACTION_DIVIDER = 5;
            const float MOUSEDIF_DIVIDER = 300;
            //const int MIN_KEYS = 2;
            //const int MIN_MOUSEDIF = 50;
            List<LogItem> prev = new List<LogItem>(GetPreviousElements(LAST_NUM));
            int keysPressed = prev.Sum(e => e.keypressCount);
            int mouseActions = prev.Sum(e => e.mouseActionsCount);
            Point mousepos = cursorPos;
            int mouseDif = 0;
            foreach (var previousElement in prev)
            {
                mouseDif += Math.Abs(mousepos.X - previousElement.cursorPos.X) +
                            Math.Abs(mousepos.Y - previousElement.cursorPos.Y);
                mousepos = previousElement.cursorPos;
            }
            //float keypress_score = keysPressed/KEYPRESS_DIVIDER;
            float score = Math.Min(keysPressed / KEYPRESS_DIVIDER, 1) + Math.Min(mouseActions / MOUSE_ACTION_DIVIDER, 1) +
                          Math.Min(mouseDif / MOUSEDIF_DIVIDER, 1);
            return score >= MIN_ACTIVITY_SCORE;
        }

        public bool IsWorking()
        {
            bool active = IsActive();
            if (!active)
                return false;                                               //if user is not active, he is not working
            var ptype = GetProcesScoreType();


            return false;

        }
        public Settings.Lists.ScoreType GetProcesScoreType()
        {
            return Settings.Self.ScoreLists.GetProceScoreType(activeWindowProcessName);
        }

        public Settings.Lists.ScoreType GetSiteScoreType()
        {
            if(IsBrowserProcess)
                return Settings.Self.ScoreLists.GetSiteScoreType(GetSiteHost());
            throw new Exception("cannot get site from not-browser process");
        }

        public string GetSiteHost()
        {
            if (!IsBrowserProcess)
                throw new Exception("cannot get site from non-briwser process");
            string sname = extraInfo;
            if (!sname.StartsWith("http://") && !sname.StartsWith("https://"))
            {
                sname = "http://" + sname;
            }
            if (Uri.IsWellFormedUriString(sname, UriKind.Absolute))
            {
                Uri uri = new Uri(sname);
                return uri.Host;
            }
            return null;

        }

        public int GetWorkedPrice()
        {
            return WorkReport.GetWorkedPrice(this);
        }
    }
}
