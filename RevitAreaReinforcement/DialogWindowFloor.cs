#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-NonCommercial-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в некоммерческих целях,
при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2020, все права защищены.
This code is listed under the Creative Commons Attribution-NonCommercial-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially,
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
