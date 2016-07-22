using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkControl
{
    /// <summary>
    /// One log item for given time
    /// </summary>
    class LogItem
    {
        public int time;
        public string ActiveWindowTitle;
        public string ActiveWindowProcessName;
        public Point cursorPos;
        public int keypressCount;
        public string extraInfo;
        /// <summary>
        /// Link to the previous element, if exitst
        /// </summary>
        public LogItem previus;

        public LogItem(int time, string activeWindowTitle, string activeWindowProcessName, Point cursorPos, int keypressCount)
        {
            this.time = time;
            ActiveWindowTitle = activeWindowTitle.Replace(";","").Replace("\r","").Replace("\n","");
            ActiveWindowProcessName = activeWindowProcessName;
            this.cursorPos = cursorPos;
            this.keypressCount = keypressCount;
            this.extraInfo = "";
        }

        public void PutExtraInfo(string info)
        {
            this.extraInfo = info.Replace(";", "").Replace("\r", "").Replace("\n", "");
        }

        public override string ToString()
        {
            return extraInfo; //keypressCount.ToString(); //cursorPos.ToString();
            //return $"Time: {UnixTimestamp.ConvertToDatetime(time).ToString("T")}, ActiveWindowTitle: {ActiveWindowTitle}, ActiveWindowProcessName: {ActiveWindowProcessName}";
        }

        public string ToCSVRow()
        {
            return $"{time};{ActiveWindowTitle};{ActiveWindowProcessName};{cursorPos.X};{cursorPos.Y};{keypressCount};{extraInfo}";
        }

        public static LogItem FromCSVRow(string row)
        {
            string[] arr = row.Split(';');
            var item = new LogItem(int.Parse(arr[0]),arr[1],arr[2],new Point(int.Parse(arr[3]), int.Parse(arr[4])),int.Parse(arr[5]));
            if (arr.Length > 6)
            {
                item.PutExtraInfo(arr[6]);
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
    }
}
