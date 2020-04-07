namespace RevitAreaReinforcement
{
    partial class DialogWindowWall
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.txtVerticalInterval = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtRebarCover = new System.Windows.Forms.TextBox();
            this.txtBackOffset = new System.Windows.Forms.TextBox();
            this.txtHorizontalFreeLength = new System.Windows.Forms.TextBox();
            this.txtVerticalFreeLength = new System.Windows.Forms.TextBox();
            this.txtTopOffset = new System.Windows.Forms.TextBox();
            this.txtHorizontalInterval = new System.Windows.Forms.TextBox();
            this.txtBottomOffset = new System.Windows.Forms.TextBox();
            this.cmbHorizonalType = new System.Windows.Forms.ComboBox();
            this.cmbVerticalType = new System.Windows.Forms.ComboBox();
            this.checkBoxGenerateVertical = new System.Windows.Forms.CheckBox();
            this.checkBoxGenerateHorizontal = new System.Windows.Forms.CheckBox();
            this.txtBoxVertArmSection = new System.Windows.Forms.TextBox();
            this.txtBoxHorizArmSection = new System.Windows.Forms.TextBox();
            this.checkBoxHorizAddInterval = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(699, 595);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(77, 22);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(780, 595);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(77, 22);
            this.button2.TabIndex = 2;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtVerticalInterval
            // 
            this.txtVerticalInterval.Location = new System.Drawing.Point(339, 494);
            this.txtVerticalInterval.Margin = new System.Windows.Forms.Padding(2);
            this.txtVerticalInterval.Name = "txtVerticalInterval";
            this.txtVerticalInterval.Size = new System.Drawing.Size(50, 20);
            this.txtVerticalInterval.TabIndex = 3;
            this.txtVerticalInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::RevitAreaReinforcement.Properties.Resources.dialog_picture1;
            this.pictureBox1.Location = new System.Drawing.Point(8, 8);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(848, 538);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // txtRebarCover
            // 
            this.txtRebarCover.Location = new System.Drawing.Point(687, 494);
            this.txtRebarCover.Margin = new System.Windows.Forms.Padding(2);
            this.txtRebarCover.Name = "txtRebarCover";
            this.txtRebarCover.Size = new System.Drawing.Size(45, 20);
            this.txtRebarCover.TabIndex = 3;
            this.txtRebarCover.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtBackOffset
            // 
            this.txtBackOffset.Enabled = false;
            this.txtBackOffset.Location = new System.Drawing.Point(241, 494);
            this.txtBackOffset.Margin = new System.Windows.Forms.Padding(2);
            this.txtBackOffset.Name = "txtBackOffset";
            this.txtBackOffset.Size = new System.Drawing.Size(50, 20);
            this.txtBackOffset.TabIndex = 3;
            this.txtBackOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtHorizontalFreeLength
            // 
            this.txtHorizontalFreeLength.Enabled = false;
            this.txtHorizontalFreeLength.Location = new System.Drawing.Point(150, 494);
            this.txtHorizontalFreeLength.Margin = new System.Windows.Forms.Padding(2);
            this.txtHorizontalFreeLength.Name = "txtHorizontalFreeLength";
            this.txtHorizontalFreeLength.Size = new System.Drawing.Size(50, 20);
            this.txtHorizontalFreeLength.TabIndex = 3;
            this.txtHorizontalFreeLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtVerticalFreeLength
            // 
            this.txtVerticalFreeLength.Location = new System.Drawing.Point(43, 88);
            this.txtVerticalFreeLength.Margin = new System.Windows.Forms.Padding(2);
            this.txtVerticalFreeLength.Name = "txtVerticalFreeLength";
            this.txtVerticalFreeLength.Size = new System.Drawing.Size(50, 20);
            this.txtVerticalFreeLength.TabIndex = 3;
            this.txtVerticalFreeLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtTopOffset
            // 
            this.txtTopOffset.Location = new System.Drawing.Point(43, 191);
            this.txtTopOffset.Margin = new System.Windows.Forms.Padding(2);
            this.txtTopOffset.Name = "txtTopOffset";
            this.txtTopOffset.Size = new System.Drawing.Size(50, 20);
            this.txtTopOffset.TabIndex = 3;
            this.txtTopOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtHorizontalInterval
            // 
            this.txtHorizontalInterval.Location = new System.Drawing.Point(43, 291);
            this.txtHorizontalInterval.Margin = new System.Windows.Forms.Padding(2);
            this.txtHorizontalInterval.Name = "txtHorizontalInterval";
            this.txtHorizontalInterval.Size = new System.Drawing.Size(50, 20);
            this.txtHorizontalInterval.TabIndex = 3;
            this.txtHorizontalInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtBottomOffset
            // 
            this.txtBottomOffset.Location = new System.Drawing.Point(43, 465);
            this.txtBottomOffset.Margin = new System.Windows.Forms.Padding(2);
            this.txtBottomOffset.Name = "txtBottomOffset";
            this.txtBottomOffset.Size = new System.Drawing.Size(50, 20);
            this.txtBottomOffset.TabIndex = 3;
            this.txtBottomOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cmbHorizonalType
            // 
            this.cmbHorizonalType.FormattingEnabled = true;
            this.cmbHorizonalType.Location = new System.Drawing.Point(535, 455);
            this.cmbHorizonalType.Margin = new System.Windows.Forms.Padding(2);
            this.cmbHorizonalType.Name = "cmbHorizonalType";
            this.cmbHorizonalType.Size = new System.Drawing.Size(115, 21);
            this.cmbHorizonalType.TabIndex = 4;
            // 
            // cmbVerticalType
            // 
            this.cmbVerticalType.FormattingEnabled = true;
            this.cmbVerticalType.Location = new System.Drawing.Point(506, 74);
            this.cmbVerticalType.Margin = new System.Windows.Forms.Padding(2);
            this.cmbVerticalType.Name = "cmbVerticalType";
            this.cmbVerticalType.Size = new System.Drawing.Size(115, 21);
            this.cmbVerticalType.TabIndex = 4;
            // 
            // checkBoxGenerateVertical
            // 
            this.checkBoxGenerateVertical.AutoSize = true;
            this.checkBoxGenerateVertical.Checked = true;
            this.checkBoxGenerateVertical.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxGenerateVertical.Location = new System.Drawing.Point(506, 52);
            this.checkBoxGenerateVertical.Name = "checkBoxGenerateVertical";
            this.checkBoxGenerateVertical.Size = new System.Drawing.Size(98, 17);
            this.checkBoxGenerateVertical.TabIndex = 5;
            this.checkBoxGenerateVertical.Text = "Вертикальная";
            this.checkBoxGenerateVertical.UseVisualStyleBackColor = true;
            this.checkBoxGenerateVertical.CheckedChanged += new System.EventHandler(this.checkBoxGenerateVertical_CheckedChanged);
            // 
            // checkBoxGenerateHorizontal
            // 
            this.checkBoxGenerateHorizontal.AutoSize = true;
            this.checkBoxGenerateHorizontal.Checked = true;
            this.checkBoxGenerateHorizontal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxGenerateHorizontal.Location = new System.Drawing.Point(535, 433);
            this.checkBoxGenerateHorizontal.Name = "checkBoxGenerateHorizontal";
            this.checkBoxGenerateHorizontal.Size = new System.Drawing.Size(109, 17);
            this.checkBoxGenerateHorizontal.TabIndex = 6;
            this.checkBoxGenerateHorizontal.Text = "Горизонтальная";
            this.checkBoxGenerateHorizontal.UseVisualStyleBackColor = true;
            this.checkBoxGenerateHorizontal.CheckedChanged += new System.EventHandler(this.checkBoxGenerateHorizontal_CheckedChanged);
            // 
            // txtBoxVertArmSection
            // 
            this.txtBoxVertArmSection.Location = new System.Drawing.Point(506, 100);
            this.txtBoxVertArmSection.Name = "txtBoxVertArmSection";
            this.txtBoxVertArmSection.Size = new System.Drawing.Size(115, 20);
            this.txtBoxVertArmSection.TabIndex = 7;
            // 
            // txtBoxHorizArmSection
            // 
            this.txtBoxHorizArmSection.Location = new System.Drawing.Point(535, 481);
            this.txtBoxHorizArmSection.Name = "txtBoxHorizArmSection";
            this.txtBoxHorizArmSection.Size = new System.Drawing.Size(115, 20);
            this.txtBoxHorizArmSection.TabIndex = 7;
            // 
            // checkBoxHorizAddInterval
            // 
            this.checkBoxHorizAddInterval.AutoSize = true;
            this.checkBoxHorizAddInterval.Location = new System.Drawing.Point(535, 507);
            this.checkBoxHorizAddInterval.Name = "checkBoxHorizAddInterval";
            this.checkBoxHorizAddInterval.Size = new System.Drawing.Size(101, 17);
            this.checkBoxHorizAddInterval.TabIndex = 6;
            this.checkBoxHorizAddInterval.Text = "Доборный шаг";
            this.checkBoxHorizAddInterval.UseVisualStyleBackColor = true;
            this.checkBoxHorizAddInterval.CheckedChanged += new System.EventHandler(this.checkBoxGenerateHorizontal_CheckedChanged);
            // 
            // DialogWindowWall
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(865, 625);
            this.Controls.Add(this.txtBoxHorizArmSection);
            this.Controls.Add(this.txtBoxVertArmSection);
            this.Controls.Add(this.checkBoxHorizAddInterval);
            this.Controls.Add(this.checkBoxGenerateHorizontal);
            this.Controls.Add(this.checkBoxGenerateVertical);
            this.Controls.Add(this.cmbVerticalType);
            this.Controls.Add(this.cmbHorizonalType);
            this.Controls.Add(this.txtRebarCover);
            this.Controls.Add(this.txtBottomOffset);
            this.Controls.Add(this.txtHorizontalInterval);
            this.Controls.Add(this.txtTopOffset);
            this.Controls.Add(this.txtVerticalFreeLength);
            this.Controls.Add(this.txtHorizontalFreeLength);
            this.Controls.Add(this.txtBackOffset);
            this.Controls.Add(this.txtVerticalInterval);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "DialogWindowWall";
            this.Text = "Параметры армирования  v2020.04.07";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtVerticalInterval;
        private System.Windows.Forms.TextBox txtRebarCover;
        private System.Windows.Forms.TextBox txtBackOffset;
        private System.Windows.Forms.TextBox txtHorizontalFreeLength;
        private System.Windows.Forms.TextBox txtVerticalFreeLength;
        private System.Windows.Forms.TextBox txtTopOffset;
        private System.Windows.Forms.TextBox txtHorizontalInterval;
        private System.Windows.Forms.TextBox txtBottomOffset;
        private System.Windows.Forms.ComboBox cmbHorizonalType;
        private System.Windows.Forms.ComboBox cmbVerticalType;
        private System.Windows.Forms.CheckBox checkBoxGenerateVertical;
        private System.Windows.Forms.CheckBox checkBoxGenerateHorizontal;
        private System.Windows.Forms.TextBox txtBoxVertArmSection;
        private System.Windows.Forms.TextBox txtBoxHorizArmSection;
        private System.Windows.Forms.CheckBox checkBoxHorizAddInterval;
    }
}