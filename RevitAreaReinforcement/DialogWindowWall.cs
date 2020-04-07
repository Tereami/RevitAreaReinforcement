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
            txtHorizontalFreeLength.Text = (reinfInfo.horizontalFreeLength * 304.8).ToString();

            cmbHorizonalType.DataSource = rebarTypes1;
            cmbVerticalType.DataSource = rebarTypes2;
            cmbHorizonalType.SelectedItem = reinfInfo.horizontalRebarTypeName; //SettingsStorage.rebarTypes1.Where(i => i.bartype.Name == reinfInfo.horizontalRebarTypeName).First();
            cmbVerticalType.SelectedItem = reinfInfo.verticalRebarTypeName; // SettingsStorage.rebarTypes2.Where(i => i.bartype.Name == reinfInfo.verticalRebarTypeName).First();

            checkBoxHorizAddInterval.Checked = reinfInfo.horizontalAddInterval;

            this.Text = "Армирование стен. Версия " + System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString();
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
            wri.horizontalFreeLength = double.Parse(txtHorizontalFreeLength.Text) / 304.8;
            
            wri.horizontalRebarTypeName = cmbHorizonalType.Text;
            wri.verticalRebarTypeName = cmbVerticalType.Text;

            wri.horizontalAddInterval = checkBoxHorizAddInterval.Checked;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private void checkBoxGenerateVertical_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxGenerateVertical.Checked)
            {
                cmbVerticalType.Enabled = true;
                txtBoxVertArmSection.Enabled = true;
                txtVerticalFreeLength.Enabled = true;
                txtVerticalInterval.Enabled = true;
            }
            else
            {
                cmbVerticalType.Enabled = false;
                txtBoxVertArmSection.Enabled = false;
                txtVerticalFreeLength.Enabled = false;
                txtVerticalInterval.Enabled = false;
            }
        }

        private void checkBoxGenerateHorizontal_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxGenerateHorizontal.Checked)
            {
                cmbHorizonalType.Enabled = true;
                txtBoxHorizArmSection.Enabled = true;
                txtTopOffset.Enabled = true;
                txtBottomOffset.Enabled = true;
                txtHorizontalInterval.Enabled = true;
            }
            else
            {
                cmbHorizonalType.Enabled = false;
                txtBoxHorizArmSection.Enabled = false;
                txtTopOffset.Enabled = false;
                txtBottomOffset.Enabled = false;
                txtHorizontalInterval.Enabled = false;
            }
        }
    }
}
