using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Authenticator;
using OtpNet;
using Base32Encoding = Google.Authenticator.Base32Encoding;
using WinFormsApp1.Helpers;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private FacebookPostService? _postService;
        private CancellationTokenSource? _cts;
        private bool _isInitializing = true;

        public Form1()
        {
            InitializeComponent();
        }

        private string GetConfigPath(string fileName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _isInitializing = true;

            SetupDataGridView();
            CreateRequiredFiles();
            
            // Tải cấu hình cũ nếu có
            if (File.Exists(GetConfigPath("savedLoginInput.txt")) ||
                File.Exists(GetConfigPath("savedPathFileImg.txt")) ||
                File.Exists(GetConfigPath("savedLinkBaiVietCanGhim.txt")) ||
                File.Exists(GetConfigPath("savedContent.txt")) ||
                File.Exists(GetConfigPath("savedDatePost.txt")) ||
                File.Exists(GetConfigPath("savedTimePost.txt")))
            {
                LoadConfigurations();
            }

            chkSaveLogin.Checked = Properties.Settings.Default.isChecked;

            _isInitializing = false;
        }

        private void SetupDataGridView()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("Index", "STT");
            dataGridView1.Columns.Add("GroupUrl", "Nhóm");
            dataGridView1.Columns.Add("PostUrl", "PostKey");
            dataGridView1.Columns.Add("Content", "Nội dung");
            dataGridView1.Columns.Add("Status", "Trạng thái");
            dataGridView1.Columns.Add("ErrorDetail", "Chi tiết");

            dataGridView1.Columns["PostUrl"].Visible = false;

            dataGridView1.Columns["Index"].FillWeight = 10;
            dataGridView1.Columns["GroupUrl"].FillWeight = 30;
            dataGridView1.Columns["Content"].FillWeight = 30;
            dataGridView1.Columns["Status"].FillWeight = 15;
            dataGridView1.Columns["ErrorDetail"].FillWeight = 15;
        }

        private void CreateRequiredFiles()
        {
            string[] requiredFiles = new string[]
            {
                "savedLoginInput.txt",
                "savedPathFileImg.txt",
                "savedLinkBaiVietCanGhim.txt",
                "savedContent.txt",
                "savedDatePost.txt",
                "savedTimePost.txt"
            };

            foreach (string file in requiredFiles)
            {
                try
                {
                    string path = GetConfigPath(file);
                    if (!File.Exists(path))
                    {
                        File.WriteAllText(path, string.Empty);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không thể tạo file {file}: {ex.Message}");
                }
            }
        }

        private void LoadConfigurations()
        {
            try
            {
                string loginPath = GetConfigPath("savedLoginInput.txt");
                string imgPath = GetConfigPath("savedPathFileImg.txt");
                string linkPath = GetConfigPath("savedLinkBaiVietCanGhim.txt");
                string contentPath = GetConfigPath("savedContent.txt");
                string datePath = GetConfigPath("savedDatePost.txt");
                string timePath = GetConfigPath("savedTimePost.txt");

                if (File.Exists(loginPath)) txtUidPassProxy2Fa.Text = File.ReadAllText(loginPath).Trim();
                if (File.Exists(imgPath)) rtbPathFileImg.Text = File.ReadAllText(imgPath).Trim();
                if (File.Exists(linkPath)) rtbLinkGroup.Text = File.ReadAllText(linkPath).Trim();
                if (File.Exists(contentPath)) rtbContent.Text = File.ReadAllText(contentPath).Trim();
                if (File.Exists(datePath)) rtbDatePost.Text = File.ReadAllText(datePath).Trim();
                if (File.Exists(timePath)) rtbTimePost.Text = File.ReadAllText(timePath).Trim();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi load cấu hình: {ex.Message}");
            }
        }

        private void SaveConfigurations()
        {
            try
            {
                File.WriteAllText(GetConfigPath("savedLoginInput.txt"), txtUidPassProxy2Fa.Text.Trim());
                File.WriteAllText(GetConfigPath("savedPathFileImg.txt"), rtbPathFileImg.Text.Trim());
                File.WriteAllText(GetConfigPath("savedLinkBaiVietCanGhim.txt"), rtbLinkGroup.Text.Trim());
                File.WriteAllText(GetConfigPath("savedContent.txt"), rtbContent.Text.Trim());
                File.WriteAllText(GetConfigPath("savedDatePost.txt"), rtbDatePost.Text.Trim());
                File.WriteAllText(GetConfigPath("savedTimePost.txt"), rtbTimePost.Text.Trim());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi lưu cấu hình: {ex.Message}");
            }
        }

        private void DeleteConfigurations()
        {
            try
            {
                string[] files = { "savedLoginInput.txt", "savedPathFileImg.txt", "savedLinkBaiVietCanGhim.txt", "savedContent.txt", "savedDatePost.txt", "savedTimePost.txt" };
                foreach (var file in files)
                {
                    string path = GetConfigPath(file);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi xóa cấu hình: {ex.Message}");
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (_cts != null)
            {
                MessageBox.Show("Tác vụ đang chạy. Vui lòng đợi hoặc hủy bỏ trước khi chạy lại.");
                return;
            }

            string inputAccount = txtUidPassProxy2Fa.Text.Trim();
            if (string.IsNullOrEmpty(inputAccount))
            {
                MessageBox.Show("Vui lòng điền thông tin tài khoản (UID|Pass|2FA)!");
                return;
            }

            string[] accountInfo = inputAccount.Split('|');
            if (accountInfo.Length < 3)
            {
                MessageBox.Show("Định dạng tài khoản không hợp lệ. Vui lòng nhập dạng: UID|Pass|2Fa");
                return;
            }

            string uid = accountInfo[0].Trim();
            string password = accountInfo[1].Trim();
            string ma2FaChu = accountInfo[2].Trim();

            string[] danhSachLinkGroup = rtbLinkGroup.Text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            string[] danhSachContent = rtbContent.Text.Split('|');
            string[] danhSachNgay = rtbDatePost.Text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            string[] danhSachThoiGian = rtbTimePost.Text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            string[] danhSachAnh = rtbPathFileImg.Text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            if (danhSachLinkGroup.Length == 0)
            {
                MessageBox.Show("Vui lòng điền danh sách Link Group!");
                return;
            }

            // Tạo danh sách Task
            var tasks = new List<PostTask>();
            for (int i = 0; i < danhSachLinkGroup.Length; i++)
            {
                string groupUrl = danhSachLinkGroup[i].Trim();
                string content = (i < danhSachContent.Length) ? danhSachContent[i] : (danhSachContent.Length > 0 ? danhSachContent[0] : "");
                string date = (i < danhSachNgay.Length) ? danhSachNgay[i].Trim() : (danhSachNgay.Length > 0 ? danhSachNgay[0].Trim() : "");
                string time = (i < danhSachThoiGian.Length) ? danhSachThoiGian[i].Trim() : (danhSachThoiGian.Length > 0 ? danhSachThoiGian[0].Trim() : "");
                string imgFolder = (i < danhSachAnh.Length) ? danhSachAnh[i].Trim() : (danhSachAnh.Length > 0 ? danhSachAnh[0].Trim() : "");

                tasks.Add(new PostTask(groupUrl, content, date, time, imgFolder));
            }

            int delayInSecondsChangeLink = (int)numericUpDown1.Value;

            // Cập nhật cấu hình lưu đăng nhập
            if (chkSaveLogin.Checked)
            {
                SaveConfigurations();
            }

            // Reset UI
            button1.Enabled = false;
            btnCancel.Enabled = true;
            rtbLogs.Clear();
            dataGridView1.Rows.Clear();

            _cts = new CancellationTokenSource();
            _postService = new FacebookPostService();

            // Lắng nghe logs từ service
            _postService.OnLog += (logMsg) =>
            {
                if (rtbLogs.IsDisposed) return;
                this.BeginInvoke(() =>
                {
                    rtbLogs.AppendText(logMsg + Environment.NewLine);
                    rtbLogs.SelectionStart = rtbLogs.Text.Length;
                    rtbLogs.ScrollToCaret();
                });
            };

            // Lắng nghe quét danh sách
            _postService.OnPostScanned += (info) =>
            {
                if (dataGridView1.IsDisposed) return;
                this.BeginInvoke(() =>
                {
                    dataGridView1.Rows.Add(info.Index, info.GroupUrl, info.PostUrl, info.ContentSnippet, info.Status, info.ErrorDetail);
                });
            };

            // Lắng nghe thay đổi trạng thái
            _postService.OnPostStatusChanged += (postUrl, status, errorDetail) =>
            {
                if (dataGridView1.IsDisposed) return;
                this.BeginInvoke(() =>
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells["PostUrl"].Value?.ToString() == postUrl)
                        {
                            row.Cells["Status"].Value = status;
                            row.Cells["ErrorDetail"].Value = errorDetail;
                            break;
                        }
                    }
                });
            };

            string pathProfile = GetProfilePath(uid);
            bool headless = chkHeadless.Checked;

            try
            {
                var token = _cts.Token;
                await Task.Run(async () =>
                {
                    await _postService.RunPostTaskAsync(
                        tasks,
                        uid,
                        password,
                        ma2FaChu,
                        pathProfile,
                        headless,
                        delayInSecondsChangeLink,
                        token
                    );
                });

                MessageBox.Show("Hoàn thành tác vụ lên lịch đăng bài!");
            }
            catch (OperationCanceledException)
            {
                rtbLogs.AppendText("[HỆ THỐNG] Tác vụ đã bị người dùng dừng lại." + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}");
            }
            finally
            {
                _postService = null;
                _cts = null;
                button1.Enabled = true;
                btnCancel.Enabled = false;
                button1.Text = "🚀 Bắt đầu";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_cts != null)
            {
                rtbLogs.AppendText("[HỆ THỐNG] Đang yêu cầu dừng tiến trình..." + Environment.NewLine);
                _cts.Cancel();
                btnCancel.Enabled = false;
            }
        }

        private string GetProfilePath(string uid)
        {
            string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string profilesFolder = Path.Combine(projectDirectory, "Profiles");
            if (!Directory.Exists(profilesFolder))
            {
                Directory.CreateDirectory(profilesFolder);
            }

            string profilePath = Path.Combine(profilesFolder, uid);
            if (!Directory.Exists(profilePath))
            {
                Directory.CreateDirectory(profilePath);
            }

            return profilePath;
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void txtProxy_TextChanged(object sender, EventArgs e) { }

        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_postService != null)
            {
                await _postService.CloseBrowserAsync();
            }
        }

        private void buttonGetTime_Click(object sender, EventArgs e)
        {
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e) { }

        private void txtUidPassProxy2Fa_TextChanged(object sender, EventArgs e)
        {
            if (_isInitializing) return;
            if (chkSaveLogin.Checked) SaveConfigurations();
        }

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            if (_postService != null)
            {
                Task.Run(async () => await _postService.CloseBrowserAsync()).Wait();
            }
            Application.Exit();
        }

        private void cbSaveLogin_CheckedChanged(object sender, EventArgs e)
        {
            if (_isInitializing) return;

            Properties.Settings.Default.isChecked = chkSaveLogin.Checked;
            Properties.Settings.Default.Save();

            if (chkSaveLogin.Checked)
            {
                SaveConfigurations();
            }
            else
            {
                DeleteConfigurations();
            }
        }

        private void chkSaveLinkProfile_CheckedChanged(object sender, EventArgs e) { }
        private void txtDuongDanThuMucProfiles_TextChanged(object sender, EventArgs e) { }

        private void rtbLinkBaiCanGhim_TextChanged(object sender, EventArgs e)
        {
            if (_isInitializing) return;
            if (chkSaveLogin.Checked) SaveConfigurations();
        }

        private void rtbPathFileImg_TextChanged(object sender, EventArgs e)
        {
            if (_isInitializing) return;
            if (chkSaveLogin.Checked) SaveConfigurations();
        }

        private void rtbContent_TextChanged(object sender, EventArgs e)
        {
            if (_isInitializing) return;
            if (chkSaveLogin.Checked) SaveConfigurations();
        }

        private void rtbDatePost_TextChanged(object sender, EventArgs e)
        {
            if (_isInitializing) return;
            if (chkSaveLogin.Checked) SaveConfigurations();
        }

        private void rtbTimePost_TextChanged(object sender, EventArgs e)
        {
            if (_isInitializing) return;
            if (chkSaveLogin.Checked) SaveConfigurations();
        }

        private void label3_Click(object sender, EventArgs e) { }
    }
}