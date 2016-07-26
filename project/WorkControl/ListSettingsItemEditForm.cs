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
    public partial class ListSettingsItemEditForm : Form
    {
        public string name;
        public Settings.Lists.ScoreType type;
        public ListSettingsItemEditForm(string name = "", Settings.Lists.ScoreType type = Settings.Lists.ScoreType.Nonwork)
        {
            this.name = name;
            this.type = type;
            InitializeComponent();
        }

        private void ListSettingsItemEditForm_Load(object sender, EventArgs e)
        {
            tbName.Text = name;
            switch (type)
            {
                case Settings.Lists.ScoreType.Nonwork:
                case Settings.Lists.ScoreType.Neutral:
                case Settings.Lists.ScoreType.Work:
                    comboBox1.SelectedIndex = (int) type;
                    break;
            }
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            name = tbName.Text;
            type = (Settings.Lists.ScoreType) comboBox1.SelectedIndex;
            Close();
        }
    }
}
