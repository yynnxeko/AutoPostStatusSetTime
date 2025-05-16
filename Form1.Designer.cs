namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.rtbLinkGroup = new System.Windows.Forms.RichTextBox();
            this.lblUid = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDuongDanThuMucProfiles = new System.Windows.Forms.TextBox();
            this.txtUidPassProxy2Fa = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.chkSaveLogin = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.rtbContent = new System.Windows.Forms.RichTextBox();
            this.rtbDatePost = new System.Windows.Forms.RichTextBox();
            this.rtbTimePost = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rtbPathFileImg = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button1.Location = new System.Drawing.Point(482, 679);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(207, 79);
            this.button1.TabIndex = 2;
            this.button1.Text = "Bắt đầu";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 244);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Link Group:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // rtbLinkGroup
            // 
            this.rtbLinkGroup.Location = new System.Drawing.Point(25, 292);
            this.rtbLinkGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rtbLinkGroup.Name = "rtbLinkGroup";
            this.rtbLinkGroup.Size = new System.Drawing.Size(351, 127);
            this.rtbLinkGroup.TabIndex = 4;
            this.rtbLinkGroup.Text = "";
            this.rtbLinkGroup.TextChanged += new System.EventHandler(this.rtbLinkBaiCanGhim_TextChanged);
            // 
            // lblUid
            // 
            this.lblUid.AutoSize = true;
            this.lblUid.Location = new System.Drawing.Point(25, 48);
            this.lblUid.Name = "lblUid";
            this.lblUid.Size = new System.Drawing.Size(94, 20);
            this.lblUid.TabIndex = 0;
            this.lblUid.Text = "UID|Pass|2Fa:\r\n";
            this.lblUid.Click += new System.EventHandler(this.label1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(199, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Đường dẫn thư mục profiles:";
            // 
            // txtDuongDanThuMucProfiles
            // 
            this.txtDuongDanThuMucProfiles.Location = new System.Drawing.Point(236, 105);
            this.txtDuongDanThuMucProfiles.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDuongDanThuMucProfiles.Name = "txtDuongDanThuMucProfiles";
            this.txtDuongDanThuMucProfiles.Size = new System.Drawing.Size(466, 27);
            this.txtDuongDanThuMucProfiles.TabIndex = 7;
            this.txtDuongDanThuMucProfiles.TextChanged += new System.EventHandler(this.txtDuongDanThuMucProfiles_TextChanged);
            // 
            // txtUidPassProxy2Fa
            // 
            this.txtUidPassProxy2Fa.Location = new System.Drawing.Point(236, 49);
            this.txtUidPassProxy2Fa.Name = "txtUidPassProxy2Fa";
            this.txtUidPassProxy2Fa.Size = new System.Drawing.Size(466, 27);
            this.txtUidPassProxy2Fa.TabIndex = 10;
            this.txtUidPassProxy2Fa.TextChanged += new System.EventHandler(this.txtUidPassProxy2Fa_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 712);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(253, 20);
            this.label4.TabIndex = 18;
            this.label4.Text = "Thời gian nghỉ để chuyển link group: ";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(273, 710);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(137, 27);
            this.numericUpDown1.TabIndex = 19;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // chkSaveLogin
            // 
            this.chkSaveLogin.AutoSize = true;
            this.chkSaveLogin.Location = new System.Drawing.Point(728, 51);
            this.chkSaveLogin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkSaveLogin.Name = "chkSaveLogin";
            this.chkSaveLogin.Size = new System.Drawing.Size(159, 24);
            this.chkSaveLogin.TabIndex = 23;
            this.chkSaveLogin.Text = "Lưu nhớ đăng nhập";
            this.chkSaveLogin.UseVisualStyleBackColor = true;
            this.chkSaveLogin.CheckedChanged += new System.EventHandler(this.cbSaveLogin_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(437, 244);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(117, 20);
            this.label6.TabIndex = 27;
            this.label6.Text = "Content bài viết:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 480);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(259, 20);
            this.label7.TabIndex = 28;
            this.label7.Text = "Ngày đăng bài viết(tháng/ngày/năm):";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(437, 480);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(461, 20);
            this.label8.TabIndex = 29;
            this.label8.Text = "Thời gian đăng bài viết(XX:XX + AM|PM(AM buổi sáng, PM buổi tối):";
            // 
            // rtbContent
            // 
            this.rtbContent.Location = new System.Drawing.Point(437, 292);
            this.rtbContent.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rtbContent.Name = "rtbContent";
            this.rtbContent.Size = new System.Drawing.Size(351, 127);
            this.rtbContent.TabIndex = 30;
            this.rtbContent.Text = "";
            this.rtbContent.TextChanged += new System.EventHandler(this.rtbContent_TextChanged);
            // 
            // rtbDatePost
            // 
            this.rtbDatePost.Location = new System.Drawing.Point(21, 517);
            this.rtbDatePost.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rtbDatePost.Name = "rtbDatePost";
            this.rtbDatePost.Size = new System.Drawing.Size(351, 127);
            this.rtbDatePost.TabIndex = 31;
            this.rtbDatePost.Text = "";
            this.rtbDatePost.TextChanged += new System.EventHandler(this.rtbDatePost_TextChanged);
            // 
            // rtbTimePost
            // 
            this.rtbTimePost.Location = new System.Drawing.Point(437, 517);
            this.rtbTimePost.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rtbTimePost.Name = "rtbTimePost";
            this.rtbTimePost.Size = new System.Drawing.Size(351, 127);
            this.rtbTimePost.TabIndex = 32;
            this.rtbTimePost.Text = "";
            this.rtbTimePost.TextChanged += new System.EventHandler(this.rtbTimePost_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 169);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(205, 20);
            this.label1.TabIndex = 33;
            this.label1.Text = "Đường dẫn thư mục hình ảnh:";
            // 
            // rtbPathFileImg
            // 
            this.rtbPathFileImg.Location = new System.Drawing.Point(236, 157);
            this.rtbPathFileImg.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rtbPathFileImg.Name = "rtbPathFileImg";
            this.rtbPathFileImg.Size = new System.Drawing.Size(466, 83);
            this.rtbPathFileImg.TabIndex = 34;
            this.rtbPathFileImg.Text = "";
            this.rtbPathFileImg.TextChanged += new System.EventHandler(this.rtbPathFileImg_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 772);
            this.Controls.Add(this.rtbPathFileImg);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtbTimePost);
            this.Controls.Add(this.rtbDatePost);
            this.Controls.Add(this.rtbContent);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.chkSaveLogin);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtUidPassProxy2Fa);
            this.Controls.Add(this.txtDuongDanThuMucProfiles);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rtbLinkGroup);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblUid);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "Auto Pin Status";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing_1);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button button1;
        private Label label2;
        private RichTextBox rtbLinkGroup;
        private Label lblUid;
        private Label label3;
        private TextBox txtDuongDanThuMucProfiles;
        private TextBox txtUidPassProxy2Fa;
        private Label label4;
        private NumericUpDown numericUpDown1;
        private CheckBox chkSaveLogin;
        private Label label6;
        private Label label7;
        private Label label8;
        private RichTextBox rtbContent;
        private RichTextBox rtbDatePost;
        private RichTextBox rtbTimePost;
        private Label label1;
        private RichTextBox rtbPathFileImg;
    }
}