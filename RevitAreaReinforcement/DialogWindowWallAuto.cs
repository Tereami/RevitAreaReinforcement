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
    public partial class DialogWindowWallAuto : Form
    {
        public RebarInfoWall rebarInfo;
        public DialogWindowWallAuto(RebarInfoWall riw)
        {
            InitializeComponent();

            rebarInfo = riw;

            radioButtonFreeLengthAuto.Checked = riw.autoVerticalFreeLength;
            numericUpDownHorizOffset.Value = (decimal)(304.8 * riw.bottomOffset);
            checkBoxAddHorizStep.Checked = riw.horizontalAddInterval;
            textBoxRazdelVert.Text = riw.verticalSectionText;
            textBoxRazdelHoris.Text = riw.horizontalSectionText;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            rebarInfo.autoVerticalFreeLength = radioButtonFreeLengthAuto.Checked;
            rebarInfo.bottomOffset = ((double)numericUpDownHorizOffset.Value) / 304.8;
            rebarInfo.horizontalAddInterval = checkBoxAddHorizStep.Checked;
            rebarInfo.verticalSectionText = textBoxRazdelVert.Text;
            rebarInfo.horizontalSectionText = textBoxRazdelHoris.Text;

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
