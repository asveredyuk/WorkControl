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
            //LogItem last = logger.log.Last();
          //  textBox1.AppendText(last.ToString() + "\r\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime time = new DateTime();
            time = time.AddSeconds(logger.log.GetWorkedSeconds());
            button1.Text = time.ToString("T");
        }
    }
}
