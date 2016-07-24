using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkControl
{
    public partial class ListsSettingsForm : Form
    {
        Dictionary<string, Settings.Lists.ScoreType> dict; 
        public enum Mode
        {
            Processes,
            Sites
        }

        private Mode mode;
        public ListsSettingsForm(Mode mode)
        {
            this.mode = mode;
            InitializeComponent();
        }

        private void ListsSettingsForm_Load(object sender, EventArgs e)
        {

        }
        //TODO: implement edit form
    }
}
