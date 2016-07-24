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
        private void timer1_Tick(object sender, EventArgs e)
        {
            logger.LogNow();
            if (logger.log.IsActiveLast())
            {
                BackColor = Color.Green;
                //button1.Text = "active";
            }
            else
            {
                BackColor = Color.Red;
                //button1.Text = "not active";
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
    }
}
