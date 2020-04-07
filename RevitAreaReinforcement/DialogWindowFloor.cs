using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RevitAreaReinforcement
{
    public partial class DialogWindowFloor : Form
    {
        public RebarInfoFloor rif;

        public DialogWindowFloor(RebarInfoFloor Info, List<string> rebarTypes)
        {
            InitializeComponent();

            rif = Info;

            comboBoxType.DataSource = rebarTypes;
            comboBoxType.Text = Info.rebarTypeName;
            numInterval.Value = (decimal)(Info.interval * 304.8);
            numCoverBottom.Value = (decimal)(Info.bottomCover * 304.8);
            numCoverTop.Value = (decimal)(Info.topCover * 304.8);

        }

        private void DialogWindowFloor_Load(object sender, EventArgs e)
        {

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            rif.rebarTypeName = comboBoxType.SelectedItem.ToString();
            rif.interval = ((double)numInterval.Value) / 304.8;
            rif.bottomCover = ((double)numCoverBottom.Value) / 304.8;
            rif.topCover = ((double)numCoverTop.Value) / 304.8;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
