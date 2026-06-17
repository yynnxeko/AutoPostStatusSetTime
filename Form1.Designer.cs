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
            btnCancel = new Button();
            label2 = new Label();
            rtbLinkGroup = new RichTextBox();
            lblUid = new Label();
            txtUidPassProxy2Fa = new TextBox();
            label4 = new Label();
            numericUpDown1 = new NumericUpDown();
            chkSaveLogin = new CheckBox();
            chkHeadless = new CheckBox();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            rtbContent = new RichTextBox();
            rtbDatePost = new RichTextBox();
            rtbTimePost = new RichTextBox();
            label1 = new Label();
            rtbPathFileImg = new RichTextBox();
            dataGridView1 = new DataGridView();
            labelLogs = new Label();
            rtbLogs = new RichTextBox();
            labelGrid = new Label();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackColor = Color.DodgerBlue;
            button1.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            button1.ForeColor = Color.White;
            button1.Location = new Point(15, 510);
            button1.Name = "button1";
            button1.Size = new Size(260, 50);
            button1.TabIndex = 2;
            button1.Text = "🚀 Bắt đầu";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.Crimson;
            btnCancel.Enabled = false;
            btnCancel.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(300, 510);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(270, 50);
            btnCancel.TabIndex = 33;
            btnCancel.Text = "🛑 Dừng lại";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(15, 120);
            label2.Name = "label2";
            label2.Size = new Size(73, 15);
            label2.TabIndex = 3;
            label2.Text = "Link Group:";
            // 
            // rtbLinkGroup
            // 
            rtbLinkGroup.Location = new Point(15, 145);
            rtbLinkGroup.Name = "rtbLinkGroup";
            rtbLinkGroup.Size = new Size(270, 130);
            rtbLinkGroup.TabIndex = 4;
            rtbLinkGroup.Text = "";
            rtbLinkGroup.TextChanged += rtbLinkBaiCanGhim_TextChanged;
            // 
            // lblUid
            // 
            lblUid.AutoSize = true;
            lblUid.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblUid.Location = new Point(15, 20);
            lblUid.Name = "lblUid";
            lblUid.Size = new Size(119, 15);
            lblUid.TabIndex = 0;
            lblUid.Text = "UID|Pass|Secret 2Fa:";
            // 
            // txtUidPassProxy2Fa
            // 
            txtUidPassProxy2Fa.Location = new Point(170, 17);
            txtUidPassProxy2Fa.Name = "txtUidPassProxy2Fa";
            txtUidPassProxy2Fa.Size = new Size(330, 23);
            txtUidPassProxy2Fa.TabIndex = 10;
            txtUidPassProxy2Fa.TextChanged += txtUidPassProxy2Fa_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label4.Location = new Point(15, 465);
            label4.Name = "label4";
            label4.Size = new Size(211, 15);
            label4.TabIndex = 18;
            label4.Text = "Thời gian nghỉ chuyển link (giây):";
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(250, 463);
            numericUpDown1.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(100, 23);
            numericUpDown1.TabIndex = 19;
            numericUpDown1.Value = new decimal(new int[] { 30, 0, 0, 0 });
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // chkSaveLogin
            // 
            chkSaveLogin.AutoSize = true;
            chkSaveLogin.Location = new Point(515, 19);
            chkSaveLogin.Name = "chkSaveLogin";
            chkSaveLogin.Size = new Size(130, 19);
            chkSaveLogin.TabIndex = 23;
            chkSaveLogin.Text = "Lưu nhớ đăng nhập";
            chkSaveLogin.UseVisualStyleBackColor = true;
            chkSaveLogin.CheckedChanged += cbSaveLogin_CheckedChanged;
            // 
            // chkHeadless
            // 
            chkHeadless.AutoSize = true;
            chkHeadless.Location = new Point(370, 464);
            chkHeadless.Name = "chkHeadless";
            chkHeadless.Size = new Size(190, 19);
            chkHeadless.TabIndex = 34;
            chkHeadless.Text = "Chạy ngầm (Headless Browser)";
            chkHeadless.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label6.Location = new Point(300, 120);
            label6.Name = "label6";
            label6.Size = new Size(98, 15);
            label6.TabIndex = 27;
            label6.Text = "Content bài viết:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label7.Location = new Point(15, 290);
            label7.Name = "label7";
            label7.Size = new Size(207, 15);
            label7.TabIndex = 28;
            label7.Text = "Ngày đăng bài viết (tháng/ngày/năm):";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label8.Location = new Point(300, 290);
            label8.Name = "label8";
            label8.Size = new Size(244, 15);
            label8.TabIndex = 29;
            label8.Text = "Thời gian đăng bài viết (XX:XX AM/PM):";
            // 
            // rtbContent
            // 
            rtbContent.Location = new Point(300, 145);
            rtbContent.Name = "rtbContent";
            rtbContent.Size = new Size(270, 130);
            rtbContent.TabIndex = 30;
            rtbContent.Text = "";
            rtbContent.TextChanged += rtbContent_TextChanged;
            // 
            // rtbDatePost
            // 
            rtbDatePost.Location = new Point(15, 315);
            rtbDatePost.Name = "rtbDatePost";
            rtbDatePost.Size = new Size(270, 130);
            rtbDatePost.TabIndex = 31;
            rtbDatePost.Text = "";
            rtbDatePost.TextChanged += rtbDatePost_TextChanged;
            // 
            // rtbTimePost
            // 
            rtbTimePost.Location = new Point(300, 315);
            rtbTimePost.Name = "rtbTimePost";
            rtbTimePost.Size = new Size(270, 130);
            rtbTimePost.TabIndex = 32;
            rtbTimePost.Text = "";
            rtbTimePost.TextChanged += rtbTimePost_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(15, 60);
            label1.Name = "label1";
            label1.Size = new Size(168, 15);
            label1.TabIndex = 33;
            label1.Text = "Đường dẫn thư mục hình ảnh:";
            // 
            // rtbPathFileImg
            // 
            rtbPathFileImg.Location = new Point(200, 57);
            rtbPathFileImg.Name = "rtbPathFileImg";
            rtbPathFileImg.Size = new Size(300, 45);
            rtbPathFileImg.TabIndex = 34;
            rtbPathFileImg.Text = "";
            rtbPathFileImg.TextChanged += rtbPathFileImg_TextChanged;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(600, 50);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(610, 420);
            dataGridView1.TabIndex = 35;
            // 
            // labelLogs
            // 
            labelLogs.AutoSize = true;
            labelLogs.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelLogs.Location = new Point(600, 490);
            labelLogs.Name = "labelLogs";
            labelLogs.Size = new Size(116, 15);
            labelLogs.TabIndex = 36;
            labelLogs.Text = "Nhật ký chạy (Logs):";
            // 
            // rtbLogs
            // 
            rtbLogs.BackColor = Color.Black;
            rtbLogs.ForeColor = Color.LimeGreen;
            rtbLogs.Location = new Point(600, 515);
            rtbLogs.Name = "rtbLogs";
            rtbLogs.ReadOnly = true;
            rtbLogs.Size = new Size(610, 230);
            rtbLogs.TabIndex = 37;
            rtbLogs.Text = "";
            // 
            // labelGrid
            // 
            labelGrid.AutoSize = true;
            labelGrid.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            labelGrid.ForeColor = Color.DodgerBlue;
            labelGrid.Location = new Point(600, 20);
            labelGrid.Name = "labelGrid";
            labelGrid.Size = new Size(393, 17);
            labelGrid.TabIndex = 38;
            labelGrid.Text = "📊 BẢNG ĐỐI SOÁT TRẠNG THÁI LÊN LỊCH ĐĂNG BÀI (GRID):";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1230, 770);
            Controls.Add(labelGrid);
            Controls.Add(rtbLogs);
            Controls.Add(labelLogs);
            Controls.Add(dataGridView1);
            Controls.Add(rtbPathFileImg);
            Controls.Add(label1);
            Controls.Add(rtbTimePost);
            Controls.Add(rtbDatePost);
            Controls.Add(rtbContent);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(chkHeadless);
            Controls.Add(btnCancel);
            Controls.Add(chkSaveLogin);
            Controls.Add(numericUpDown1);
            Controls.Add(label4);
            Controls.Add(txtUidPassProxy2Fa);
            Controls.Add(rtbLinkGroup);
            Controls.Add(label2);
            Controls.Add(button1);
            Controls.Add(lblUid);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "Form1";
            Text = "Auto Post Status Set Time - Playwright Engine";
            FormClosing += Form1_FormClosing_1;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button1;
        private Button btnCancel;
        private Label label2;
        private RichTextBox rtbLinkGroup;
        private Label lblUid;
        private TextBox txtUidPassProxy2Fa;
        private Label label4;
        private NumericUpDown numericUpDown1;
        private CheckBox chkSaveLogin;
        private CheckBox chkHeadless;
        private Label label6;
        private Label label7;
        private Label label8;
        private RichTextBox rtbContent;
        private RichTextBox rtbDatePost;
        private RichTextBox rtbTimePost;
        private Label label1;
        private RichTextBox rtbPathFileImg;
        private DataGridView dataGridView1;
        private Label labelLogs;
        private RichTextBox rtbLogs;
        private Label labelGrid;
    }
}