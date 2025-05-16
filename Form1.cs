using Microsoft.VisualBasic.ApplicationServices;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using Selenium.Extensions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using WinFormsApp1.Helpers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Keys = OpenQA.Selenium.Keys;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Google.Authenticator;
using OtpNet;
using Base32Encoding = Google.Authenticator.Base32Encoding;
using System.Net;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            chkSaveLogin.Checked = Properties.Settings.Default.isChecked;

        }

        ChromeHepler chromeHepler = null;
        public static string GenerateTOTP(string base32Secret)
        {
            // Convert base32 secret to byte array
            var bytes = Base32Encoding.ToBytes(base32Secret);
            var totp = new Totp(bytes);
            return totp.ComputeTotp(); // trả về mã 6 số hiện tại
        }

        private async void button1_Click(object sender, EventArgs e)
        {

            int giaTri = 0;
            string[] danhSachLinkGroup = rtbLinkGroup.Text.Split("\n");
            string[] danhSachContent = rtbContent.Text.Split("|");



            string[] danhSachNgay = rtbDatePost.Text.Split("\n");
            string[] danhSachThoiGian = rtbTimePost.Text.Split("\n");
            string[] danhSachAnh = rtbPathFileImg.Text.Split("\n");
            string[] cookie = txtUidPassProxy2Fa.Text.Split("\n");
            string[] accountInfo = txtUidPassProxy2Fa.Text.Split("|");
            string profileName = "Profile Dung";
            string pathProfile = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + profileName;
            ChromeHepler chromeHepler = new ChromeHepler();
            string uid = accountInfo[0];
            string userName = accountInfo[0];
            string password = accountInfo[1];
            string ma2FaChu = accountInfo[2];
            int delayInSecondsChangeLink = (int)numericUpDown1.Value;
            Thread.Sleep(5000);
            pathProfile = pathProfile + "\\" + uid;
            if (await chromeHepler.OpenDriver(pathProfile, isShowImage: true, 700, 700) == null)
            {
                MessageBox.Show(chromeHepler.ErrorMessage);
                return;
            }
            chromeHepler.GoToUrl("https://facebook.com");
            chromeHepler.AddCookie("https://facebook.com", cookie[0]);
            chromeHepler.GoToUrl("https://facebook.com");
            Thread.Sleep(2500);

            Thread.Sleep(2500);
            bool checkLogin = chromeHepler.FindElement(By.Id("email")) != null;            
            if (checkLogin == true)
            {
                string ma2Fa = GenerateTOTP(ma2FaChu);
                string source = chromeHepler.chrome.PageSource;
                bool isSendkeySuccess = chromeHepler.SendKeys(By.Id("email"), userName);
                chromeHepler.Delay(3, 8);
                isSendkeySuccess = chromeHepler.SendKeys(By.Id("pass"), password);
                chromeHepler.Delay(3, 8);
                bool isClickSuccess = chromeHepler.Click(By.Name("login"));
                chromeHepler.Delay(3, 8);
                bool checkCheckpoint = chromeHepler.FindElementExist(By.XPath("//span[text()=\"Try Another Way\"]"), 2);
                bool checkCheckpointVn = chromeHepler.FindElementExist(By.XPath("//span[text()=\"Thử cách khác\"]"), 2);
                if (checkCheckpoint)
                {
                    chromeHepler.Delay(3, 8);
                    chromeHepler.Click(By.XPath("//span[text()=\"Try Another Way\"]"));
                    chromeHepler.Delay(3, 8);
                    chromeHepler.Click(By.XPath("//div[text()=\"Authentication app\"]"));
                    chromeHepler.Delay(3, 8);
                    chromeHepler.Click(By.XPath("//span[text()=\"Continue\"]"));
                    chromeHepler.Delay(3, 8);
                    chromeHepler.SendKeys(chromeHepler.chrome, By.XPath("//input[@type=\"text\" and @tabindex=\"0\"]"), ma2Fa);
                    chromeHepler.Delay(3, 8);
                    chromeHepler.Click(By.XPath("//span[text()=\"Continue\"]"));
                    chromeHepler.Delay(3, 8);
                }
                else if (checkCheckpointVn)
                {
                    chromeHepler.Delay(3, 8);
                    chromeHepler.Click(By.XPath("//span[text()=\"Thử cách khác\"]"));
                    chromeHepler.Delay(3, 8);
                    chromeHepler.Click(By.XPath("//div[text()='Ứng dụng xác thực']"));
                    chromeHepler.Delay(3, 8);
                    chromeHepler.Click(By.XPath("//span[text()=\"Tiếp tục\"]"));
                    chromeHepler.Delay(3, 8);
                    chromeHepler.SendKeys(chromeHepler.chrome, By.XPath("//input[@type=\"text\" and @tabindex=\"0\"]"), ma2Fa);
                    chromeHepler.Delay(3, 8);
                    chromeHepler.Click(By.XPath("//span[text()=\"Tiếp tục\"]"));
                    chromeHepler.Delay(3, 8);
                }
            }

            chromeHepler.Delay(1, 5);
            chromeHepler.GoToUrl("https://www.facebook.com/settings/?tab=language");
            var checkLanguage = chromeHepler.FindElementExist(By.XPath("//span[text()='English (US)']"), 2);
            if (!checkLanguage)
            {
                chromeHepler.Click(By.XPath("//span[text()='Tiếng Việt']"));
                chromeHepler.Delay(1, 5);
                chromeHepler.Click(By.XPath("//span[text()='English (US)']"));
                chromeHepler.Delay(1, 5);

            }
            chromeHepler.Delay(1, 5);

            //Vào link từng group 
            for (int i = 0; i < danhSachLinkGroup.Length; i++)
            {
                if (danhSachLinkGroup[i] == "")
                {
                    chromeHepler.Delay(1, 5);
                    chromeHepler.Quit();
                    return;
                }
                else
                {
                    chromeHepler.Delay(3, 8);
                    chromeHepler.GoToUrl(danhSachLinkGroup[i]);
                    chromeHepler.Delay(3, 8);

                    var checkMenu = chromeHepler.FindElementExist(By.XPath("//span[text()='Write something...']"), 2);
                    if (checkMenu)
                    {
                        chromeHepler.Click(By.XPath("//span[text()='Write something...']"));
                    }
                    chromeHepler.Delay(3, 8);                   
                    chromeHepler.SendKeys(chromeHepler.chrome, By.XPath("//div[@aria-placeholder='Create a public post…']"), danhSachContent[i]);
                    chromeHepler.Delay(3, 8);
                    chromeHepler.Click(By.XPath("//div[@aria-label='Photo/video']"));
                    chromeHepler.Delay(3, 8);
                    string folderPath = danhSachAnh[i];
                    // Các định dạng ảnh hợp lệ
                    string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff" };

                    if (folderPath.Length > 0)
                    {
                        var images = Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly)
                       .Where(file => imageExtensions.Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase))
                       .ToList();
                        if (images.Count > 0)
                        {
                            var elements = chromeHepler.chrome.FindElements(By.XPath("//input[@type='file' and @multiple]"));
                            elements.Last().SendKeys(string.Join("\n", images));
                        }
                    }

                    var element = chromeHepler.FindElementExist(By.XPath("//i[contains(@style, 'background-image') and contains(@style, 'l7vZatYS2lP.png')]"),2);
                    if (element)
                    {
                        chromeHepler.Click(By.XPath("//i[contains(@style, 'background-image') and contains(@style, 'l7vZatYS2lP.png')]"));
                    }
                    chromeHepler.Delay(3, 9);                                        
                    chromeHepler.Delay(3, 9);
                    var chooseDate = chromeHepler.FindElement(By.XPath("//span[text()='Date' and @aria-label]"));
                    if (chooseDate != null)
                    {
                        ((IJavaScriptExecutor)chromeHepler.chrome).ExecuteScript("arguments[0].click();", chooseDate);
                    }
                    chromeHepler.Delay(3, 9);
                    
                    chromeHepler.SendKeys(By.XPath("//input[@role='combobox' and @aria-autocomplete='none']"), danhSachNgay[i]);
                    chromeHepler.Delay(3, 9);
                    var chooseTime = chromeHepler.FindElement(By.XPath("//span[text()='Time' and @aria-label]"));
                    if (chooseTime != null)
                    {
                        ((IJavaScriptExecutor)chromeHepler.chrome).ExecuteScript("arguments[0].click();", chooseTime);
                    }
                    chromeHepler.Delay(3, 9);
                    var timeInput = chromeHepler.FindElement(By.XPath("//input[@role='combobox' and @placeholder='00:00']"));
                    if (timeInput != null)
                    {
                        timeInput.SendKeys(danhSachThoiGian[i]);
                        chromeHepler.Delay(3, 9);
                        timeInput.SendKeys(Keys.Enter);
                    }
                    chromeHepler.Delay(5, 9);
                    chromeHepler.Click(By.XPath("//span[text()='Schedule']"));
                    chromeHepler.Delay(15, 20);

                    Thread.Sleep(delayInSecondsChangeLink * 1000);

                }
            }
            chromeHepler.Quit();
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtProxy_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            chromeHepler.Quit();
        }
        private void buttonGetTime_Click(object sender, EventArgs e)
        {
            // Lấy giá trị từ NumericUdpDown
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;

        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {


            // Chờ trong số giây đã nhập

        }

        private void txtUidPassProxy2Fa_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText("savedLoginInput.txt", txtUidPassProxy2Fa.Text.Trim());
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }


        public string checkFileExist(string nameFile, string inputText)
        {
            if (chkSaveLogin.Checked)
            {

                if (!string.IsNullOrEmpty(inputText))
                {
                    File.WriteAllText(nameFile, inputText);
                    return inputText;
                }
                // Đọc nội dung từ tệp "savedInput.txt"
                string inputFromFile = File.ReadAllText(nameFile);
                return inputFromFile;
            }
            return string.Empty;
        }
        private void cbSaveLogin_CheckedChanged(object sender, EventArgs e)
        {
            chkSaveLogin.CheckedChanged += cbSaveLogin_CheckedChanged;
            Properties.Settings.Default.isChecked = chkSaveLogin.Checked;
            Properties.Settings.Default.Save();
            txtUidPassProxy2Fa.Text = checkFileExist("savedLoginInput.txt", txtUidPassProxy2Fa.Text.Trim());
            txtDuongDanThuMucProfiles.Text = checkFileExist("savedProfileLink.txt", txtDuongDanThuMucProfiles.Text.Trim());
            rtbPathFileImg.Text = checkFileExist("savedPathFileImg.txt", rtbPathFileImg.Text.Trim());
            rtbLinkGroup.Text = checkFileExist("savedLinkBaiVietCanGhim.txt", rtbLinkGroup.Text.Trim());
            rtbContent.Text = checkFileExist("savedContent.txt", rtbContent.Text.Trim());
            rtbDatePost.Text = checkFileExist("savedDatePost.txt", rtbDatePost.Text.Trim());
            rtbTimePost.Text = checkFileExist("savedTimePost.txt", rtbTimePost.Text.Trim());
        }

        private void chkSaveLinkProfile_CheckedChanged(object sender, EventArgs e)
        {


        }

        private void txtDuongDanThuMucProfiles_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText("savedProfileLink.txt", txtDuongDanThuMucProfiles.Text.Trim());
        }

        private void rtbLinkBaiCanGhim_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText("savedLinkBaiVietCanGhim.txt", rtbLinkGroup.Text.Trim());
        }

        private void rtbPathFileImg_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText("savedPathFileImg.txt", rtbPathFileImg.Text.Trim());
        }

        private void rtbContent_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText("savedContent.txt", rtbContent.Text.Trim());
        }

        private void rtbDatePost_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText("savedDatePost.txt", rtbDatePost.Text.Trim());
        }

        private void rtbTimePost_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText("savedTimePost.txt", rtbTimePost.Text.Trim());
        }
    }
}