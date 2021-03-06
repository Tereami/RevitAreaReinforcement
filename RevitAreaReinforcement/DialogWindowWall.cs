﻿#region License
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            txtVerticalOffset.Text = (reinfInfo.verticalOffset * 304.8).ToString();

            txtHorizontalInterval.Text = (reinfInfo.horizontalRebarInterval * 304.8).ToString();
            txtVerticalInterval.Text = (reinfInfo.verticalRebarInterval * 304.8).ToString();

            txtRebarCover.Text = (reinfInfo.rebarCover * 304.8).ToString();

            txtVerticalFreeLength.Text = (reinfInfo.verticalFreeLength * 304.8).ToString();
            checkBoxAutoVerticalFreeLengh.Checked = wri.autoVerticalFreeLength;
            txtHorizontalFreeLength.Text = (reinfInfo.horizontalFreeLength * 304.8).ToString();

            cmbHorizonalType.DataSource = rebarTypes1;
            cmbVerticalType.DataSource = rebarTypes2;
            cmbHorizonalType.SelectedItem = reinfInfo.horizontalRebarTypeName;
            cmbVerticalType.SelectedItem = reinfInfo.verticalRebarTypeName;

            checkBoxHorizAddInterval.Checked = reinfInfo.horizontalAddInterval;

            checkBoxUnificateLength.Checked = reinfInfo.useUnification;
            foreach(double len in reinfInfo.lengthsUnification)
            {
                string letText = (len * 304.8).ToString("F0");
                textBoxLengths.Text += letText + ";";
            }

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
            wri.verticalOffset = double.Parse(txtVerticalOffset.Text) / 304.8;

            wri.horizontalRebarInterval = double.Parse(txtHorizontalInterval.Text) / 304.8;
            wri.verticalRebarInterval = double.Parse(txtVerticalInterval.Text) / 304.8;

            wri.rebarCover = double.Parse(txtRebarCover.Text) / 304.8;

            wri.verticalFreeLength = double.Parse(txtVerticalFreeLength.Text) / 304.8;
            wri.autoVerticalFreeLength = checkBoxAutoVerticalFreeLengh.Checked;
            wri.horizontalFreeLength = double.Parse(txtHorizontalFreeLength.Text) / 304.8;

            wri.horizontalRebarTypeName = cmbHorizonalType.Text;
            wri.verticalRebarTypeName = cmbVerticalType.Text;

            wri.horizontalAddInterval = checkBoxHorizAddInterval.Checked;

            wri.useUnification = checkBoxUnificateLength.Checked;
            wri.lengthsUnification = new List<double>();
            foreach(string lenStr in textBoxLengths.Text.Split(';'))
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
            if (checkBoxGenerateVertical.Checked)
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
            if (checkBoxGenerateHorizontal.Checked)
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

        private void checkBoxUnificateLength_CheckedChanged(object sender, EventArgs e)
        {
            textBoxLengths.Enabled = checkBoxUnificateLength.Checked;
        }

        private void checkBoxAutoVerticalFreeLengh_CheckedChanged(object sender, EventArgs e)
        {
            txtVerticalFreeLength.Enabled = !this.Enabled;
        }
    }
}
