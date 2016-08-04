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
            switch (mode)
            {
                case Mode.Processes:
                    dict = Settings.Self.ScoreLists.processTypes;
                    break;
                case Mode.Sites:
                    dict = Settings.Self.ScoreLists.siteTypes;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            InitializeComponent();
        }

        private void ListsSettingsForm_Load(object sender, EventArgs e)
        {
            //var row = new DataGridViewRow();
            
            RefreshDataGrid();

            //dataGridView1.Rows.Add("azaza", "23:33", WorkControl.Properties.Resources.red);
        }

        private void RefreshDataGrid()
        {
            var index = dataGridView1.FirstDisplayedScrollingColumnIndex;
            dataGridView1.Rows.Clear();
            foreach (var pair in dict)
            {
                var icon = GetApproprIcon(pair.Value);
                dataGridView1.Rows.Add(pair.Key, "", icon);
            }
            dataGridView1.FirstDisplayedScrollingColumnIndex = index;
        }

        private Icon GetApproprIcon(Settings.Lists.ScoreType type)
        {
            switch (type)
            {
                case Settings.Lists.ScoreType.Nonwork:
                    return WorkControl.Properties.Resources.red;
                case Settings.Lists.ScoreType.Neutral:
                    return WorkControl.Properties.Resources.yellow;
                case Settings.Lists.ScoreType.Work:
                    return WorkControl.Properties.Resources.green;
                case Settings.Lists.ScoreType.Unknown:
                    return WorkControl.Properties.Resources.unknown;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1)
            {
                MessageBox.Show("Please, select one row to edit");
                return;
            }
            var selectedRow = dataGridView1.SelectedRows[0];
            string name = selectedRow.Cells[0].Value.ToString();
            var f = new ListSettingsItemEditForm(name,dict[name]);
            f.ShowDialog();
            string newName = f.name;
            var type = f.type;
            if (newName != name)
            {
                //we need to delete item
                dict.Remove(name);
            }
            dict[newName] = type;
            Settings.Self.ScoreLists.Save();
            RefreshDataGrid();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please, select row to delete");
                return;
            }
            foreach (DataGridViewRow selectedRow in dataGridView1.SelectedRows)
            {
                dict.Remove(selectedRow.Cells[0].Value.ToString());
            }
            Settings.Self.ScoreLists.Save();
            RefreshDataGrid();
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            var f = new ListSettingsItemEditForm();
            f.ShowDialog();
            string name = f.name;
            var type = f.type;
            if (dict.ContainsKey(name))
            {
                MessageBox.Show("This item already exists");
                return;
            }
            dict[name] = type;
            Settings.Self.ScoreLists.Save();
            RefreshDataGrid();
        }

        //TODO: implement edit form
    }
}
