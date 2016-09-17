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
    public partial class SettingsWizardForm : Form
    {
        Dictionary<string, Settings.Lists.ScoreType> backup;
        Dictionary<string, Settings.Lists.ScoreType> dict;
        public enum Mode
        {
            Processes,
            Sites
        }

        Mode mode;
        public SettingsWizardForm(Mode mode)
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
            backup = new Dictionary<string, Settings.Lists.ScoreType>(dict);
            InitializeComponent();
        }

        private void SettingsWizardForm_Load(object sender, EventArgs e)
        {
            RefreshList();

        }

        private void RefreshList()
        {
            List<object> names = new List<object>(from c in dict select c.Key);
            lbItems.Items.Clear();
            lbItems.Items.AddRange(names.ToArray());
        }
        private void lbItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            string item = lbItems.SelectedItem.ToString();
            OnItemSelected(item);
        }

        private string selectedItem;

        private void SaveChangesToItem()
        {
            if(selectedItem == null)
                return;
            if (selectedItem != textBoxName.Text)
            {
                //name has changed
                dict.Remove(selectedItem);
            }
            dict[textBoxName.Text] = (Settings.Lists.ScoreType)comboBoxType.SelectedIndex;

        }
        private void OnItemSelected(string item)
        {
            SaveChangesToItem();
            selectedItem = item;
            if (!groupBoxItemSettings.Visible)
                groupBoxItemSettings.Visible = true;
            textBoxName.Text = item;
            Settings.Lists.ScoreType type = dict[item];
            switch (type)
            {
                case Settings.Lists.ScoreType.Nonwork:
                case Settings.Lists.ScoreType.Neutral:
                case Settings.Lists.ScoreType.Work:
                    comboBoxType.SelectedIndex = (int) type;
                    break;
            }
        }

        private void Save()
        {
            Settings.Self.ScoreLists.Save();
        }

        private void Discard()
        {

            switch (mode)
            {
                case Mode.Processes:
                    Settings.Self.ScoreLists.processTypes = backup;
                    break;
                case Mode.Sites:
                    Settings.Self.ScoreLists.siteTypes = backup;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private void btOk_Click(object sender, EventArgs e)
        {
            Save();
        }


        private void SettingsWizardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (selectedItem != null)
            {
                var res = MessageBox.Show("Do you want to save changes?", "Unsaved changes",
                    MessageBoxButtons.YesNoCancel);
                switch (res)
                {
                        case DialogResult.Cancel:
                            e.Cancel = true;
                            break;
                        case DialogResult.No:
                            Discard();
                            break;
                        case DialogResult.Yes:
                            Save();
                            break;
                }
            }
        }
    }
}
