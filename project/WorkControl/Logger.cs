using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;

namespace WorkControl
{
    /// <summary>
    /// Class, which hanles everything happening in system
    /// </summary>
    class Logger
    {
        //singletone
        private static Logger self;

        public static Logger Self
        {
            get
            {
                if (self == null)
                {
                    self = new Logger();
                }
                return self;
            }
        }
        /// <summary>
        /// Path to file with logs
        /// </summary>
        const string LOG_FNAME = "log.csv";
        /// <summary>
        /// List of all log events
        /// </summary>
        public Log log;

        private StreamWriter sw;
        /// <summary>
        /// Number of pressed keys after previous tick
        /// </summary>
        int keypressCount;

        private Logger()
        {
            log = new Log();
            if (File.Exists(LOG_FNAME))
            {

                string[] lines = File.ReadAllLines(LOG_FNAME);
                List<LogItem> items = new List<LogItem>();
                
                foreach (var line in lines)
                {
                    LogItem item = LogItem.FromCSVRow(line);
                    if (prev != null && ((item.time - prev.time) < 3))
                    {
                        item.previus = prev;
                    }
                    items.Add(item);
                    prev = item;
                }
                //log.AddRangeOfItems(new List<LogItem>(from c in lines select LogItem.FromCSVRow(c)));

            }
            _proc = HookCallback;
            
            
        }

        ~Logger()
        {
            if(_hookID != IntPtr.Zero)
                UnhookWindowsHookEx(_hookID);
            //sw.Close();
        }
        /// <summary>
        /// Init the logger to be ready for logging
        /// </summary>
        public void Init()
        {
            _hookID = SetHook(_proc);
            sw = new StreamWriter(LOG_FNAME, true, Encoding.Default);
        }
        /// <summary>
        /// Last added item
        /// </summary>
        LogItem prev;
        /// <summary>
        /// Log state for now
        /// </summary>
        public void LogNow()
        {
            string title = GetActiveWindowTitle();
            string pname = GetActiveWindowProcessName();
            Point cursorPos = GetMousePosition();
            LogItem item = new LogItem(UnixTimestamp.GetFromDatatime(DateTime.Now), title, pname, cursorPos,
                keypressCount);
            if (prev != null && ((item.time - prev.time) < 3))
            {
                item.previus = prev;
            }
            prev = item;
            TryToGetExtraInfo(item);
            log.PutItem(item);
            sw.WriteLine(item.ToCSVRow());
            sw.Flush();
            keypressCount = 0;
        }

        private void TryToGetExtraInfo(LogItem item)
        {
            if (item.ActiveWindowProcessName == "chrome")
            {
                item.PutExtraInfo(GetChromeUrl());
            }
        }

        public static string GetChromeUrl()
        {

            AutomationElement element = AutomationElement.FromHandle(GetForegroundWindow());
            if (element == null)
                return null;

            AutomationElementCollection edits5 = element.FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
            AutomationElement edit = edits5[0];
            string vp = ((ValuePattern)edit.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
            //Console.WriteLine(vp);
            return vp;
        }
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        static extern int GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);


        private uint GetActiveWindowPid()
        {
            IntPtr handle = GetForegroundWindow();
            uint pid;
            GetWindowThreadProcessId(handle, out pid);
            return pid;

        }

        private string GetActiveWindowProcessName()
        {
            uint pid = GetActiveWindowPid();
            Process p = Process.GetProcessById((int)pid);
            return p.ProcessName;
        }

        private Point GetMousePosition()
        {
            return Cursor.Position;
        }
        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, buff, nChars) > 0)
            {
                return buff.ToString();
            }
            return null;
        }


        private const int WH_KEYBOARD_LL = 13;

        private const int WM_KEYDOWN = 0x0100;

        private  IntPtr _hookID = IntPtr.Zero;
        private LowLevelKeyboardProc _proc;
        private  IntPtr SetHook(LowLevelKeyboardProc proc)

        {

            using (Process curProcess = Process.GetCurrentProcess())

            using (ProcessModule curModule = curProcess.MainModule)

            {

                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,

                    GetModuleHandle(curModule.ModuleName), 0);

            }

        }


        private delegate IntPtr LowLevelKeyboardProc(

            int nCode, IntPtr wParam, IntPtr lParam);


        private  IntPtr HookCallback(

            int nCode, IntPtr wParam, IntPtr lParam)

        {

            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)

            {

                int vkCode = Marshal.ReadInt32(lParam);

                keypressCount++;

            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);

        }


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern IntPtr SetWindowsHookEx(int idHook,

            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        [return: MarshalAs(UnmanagedType.Bool)]

        private static extern bool UnhookWindowsHookEx(IntPtr hhk);


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,

            IntPtr wParam, IntPtr lParam);


        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
