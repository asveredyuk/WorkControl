using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkControl
{
    public partial class Form1 : Form
    {
        Logger logger;
        public Form1()
        {
            InitializeComponent();
            logger = Logger.Self;
            logger.Init();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        int nonworkCounter;
        const int NONWORK_NOTIFY_INTERVAL = 60*5; //each 5 minutes
        private void timer1_Tick(object sender, EventArgs e)
        {
            logger.LogNow();
            /*if (logger.log.IsActiveLast())
            {
                BackColor = Color.Green;
                //button1.Text = "active";
            }
            else
            {
                BackColor = Color.Red;
                //button1.Text = "not active";
            }*/
            int workstate = logger.log.GetWorkedPriceLast();
            if (workstate > 1)
                nonworkCounter = 0;
            else
            {
                nonworkCounter++;
                if (nonworkCounter%NONWORK_NOTIFY_INTERVAL == 0)
                {
                    notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
                    notifyIcon.BalloonTipTitle = "You are not working";
                    notifyIcon.BalloonTipText = $"You were not working for last {nonworkCounter/60} minutes";
                    notifyIcon.ShowBalloonTip(1000);
                    Console.WriteLine("notyfied");
                }
            }
            switch (workstate)
            {
                case 0:
                    BackColor = Color.Red;
                    notifyIcon.Icon = WorkControl.Properties.Resources.red;
                    break;
                case 1:
                    BackColor = Color.Yellow;
                    notifyIcon.Icon = WorkControl.Properties.Resources.yellow;
                    break;
                case 2:
                    BackColor = Color.Green;
                    notifyIcon.Icon = WorkControl.Properties.Resources.green;
                    break;
            }
            
            //LogItem last = logger.log.Last();
          //  textBox1.AppendText(last.ToString() + "\r\n");
        }

        private void LogLine(string str)
        {
            textBox1.AppendText(str + "\r\n");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            new ListsSettingsForm(ListsSettingsForm.Mode.Processes).Show();
            return;
            textBox1.Clear();
            //DateTime time = new DateTime();
            //time = time.AddSeconds(logger.log.GetWorkedSeconds());
            //button1.Text = time.ToString("T");
            /*var rep = logger.log.GetActivityReport();
            textBox1.AppendText($"active time {UnixTimestamp.ConvertIntervalToDateTime(rep.ActiveSeconds).ToLongTimeString()}\r\n");
            textBox1.AppendText($"non active time {UnixTimestamp.ConvertIntervalToDateTime(rep.NonActiveSeconds).ToLongTimeString()}\r\n");
            textBox1.AppendText($"total time {UnixTimestamp.ConvertIntervalToDateTime(rep.TotalSeconds).ToLongTimeString()}\r\n");
            textBox1.AppendText($"active percentage {rep.ActiveSeconds*100/rep.TotalSeconds}%\r\n");*/

            //var rep = logger.log.GetProcessesReport();
            var rep = logger.log.GetSitesReport();
            //var re = from c in rep.Times
            //    orderby c.Value
            //    select $"{c.Key}\t-\t{UnixTimestamp.ConvertIntervalToDateTime(c.Value).ToLongTimeString()}";
            const int LIMIT = 5;
            int counter = 0;
            int timeCounter = 0;
            foreach (var pair in from c in rep.Times orderby c.Value descending select c)
            {
                string type = "unknown";
                switch (Settings.Self.ScoreLists.GetSiteScoreType(pair.Key))
                {
                    case Settings.Lists.ScoreType.Nonwork:
                        type = "bad";
                        break;
                    case Settings.Lists.ScoreType.Neutral:
                        type = "neutral";
                        break;
                    case Settings.Lists.ScoreType.Work:
                        type = "good";
                        break;
                }
                LogLine($"{pair.Key}({type})\t-\t{UnixTimestamp.ConvertIntervalToDateTime(pair.Value).ToLongTimeString()}");
                counter++;
                timeCounter += pair.Value;
                if (counter >= LIMIT)
                {
                    break;
                }
            }
            if (counter >= LIMIT)
            {
                // we have more
                int totalTime = rep.Times.Sum(p => p.Value);
                LogLine($"other\t-\t{UnixTimestamp.ConvertIntervalToDateTime(totalTime - timeCounter).ToLongTimeString()}");
            }
        }

        const int LINES_LIMIT = 10;
        private void btSitesReport_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            var rep = logger.log.GetSitesReport();
            //var re = from c in rep.Times
            //    orderby c.Value
            //    select $"{c.Key}\t-\t{UnixTimestamp.ConvertIntervalToDateTime(c.Value).ToLongTimeString()}";
            int maxlen = rep.Times.Select(c => c.Key.Length).Max();

            int counter = 0;
            int timeCounter = 0;
            foreach (var pair in from c in rep.Times orderby c.Value descending select c)
            {
                string type = "unknown";
                switch (Settings.Self.ScoreLists.GetSiteScoreType(pair.Key))
                {
                    case Settings.Lists.ScoreType.Nonwork:
                        type = "bad";
                        break;
                    case Settings.Lists.ScoreType.Neutral:
                        type = "neutral";
                        break;
                    case Settings.Lists.ScoreType.Work:
                        type = "good";
                        break;
                }
                LogLine($"{string.Format("{0,-" + (maxlen + 1).ToString() + "}", pair.Key)}({type,-8}) : {UnixTimestamp.ConvertIntervalToDateTime(pair.Value).ToLongTimeString()}");
                counter++;
                timeCounter += pair.Value;
                if (counter >= LINES_LIMIT)
                {
                    break;
                }
            }
            if (counter >= LINES_LIMIT)
            {
                // we have more
                int totalTime = rep.Times.Sum(p => p.Value);
                LogLine($"{string.Format("{0,-" + (maxlen + 1).ToString() + "}", "other")}{"",-10} : {UnixTimestamp.ConvertIntervalToDateTime(totalTime - timeCounter).ToLongTimeString()}");
            }
        }

        private void btProcessReport_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            var rep = logger.log.GetProcessesReport();
            //var re = from c in rep.Times
            //    orderby c.Value
            //    select $"{c.Key}\t-\t{UnixTimestamp.ConvertIntervalToDateTime(c.Value).ToLongTimeString()}";
            int counter = 0;
            int timeCounter = 0;
            int maxlen = rep.Times.Select(c => c.Key.Length).Max();
            foreach (var pair in from c in rep.Times orderby c.Value descending select c)
            {
                string type = "unknown";
                if (Settings.Self.ScoreLists.IsBrowserProcess(pair.Key))
                {
                    type = "browser";
                }
                else
                {
                    switch (Settings.Self.ScoreLists.GetProceScoreType(pair.Key))
                    {
                        case Settings.Lists.ScoreType.Nonwork:
                            type = "bad";
                            break;
                        case Settings.Lists.ScoreType.Neutral:
                            type = "neutral";
                            break;
                        case Settings.Lists.ScoreType.Work:
                            type = "good";
                            break;
                    }
                }

                LogLine($"{string.Format("{0,-"+(maxlen+1).ToString() + "}",pair.Key)}({type,-8}) : {UnixTimestamp.ConvertIntervalToDateTime(pair.Value).ToLongTimeString()}");
                counter++;
                timeCounter += pair.Value;
                if (counter >= LINES_LIMIT)
                {
                    break;
                }
            }
            if (counter >= LINES_LIMIT)
            {
                // we have more
                int totalTime = rep.Times.Sum(p => p.Value);
                LogLine($"{string.Format("{0,-" + (maxlen + 1).ToString() + "}", "other")}{"",-10} : {UnixTimestamp.ConvertIntervalToDateTime(totalTime - timeCounter).ToLongTimeString()}");
            }
        }

        private void btActiveReport_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            var rep = logger.log.GetActivityReport();
            LogLine($"active time {UnixTimestamp.ConvertIntervalToDateTime(rep.ActiveSeconds).ToLongTimeString()}");
            LogLine($"non active time {UnixTimestamp.ConvertIntervalToDateTime(rep.NonActiveSeconds).ToLongTimeString()}");
            LogLine($"total time {UnixTimestamp.ConvertIntervalToDateTime(rep.TotalSeconds).ToLongTimeString()}");
            LogLine($"active percentage {rep.ActiveSeconds * 100 / rep.TotalSeconds}%");
        }

        private void btWorkReport_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            var rep = logger.log.GetWorkReport();
            LogLine($"worked time {UnixTimestamp.ConvertIntervalToDateTime(rep.WorkedSeconds).ToLongTimeString()}");
            LogLine($"non worked time {UnixTimestamp.ConvertIntervalToDateTime(rep.NonWorkedSeconds).ToLongTimeString()}");
            LogLine($"total time {UnixTimestamp.ConvertIntervalToDateTime(rep.TotalSeconds).ToLongTimeString()}");
            LogLine($"work percentage {rep.WorkedSeconds* 100 / rep.TotalSeconds}%");
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = false;
            }
            else
            {
                ShowInTaskbar = true;
            }
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            if(WindowState == FormWindowState.Minimized)
                WindowState = FormWindowState.Normal;
        }

        private void processesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ListsSettingsForm(ListsSettingsForm.Mode.Processes).ShowDialog();
        }

        private void sitesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ListsSettingsForm(ListsSettingsForm.Mode.Sites).ShowDialog();
        }

        private void btClearLog_Click(object sender, EventArgs e)
        {
            logger.ClearLog();
        }
    }
}
