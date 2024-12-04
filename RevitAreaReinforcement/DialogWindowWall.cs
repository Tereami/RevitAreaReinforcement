#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2020, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2020, all rigths reserved.*/
#endregion
#region Usings
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
#endregion

namespace RevitAreaReinforcement
{
    public partial class DialogWindowWall : Form
    {
        public RebarInfoWall wri;

        public DialogWindowWall(RebarInfoWall reinfInfo, List<string> rebarTypes1, List<string> rebarTypes2)
        {
            InitializeComponent();

            wri = reinfInfo;

            checkBoxGenerateHorizontal.Checked = reinfInfo.generateHorizontal;
            txtBoxHorizArmSection.Text = reinfInfo.horizontalSectionText;
            checkBoxGenerateVertical.Checked = reinfInfo.generateVertical;
            txtBoxVertArmSection.Text = reinfInfo.verticalSectionText;

            txtBackOffset.Text = (reinfInfo.backOffset * 304.8).ToString();
            txtBottomOffset.Text = (reinfInfo.bottomOffset * 304.8).ToString();
            txtTopOffset.Text = (reinfInfo.topOffset * 304.8).ToString();

            txtHorizontalInterval.Text = (reinfInfo.horizontalRebarInterval * 304.8).ToString();
            txtVerticalInterval.Text = (reinfInfo.verticalRebarInterval * 304.8).ToString();

            txtRebarCover.Text = (reinfInfo.rebarCover * 304.8).ToString();

            txtVerticalFreeLength.Text = (reinfInfo.verticalFreeLength * 304.8).ToString();
            checkBoxAutoVerticalFreeLengh.Checked = wri.autoVerticalFreeLength;
            checkBox_AsymVertFreeLength.Checked = wri.verticalAsymmOffset;
            radioButtonForceUp.Checked = wri.verticalRebarStretched;
            numericUpDownVertFreeLengthRound.Value = (decimal)(wri.verticalFreeLengthRound * 304.8);

            txtHorizontalFreeLength.Text = (reinfInfo.horizontalFreeLength * 304.8).ToString();
            cmbHorizonalType.DataSource = rebarTypes1;
            cmbVerticalType.DataSource = rebarTypes2;
            cmbHorizonalType.SelectedItem = reinfInfo.horizontalRebarTypeName;
            cmbVerticalType.SelectedItem = reinfInfo.verticalRebarTypeName;

            checkBoxHorizAddInterval.Checked = reinfInfo.horizontalAddInterval;

            checkBoxUnificateLength.Checked = reinfInfo.useUnification;
            textBoxLengths.Text = string.Join(";",
                reinfInfo.lengthsUnification.Select(i => (i * 304.8).ToString("F0")));

            string appVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.Text = $"{this.Text} v. {appVersion}";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            wri.generateHorizontal = checkBoxGenerateHorizontal.Checked;
            wri.verticalSectionText = txtBoxVertArmSection.Text;
            wri.generateVertical = checkBoxGenerateVertical.Checked;
            wri.horizontalSectionText = txtBoxHorizArmSection.Text;

            wri.backOffset = double.Parse(txtBackOffset.Text) / 304.8;
            wri.bottomOffset = double.Parse(txtBottomOffset.Text) / 304.8;
            wri.topOffset = double.Parse(txtTopOffset.Text) / 304.8;

            wri.horizontalRebarInterval = double.Parse(txtHorizontalInterval.Text) / 304.8;
            wri.verticalRebarInterval = double.Parse(txtVerticalInterval.Text) / 304.8;

            wri.rebarCover = double.Parse(txtRebarCover.Text) / 304.8;

            wri.verticalFreeLength = double.Parse(txtVerticalFreeLength.Text) / 304.8;
            wri.autoVerticalFreeLength = checkBoxAutoVerticalFreeLengh.Checked;
            wri.verticalAsymmOffset = checkBox_AsymVertFreeLength.Checked;
            wri.verticalRebarStretched = radioButtonForceUp.Checked;
            wri.verticalFreeLengthRound = ((double)numericUpDownVertFreeLengthRound.Value) / 304.8;


            wri.horizontalFreeLength = double.Parse(txtHorizontalFreeLength.Text) / 304.8;
            wri.horizontalRebarTypeName = cmbHorizonalType.Text;
            wri.verticalRebarTypeName = cmbVerticalType.Text;

            wri.horizontalAddInterval = checkBoxHorizAddInterval.Checked;

            wri.useUnification = checkBoxUnificateLength.Checked;
            wri.lengthsUnification = new List<double>();
            foreach (string lenStr in textBoxLengths.Text.Split(';'))
            {
                double l = 0;
                double.TryParse(lenStr, out l);
                if (l == 0) continue;
                l = l / 304.8;
                wri.lengthsUnification.Add(l);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private void checkBoxGenerateVertical_CheckedChanged(object sender, EventArgs e)
        {
            bool chkd = checkBoxGenerateVertical.Checked;

            cmbVerticalType.Enabled = chkd;
            txtBoxVertArmSection.Enabled = chkd;
            txtVerticalFreeLength.Enabled = chkd;
            txtVerticalInterval.Enabled = chkd;
        }

        private void checkBoxGenerateHorizontal_CheckedChanged(object sender, EventArgs e)
        {
            bool chkd = checkBoxGenerateHorizontal.Checked;

            cmbHorizonalType.Enabled = chkd;
            txtBoxHorizArmSection.Enabled = chkd;
            txtTopOffset.Enabled = chkd;
            txtBottomOffset.Enabled = chkd;
            txtHorizontalInterval.Enabled = chkd;
        }

        private void checkBoxUnificateLength_CheckedChanged(object sender, EventArgs e)
        {
            textBoxLengths.Enabled = checkBoxUnificateLength.Checked;
        }

        private void checkBoxAutoVerticalFreeLengh_CheckedChanged(object sender, EventArgs e)
        {
            bool chkd = checkBoxAutoVerticalFreeLengh.Checked;
            txtVerticalFreeLength.Enabled = !chkd;
            checkBox_AsymVertFreeLength.Enabled = chkd;
            radioButtonForceUp.Enabled = chkd;
            radioButtonForceDown.Enabled = chkd;
            numericUpDownVertFreeLengthRound.Enabled = chkd;
            label1.Enabled = chkd;
        }
    }
}
