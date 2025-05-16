using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chrome.ChromeDriverExtensions;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Helpers
{
    public class ChromeHepler
    {
        public string ErrorMessage = "";

        public ChromeDriver chrome;

        private ChromeOptions options;

        private ChromeResult chromeResult = null;

        private IWebDriver driver;

        public void Delay(int min, int max)
        {
            Random random = new Random();

            Thread.Sleep(random.Next(min, max) * 1000);

        }
        public void Delay(int time)
        {
            Thread.Sleep(time * 1000);

        }

        public ChromeHepler(ChromeOptions options)
        {
            this.options = options;
        }

        public void setChromeResult(ChromeResult chromeResult)
        {
            this.chromeResult = chromeResult;
        }

        public void SetDriver(IWebDriver driver)
        {
            this.driver = driver;
        }

        public ChromeHepler()
        {
        }

        public async Task<ChromeResult> OpenDriver(string pathProfile = "", bool isShowImage = true, int chromeSizeWidth = 300, int chromeSizeHeight = 300)
        {
            try
            {
                ChromeOptions chromeOptions = new ChromeOptions();
                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
                string pathChrome = ChromeService.GetPathChrome();
                chromeOptions.BinaryLocation = pathChrome;

                chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.notifications", 2);

                chromeDriverService.HideCommandPromptWindow = true;
                chromeOptions.AddArgument($"--window-size={chromeSizeWidth},{chromeSizeHeight}");
                if (pathProfile != "")
                {
                    chromeOptions.AddArgument("--user-data-dir=" + pathProfile);
                }
                if (!isShowImage)
                {
                    chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
                }
                chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
                chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", true);
                chromeDriverService.SuppressInitialDiagnosticInformation = true;
                chromeDriverService.HideCommandPromptWindow = true;
                chromeResult = new ChromeResult();
                chromeResult.ChromeDriver = new ChromeDriver(chromeDriverService, chromeOptions);
                chromeResult.IdProcess = chromeDriverService.ProcessId;
                chrome = chromeResult.ChromeDriver;
                return chromeResult;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                File.WriteAllText("Error.txt", $"Class:{nameof(ChromeHepler)} ==>{OpenDriver}< exception:+>> {ex.ToString()}");
            }
            return null;
        }
        //Chức năng: Chuyển đổi Cookie dạng Text sang dạng Cookie của selenium
        public List<Cookie> ConvertRawCookieToCookie(string cookieText, string domain)
        {
            List<Cookie> listCookies = new List<Cookie>();
            string[] splitCookies = cookieText.Split(';');
            foreach (string cookie in splitCookies)
            {
                string[] splitNameValue = cookie.Trim().Split('=');
                if (splitNameValue.Length == 2)
                {
                    string name = splitNameValue[0];
                    string value = splitNameValue[1];
                    listCookies.Add(new Cookie(name, value, domain, "/", DateTime.Now.AddDays(100)));
                }
            }
            return listCookies;
        }

        //Chức năng: Thêm cookie vào Chrome
        public bool AddCookie(string url, string cookie)
        {
            try
            {
                string domain = url.Replace("https://", "");
                domain = domain.Replace("http://", "");
                domain = domain.Replace("/", "");
                List<Cookie> listCookies = this.ConvertRawCookieToCookie(cookie, "." + domain);
                foreach (Cookie cook in listCookies)
                {
                    this.chrome.Manage().Cookies.AddCookie(cook);
                }
                return true;
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
                return false;
            }
        }

        //Chức năng: Thoát cửa sổ Chrome
        public bool Quit()
        {
            try
            {
                this.chrome.Dispose();
                this.chrome.Quit();
                return true;
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
                return false;
            }
        }

        //Chức năng: Đi tới đường dẫn
        public bool GoToUrl(string url)
        {
            try
            {
                this.chrome.Url = url;
                this.chrome.Navigate();
                return true;
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
                return false;
            }
        }

        //Chức năng: Tìm phần tử trong trang web
        public IWebElement FindElement(By searchElement)
        {
            try
            {
                return this.chrome.FindElement(searchElement);

            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
            return null;
        }
        public IWebElement FindElement(By searchElement, int timeout)
        {
            try
            {
                if (!FindElementExist(searchElement, timeout))
                {
                    return null;
                }

                return this.chrome.FindElement(searchElement);
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
            return null;
        }
        public bool FindElementExist(By searchElement, int timeout)
        {
            try
            {
                IWebElement result = null;
                Stopwatch stopwatch = Stopwatch.StartNew();

                while (stopwatch.ElapsedMilliseconds < timeout * 1000)
                {
                    try
                    {
                        result = this.chrome.FindElement(searchElement);
                        if (result != null)
                        {
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }

            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
            return false;
        }



        public static List<IWebElement> FindElements(ChromeDriver driver, By by, int index = 0, int timeout = 3)
        {
            if (!Waits(driver, by, index, timeout))
            {
                return null;
            }
            return driver.FindElements(by).ToList();
        }

        //Chức năng: Click vào phần tử đã được tìm
        public bool Click(By searchElement)
        {
            try
            {
                this.FindElement(searchElement).Click();
                return true;

            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
            return false;
        }

        public static bool Click(ChromeDriver driver, IWebElement elem, int index = 0, int timeout = 3)
        {
            ScrollIntoView(driver, elem);
            elem.Click();
            return true;
        }
        public bool Click(ChromeDriver driver, By by, int index = 0, int timeout = 3)
        {
            if (!Waits(driver, by, index, timeout))
            {
                return false;
            }
            var elem = driver.FindElements(by)[index];
            ScrollIntoView(driver, elem);
            elem.Click();
            return true;
        }

        //Chức năng: Truyền chữ vào phần tử đã được tìm
        public bool SendKeys(By searchElement, string keys)
        {
            try
            {
                this.FindElement(searchElement).SendKeys(keys);
                return true;

            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
            return false;
        }

        //SendKeys: Chức năng chuyền text vô Element đã được tìm kiếm
        public bool SendKeys(By searchElement, string keys, int timeout = 30)
        {

            try
            {
                if (!Wait(searchElement, timeout))
                {
                    return false;
                }
                this.chrome.FindElement(searchElement).SendKeys(keys);
                return true;

            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
            return false;
        }

        public bool SendKeys(ChromeDriver driver, By by, object text, int index = 0, bool clear = false, int timeout = 3, bool checkDisplayed = true, bool fastMode = false, bool enter = false)
        {
            if (!Waits(driver, by, index, timeout, checkDisplayed))
            {
                return false;
            }
            var elem = driver.FindElements(by)[index];
            ScrollIntoView(driver, elem);
            if (clear)
            {
                elem.Clear();
            }
            if (fastMode)
            {
                try
                {
                    Thread t = new Thread(
                    () =>
                    {
                        Clipboard.SetText(text.ToString());
                        elem.SendKeys(OpenQA.Selenium.Keys.Control + 'v');
                    });
                    t.SetApartmentState(ApartmentState.STA);
                    t.Start();
                    t.Join();
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    elem.SendKeys(text.ToString());
                    if (enter)
                    {
                        elem.SendKeys(OpenQA.Selenium.Keys.Enter);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("ChromeDriver only supports characters in the BMP"))
                    {
                        try
                        {
                            Thread t = new Thread(
                            () =>
                            {
                                Clipboard.SetText(text.ToString());
                                elem?.SendKeys(OpenQA.Selenium.Keys.Control + 'v');
                            });
                            t.SetApartmentState(ApartmentState.STA);
                            t.Start();
                            t.Join();
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
            }
            string value = driver.ExecuteScript("return arguments[0].value", elem)?.ToString()!;
            if (value == "")
            {

                return SendKeys(driver, by, text, index, clear, timeout);
            }
            return true;
        }

        public static bool SendKeys(ChromeDriver driver, IWebElement elem, object text, int index = 0, bool clear = false, int timeout = 3, bool checkDisplayed = true, bool fastMode = false, bool enter = false)
        {
            ScrollIntoView(driver, elem);
            if (clear)
            {
                elem.Clear();
            }
            if (fastMode)
            {
                try
                {
                    Thread t = new Thread(
                    () =>
                    {
                        Clipboard.SetText(text.ToString());
                        elem.SendKeys(OpenQA.Selenium.Keys.Control + 'v');
                    });
                    t.SetApartmentState(ApartmentState.STA);
                    t.Start();
                    t.Join();
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    elem.SendKeys(text.ToString());
                    if (enter)
                    {
                        elem.SendKeys(OpenQA.Selenium.Keys.Enter);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("ChromeDriver only supports characters in the BMP"))
                    {
                        try
                        {
                            Thread t = new Thread(
                            () =>
                            {
                                Clipboard.SetText(text.ToString());
                                elem?.SendKeys(OpenQA.Selenium.Keys.Control + 'v');
                            });
                            t.SetApartmentState(ApartmentState.STA);
                            t.Start();
                            t.Join();
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
            }
            string value = driver.ExecuteScript("return arguments[0].value", elem)?.ToString()!;
            if (value == "")
            {

                return SendKeys(driver, elem, text, index, clear, timeout);
            }
            return true;
        }

        public bool Wait(By xpath, int timeout = 30)
        {
            Stopwatch stopwatcth = Stopwatch.StartNew();
            while (stopwatcth.ElapsedMilliseconds < timeout * 1000)
            {
                try
                {
                    var element = this.chrome.FindElement(xpath);
                    if (element != null)
                    {
                        return true;
                    }
                }
                catch
                {

                }
            }

            return false;
        }
        //Chức năng: Thực thi 1 đoạn javaScript vào cửa sổ web hiện tại
        public bool ExecuteScript(string jsContent)
        {
            try
            {
                this.chrome.ExecuteScript(jsContent);
                return true;
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
                return false;
            }
        }



        public static void ScrollIntoView(ChromeDriver driver, IWebElement elem, double delay = 1)
        {
            Actions actions = new Actions(driver);
            actions.MoveToElement(elem);
            actions.Perform();
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView({ behavior: 'smooth', block: 'end', inline: 'nearest' });", elem);
            driver.ExecuteScript("arguments[0].scrollIntoView({ behavior: \"smooth\", block: \"end\", inline: \"nearest\" });", elem);
            Sleep(delay);
        }

        public static void Sleep(double delay)
        {
            Thread.Sleep(Convert.ToInt32(delay * 1000));
        }

        public static bool Waits(ChromeDriver driver, List<By> bys, int timeout = 3, bool checkDisplayed = true)
        {
            for (int i = 0; i < timeout; i++)
            {
                foreach (var by in bys)
                {
                    try
                    {
                        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(1));
                        wait.Until(d =>
                        {
                            try
                            {
                                IWebElement elem = driver.FindElement(by);
                                if (checkDisplayed)
                                {
                                    return elem.Enabled && elem.Displayed;
                                }
                                else
                                {
                                    return elem.Enabled;
                                }
                            }
                            catch
                            {
                                return false;
                            }
                        });
                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("no such window: target window already closed"))
                        {
                            throw new Exception("no such window: target window already closed");
                        }
                    }
                }
            }
            return false;
        }
        public static bool Waits(ChromeDriver driver, By by, int index = 0, int timeout = 3, bool checkDisplayed = true)
        {
            for (int i = 0; i < timeout; i++)
            {
                try
                {
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
                    wait.Until(d =>
                    {
                        try
                        {
                            if (driver?.FindElements(by)?.Count > 0)
                            {
                                IWebElement elem = driver.FindElements(by)[index];
                                if (checkDisplayed)
                                {
                                    return elem.Enabled! && elem.Displayed!;
                                }
                                else
                                {
                                    return elem.Enabled;
                                }
                            }
                        }
                        catch
                        {
                        }

                        return false;
                    });
                    return true;
                }
                catch (Exception ex)
                {
                    //if (ex.Message.Contains("no such window: target window already closed"))
                    //{
                    //    throw new Exception("no such window: target window already closed");
                    //}
                }
            }
            return false;
        }
    }
}
