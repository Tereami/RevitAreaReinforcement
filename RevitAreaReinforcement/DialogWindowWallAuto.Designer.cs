namespace RevitAreaReinforcement
{
    partial class DialogWindowWallAuto
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonFreeLengthAuto = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.radioButtonFreeLengthManual = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numericUpDownHorizOffset = new System.Windows.Forms.NumericUpDown();
            this.checkBoxAddHorizStep = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxRazdelHoris = new System.Windows.Forms.TextBox();
            this.textBoxRazdelVert = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.checkBoxAdditionalStepSpace = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHorizOffset)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(288, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "Армирование будет выполнено автоматически по данным, указанным в стенах.";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.radioButtonFreeLengthAuto);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.radioButtonFreeLengthManual);
            this.groupBox1.Location = new System.Drawing.Point(12, 48);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(288, 111);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Выпуски вертикальной арматуры";
            // 
            // radioButtonFreeLengthAuto
            // 
            this.radioButtonFreeLengthAuto.AutoSize = true;
            this.radioButtonFreeLengthAuto.Location = new System.Drawing.Point(7, 43);
            this.radioButtonFreeLengthAuto.Name = "radioButtonFreeLengthAuto";
            this.radioButtonFreeLengthAuto.Size = new System.Drawing.Size(103, 17);
            this.radioButtonFreeLengthAuto.TabIndex = 1;
            this.radioButtonFreeLengthAuto.Text = "Автоматически";
            this.radioButtonFreeLengthAuto.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(6, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(276, 36);
            this.label2.TabIndex = 0;
            this.label2.Text = "Требуются параметры Рзм.ТолщинаПерекрытия, Арм.КлассЧисло, Мтрл.КодМатериала";
            // 
            // radioButtonFreeLengthManual
            // 
            this.radioButtonFreeLengthManual.AutoSize = true;
            this.radioButtonFreeLengthManual.Checked = true;
            this.radioButtonFreeLengthManual.Location = new System.Drawing.Point(7, 20);
            this.radioButtonFreeLengthManual.Name = "radioButtonFreeLengthManual";
            this.radioButtonFreeLengthManual.Size = new System.Drawing.Size(201, 17);
            this.radioButtonFreeLengthManual.TabIndex = 0;
            this.radioButtonFreeLengthManual.TabStop = true;
            this.radioButtonFreeLengthManual.Text = "Из параметра Арм.ДлинаВыпуска";
            this.radioButtonFreeLengthManual.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.numericUpDownHorizOffset);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.checkBoxAdditionalStepSpace);
            this.groupBox2.Controls.Add(this.checkBoxAddHorizStep);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(12, 165);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(288, 129);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Горизонтальная арматура";
            // 
            // numericUpDownHorizOffset
            // 
            this.numericUpDownHorizOffset.Location = new System.Drawing.Point(129, 19);
            this.numericUpDownHorizOffset.Name = "numericUpDownHorizOffset";
            this.numericUpDownHorizOffset.Size = new System.Drawing.Size(63, 20);
            this.numericUpDownHorizOffset.TabIndex = 4;
            this.numericUpDownHorizOffset.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // checkBoxAddHorizStep
            // 
            this.checkBoxAddHorizStep.AutoSize = true;
            this.checkBoxAddHorizStep.Checked = true;
            this.checkBoxAddHorizStep.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAddHorizStep.Location = new System.Drawing.Point(9, 45);
            this.checkBoxAddHorizStep.Name = "checkBoxAddHorizStep";
            this.checkBoxAddHorizStep.Size = new System.Drawing.Size(101, 17);
            this.checkBoxAddHorizStep.TabIndex = 3;
            this.checkBoxAddHorizStep.Text = "Доборный шаг";
            this.checkBoxAddHorizStep.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(198, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "мм";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Отступы";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.textBoxRazdelHoris);
            this.groupBox3.Controls.Add(this.textBoxRazdelVert);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(12, 300);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(288, 84);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Параметр \"Раздел\"";
            // 
            // textBoxRazdelHoris
            // 
            this.textBoxRazdelHoris.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRazdelHoris.Location = new System.Drawing.Point(128, 47);
            this.textBoxRazdelHoris.Name = "textBoxRazdelHoris";
            this.textBoxRazdelHoris.Size = new System.Drawing.Size(153, 20);
            this.textBoxRazdelHoris.TabIndex = 5;
            // 
            // textBoxRazdelVert
            // 
            this.textBoxRazdelVert.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRazdelVert.Location = new System.Drawing.Point(128, 21);
            this.textBoxRazdelVert.Name = "textBoxRazdelVert";
            this.textBoxRazdelVert.Size = new System.Drawing.Size(153, 20);
            this.textBoxRazdelVert.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Для вертикальной:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Для горизонтальной:";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(225, 395);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Location = new System.Drawing.Point(145, 395);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 4;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // checkBoxAdditionalStepSpace
            // 
            this.checkBoxAdditionalStepSpace.AutoSize = true;
            this.checkBoxAdditionalStepSpace.Checked = true;
            this.checkBoxAdditionalStepSpace.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAdditionalStepSpace.Location = new System.Drawing.Point(9, 68);
            this.checkBoxAdditionalStepSpace.Name = "checkBoxAdditionalStepSpace";
            this.checkBoxAdditionalStepSpace.Size = new System.Drawing.Size(152, 17);
            this.checkBoxAdditionalStepSpace.TabIndex = 3;
            this.checkBoxAdditionalStepSpace.Text = "Учащение по низу/верху";
            this.checkBoxAdditionalStepSpace.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.Location = new System.Drawing.Point(6, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(276, 38);
            this.label7.TabIndex = 0;
            this.label7.Text = "Требуются параметры Арм.ВысотаУчащенияНиз и Арм.ВысотаУчащенияВерх";
            // 
            // DialogWindowWallAuto
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(312, 430);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DialogWindowWallAuto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHorizOffset)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonFreeLengthAuto;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioButtonFreeLengthManual;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numericUpDownHorizOffset;
        private System.Windows.Forms.CheckBox checkBoxAddHorizStep;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBoxRazdelHoris;
        private System.Windows.Forms.TextBox textBoxRazdelVert;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkBoxAdditionalStepSpace;
    }
}