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
            button1 = new Button();
            label2 = new Label();
            rtbLinkGroup = new RichTextBox();
            lblUid = new Label();
            txtUidPassProxy2Fa = new TextBox();
            label4 = new Label();
            numericUpDown1 = new NumericUpDown();
            chkSaveLogin = new CheckBox();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            rtbContent = new RichTextBox();
            rtbDatePost = new RichTextBox();
            rtbTimePost = new RichTextBox();
            label1 = new Label();
            rtbPathFileImg = new RichTextBox();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            button1.Location = new Point(422, 509);
            button1.Name = "button1";
            button1.Size = new Size(181, 59);
            button1.TabIndex = 2;
            button1.Text = "Bắt đầu";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(18, 183);
            label2.Name = "label2";
            label2.Size = new Size(68, 15);
            label2.TabIndex = 3;
            label2.Text = "Link Group:";
            label2.Click += label2_Click;
            // 
            // rtbLinkGroup
            // 
            rtbLinkGroup.Location = new Point(22, 219);
            rtbLinkGroup.Name = "rtbLinkGroup";
            rtbLinkGroup.Size = new Size(308, 96);
            rtbLinkGroup.TabIndex = 4;
            rtbLinkGroup.Text = "";
            rtbLinkGroup.TextChanged += rtbLinkBaiCanGhim_TextChanged;
            // 
            // lblUid
            // 
            lblUid.AutoSize = true;
            lblUid.Location = new Point(22, 36);
            lblUid.Name = "lblUid";
            lblUid.Size = new Size(76, 15);
            lblUid.TabIndex = 0;
            lblUid.Text = "UID|Pass|2Fa:\r\n";
            lblUid.Click += label1_Click;
            // 
            // txtUidPassProxy2Fa
            // 
            txtUidPassProxy2Fa.Location = new Point(206, 37);
            txtUidPassProxy2Fa.Margin = new Padding(3, 2, 3, 2);
            txtUidPassProxy2Fa.Name = "txtUidPassProxy2Fa";
            txtUidPassProxy2Fa.Size = new Size(408, 23);
            txtUidPassProxy2Fa.TabIndex = 10;
            txtUidPassProxy2Fa.TextChanged += txtUidPassProxy2Fa_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 534);
            label4.Name = "label4";
            label4.Size = new Size(205, 15);
            label4.TabIndex = 18;
            label4.Text = "Thời gian nghỉ để chuyển link group: ";
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(239, 532);
            numericUpDown1.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(120, 23);
            numericUpDown1.TabIndex = 19;
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // chkSaveLogin
            // 
            chkSaveLogin.AutoSize = true;
            chkSaveLogin.Location = new Point(637, 38);
            chkSaveLogin.Name = "chkSaveLogin";
            chkSaveLogin.Size = new Size(130, 19);
            chkSaveLogin.TabIndex = 23;
            chkSaveLogin.Text = "Lưu nhớ đăng nhập";
            chkSaveLogin.UseVisualStyleBackColor = true;
            chkSaveLogin.CheckedChanged += cbSaveLogin_CheckedChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(382, 183);
            label6.Name = "label6";
            label6.Size = new Size(94, 15);
            label6.TabIndex = 27;
            label6.Text = "Content bài viết:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(18, 360);
            label7.Name = "label7";
            label7.Size = new Size(208, 15);
            label7.TabIndex = 28;
            label7.Text = "Ngày đăng bài viết(tháng/ngày/năm):";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(382, 360);
            label8.Name = "label8";
            label8.Size = new Size(370, 15);
            label8.TabIndex = 29;
            label8.Text = "Thời gian đăng bài viết(XX:XX + AM|PM(AM buổi sáng, PM buổi tối):";
            // 
            // rtbContent
            // 
            rtbContent.Location = new Point(382, 219);
            rtbContent.Name = "rtbContent";
            rtbContent.Size = new Size(308, 96);
            rtbContent.TabIndex = 30;
            rtbContent.Text = "";
            rtbContent.TextChanged += rtbContent_TextChanged;
            // 
            // rtbDatePost
            // 
            rtbDatePost.Location = new Point(18, 388);
            rtbDatePost.Name = "rtbDatePost";
            rtbDatePost.Size = new Size(308, 96);
            rtbDatePost.TabIndex = 31;
            rtbDatePost.Text = "";
            rtbDatePost.TextChanged += rtbDatePost_TextChanged;
            // 
            // rtbTimePost
            // 
            rtbTimePost.Location = new Point(382, 388);
            rtbTimePost.Name = "rtbTimePost";
            rtbTimePost.Size = new Size(308, 96);
            rtbTimePost.TabIndex = 32;
            rtbTimePost.Text = "";
            rtbTimePost.TextChanged += rtbTimePost_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(22, 78);
            label1.Name = "label1";
            label1.Size = new Size(167, 15);
            label1.TabIndex = 33;
            label1.Text = "Đường dẫn thư mục hình ảnh:";
            // 
            // rtbPathFileImg
            // 
            rtbPathFileImg.Location = new Point(206, 78);
            rtbPathFileImg.Name = "rtbPathFileImg";
            rtbPathFileImg.Size = new Size(408, 63);
            rtbPathFileImg.TabIndex = 34;
            rtbPathFileImg.Text = "";
            rtbPathFileImg.TextChanged += rtbPathFileImg_TextChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(787, 579);
            Controls.Add(rtbPathFileImg);
            Controls.Add(label1);
            Controls.Add(rtbTimePost);
            Controls.Add(rtbDatePost);
            Controls.Add(rtbContent);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(chkSaveLogin);
            Controls.Add(numericUpDown1);
            Controls.Add(label4);
            Controls.Add(txtUidPassProxy2Fa);
            Controls.Add(rtbLinkGroup);
            Controls.Add(label2);
            Controls.Add(button1);
            Controls.Add(lblUid);
            Name = "Form1";
            Text = "Auto Post Status Set TIme";
            FormClosing += Form1_FormClosing_1;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
        private Button button1;
        private Label label2;
        private RichTextBox rtbLinkGroup;
        private Label lblUid;
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