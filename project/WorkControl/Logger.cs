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
using NDde.Client;

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
        /// <summary>
        /// Number of mouse actions (buttons down and wheel move)
        /// </summary>
        int mouseActionsCount;

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
                log.AddRangeOfItems(items);
                //log.AddRangeOfItems(new List<LogItem>(from c in lines select LogItem.FromCSVRow(c)));

            }
            _keysProc = HookCallback;
            _proc = MouseHookCallback;


        }

        ~Logger()
        {
            if(_keysHookID != IntPtr.Zero)
                UnhookWindowsHookEx(_keysHookID);
            if(_mouseHookID != IntPtr.Zero)
                UnhookWindowsHookEx(_mouseHookID);
            //sw.Close();
        }
        /// <summary>
        /// Init the logger to be ready for logging
        /// </summary>
        public void Init()
        {
            _keysHookID = SetHook(_keysProc);
            _mouseHookID = SetHook(_proc);
            
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
                keypressCount,mouseActionsCount);
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
            mouseActionsCount = 0;
        }

        private void TryToGetExtraInfo(LogItem item)
        {
            if (item.activeWindowProcessName == "chrome")
            {
                item.PutExtraInfo(GetChromeUrl());
            }
            if (item.activeWindowProcessName == "firefox")
            {
                item.PutExtraInfo(GetFirefoxUrl());
            }
            if (item.activeWindowProcessName == "opera")
            {
                item.PutExtraInfo(GetOperaUrl());
            }
        }

        public static string GetChromeUrl()
        {

            AutomationElement element = AutomationElement.FromHandle(GetForegroundWindow());
            if (element == null)
                return null;

            AutomationElementCollection edits5 = element.FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
            if (edits5.Count == 0)
                return "null";
            AutomationElement edit = edits5[0];
            string vp = ((ValuePattern)edit.GetCurrentPattern(ValuePattern.Pattern)).Current.Value;
            //Console.WriteLine(vp);
            return vp;
        }

        public static string GetFirefoxUrl()
        {
            return GetBrowserURL("firefox");
        }

        public static string GetOperaUrl()
        {
            return null;
            //return GetBrowserURL("opera");
        }

        static string GetBrowserURL(string browser)
        {
            try
            {
                DdeClient dde = new DdeClient(browser, "WWW_GetWindowInfo");
                dde.Connect();
                string url = dde.Request("URL", int.MaxValue);
                string[] text = url.Split(new string[] { "\",\"" }, StringSplitOptions.RemoveEmptyEntries);
                dde.Disconnect();
                string res = text[0].Substring(1);
                Console.WriteLine(res);
                return res;
            }
            catch
            {
                return null;
            }
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

        private  IntPtr _keysHookID = IntPtr.Zero;
        private LowLevelKeyboardProc _keysProc;
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

            return CallNextHookEx(_keysHookID, nCode, wParam, lParam);

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




        private LowLevelMouseProc _proc;

        private IntPtr _mouseHookID = IntPtr.Zero;


        private IntPtr SetHook(LowLevelMouseProc proc)

        {

            using (Process curProcess = Process.GetCurrentProcess())

            using (ProcessModule curModule = curProcess.MainModule)

            {

                return SetWindowsHookEx(WH_MOUSE_LL, proc,

                    GetModuleHandle(curModule.ModuleName), 0);

            }

        }


        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);


        private IntPtr MouseHookCallback(

            int nCode, IntPtr wParam, IntPtr lParam)

        {

            //if (nCode >= 0 &&

            //    MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam)

            //{

            //    MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));

            //    Console.WriteLine(hookStruct.pt.x + ", " + hookStruct.pt.y);

            //}
            if (nCode >= 0)
            {
                //check the action
                if (MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam || MouseMessages.WM_RBUTTONDOWN == (MouseMessages)wParam || MouseMessages.WM_MOUSEWHEEL == (MouseMessages)wParam)
                {
                    mouseActionsCount++;
                    //Console.WriteLine("mouse action!");
                }
            }

            return CallNextHookEx(_mouseHookID, nCode, wParam, lParam);

        }


        private const int WH_MOUSE_LL = 14;


        private enum MouseMessages

        {

            WM_LBUTTONDOWN = 0x0201,

            WM_LBUTTONUP = 0x0202,

            WM_MOUSEMOVE = 0x0200,

            WM_MOUSEWHEEL = 0x020A,

            WM_RBUTTONDOWN = 0x0204,

            WM_RBUTTONUP = 0x0205

        }




        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern IntPtr SetWindowsHookEx(int idHook,

            LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

    }




}
