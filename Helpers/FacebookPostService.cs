using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Playwright;
using OtpNet;
using Google.Authenticator;
using Base32Encoding = Google.Authenticator.Base32Encoding;

namespace WinFormsApp1.Helpers
{
    public class PostInfo
    {
        public int Index { get; set; }
        public string GroupUrl { get; set; } = string.Empty;
        public string PostUrl { get; set; } = string.Empty;
        public string ContentSnippet { get; set; } = string.Empty;
        public string Status { get; set; } = "Chờ đăng";
        public string ErrorDetail { get; set; } = string.Empty;
    }

    public class FacebookPostService
    {
        private readonly SelectorProvider _selectors;
        private IPlaywright? _playwright;
        private IBrowserContext? _browserContext;

        public event Action<string>? OnLog;
        public event Action<PostInfo>? OnPostScanned;
        public event Action<string, string, string>? OnPostStatusChanged;

        public FacebookPostService()
        {
            _selectors = new SelectorProvider();
        }

        private void Log(string message)
        {
            OnLog?.Invoke($"[{DateTime.Now:HH:mm:ss}] {message}");
        }

        public static string GenerateTOTP(string base32Secret)
        {
            try
            {
                var cleaned = base32Secret.Trim().Replace(" ", string.Empty);
                var bytes = Base32Encoding.ToBytes(cleaned);
                var totp = new Totp(bytes);
                return totp.ComputeTotp();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi tạo mã 2FA: {ex.Message}. Hãy kiểm tra lại chuỗi bí mật 2FA.");
            }
        }

        public async Task<bool> InitializeBrowserAsync(string profilePath, bool headless = false)
        {
            try
            {
                Log("Đang kiểm tra và cài đặt trình duyệt lõi Playwright (lần đầu sẽ hơi lâu)...");
                try
                {
                    var exitCode = Microsoft.Playwright.Program.Main(new[] { "install", "chromium" });
                    if (exitCode != 0)
                    {
                        Log("Cảnh báo: Lệnh cài đặt trình duyệt lõi trả về mã lỗi.");
                    }
                }
                catch (Exception ex)
                {
                    Log($"Cảnh báo khi cài đặt trình duyệt lõi: {ex.Message}");
                }

                Log("Đang khởi tạo Playwright Engine...");
                _playwright ??= await Playwright.CreateAsync();

                string fullProfilePath = Path.Combine(profilePath);
                Directory.CreateDirectory(fullProfilePath);

                Log($"Khởi chạy trình duyệt với Profile: {fullProfilePath}");
                _browserContext = await _playwright.Chromium.LaunchPersistentContextAsync(
                    fullProfilePath,
                    new BrowserTypeLaunchPersistentContextOptions
                    {
                        Headless = headless,
                        Channel = "chromium",
                        ViewportSize = new ViewportSize { Width = 1280, Height = 800 },
                        Locale = "en-US", // Đặt mặc định là tiếng Anh Mỹ để đồng bộ hóa giao diện
                        TimezoneId = "Asia/Ho_Chi_Minh",
                        UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
                        IsMobile = false,
                        HasTouch = false,
                        Args = new[] { 
                            "--disable-blink-features=AutomationControlled",
                            "--disable-notifications"
                        }
                    });

                _browserContext.SetDefaultTimeout(30000);
                Log("Khởi tạo trình duyệt thành công.");
                return true;
            }
            catch (Exception ex)
            {
                Log($"Lỗi khởi động trình duyệt: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> IsLoggedInAsync(IPage page)
        {
            try
            {
                string url = page.Url.ToLower();
                if (url.Contains("/login") || url.Contains("/checkpoint") || url.Contains("/approvals") || url.Contains("2fa"))
                {
                    return false;
                }

                var emailInput = await page.QuerySelectorAsync("input[name='email'], input[id='email'], #email");
                if (emailInput != null && await emailInput.IsVisibleAsync())
                {
                    return false;
                }

                var passInput = await page.QuerySelectorAsync("input[name='pass'], input[id='pass'], #pass");
                if (passInput != null && await passInput.IsVisibleAsync())
                {
                    return false;
                }

                var loggedInSelectors = new[]
                {
                    "a[href*='/profile.php']", "a[href*='/me/']", "a[href*='/me?']", "a[href*='/me']",
                    "a[href*='/notifications']", "a[href*='/messages/']", "a[href*='/friends/']", 
                    "div[role='feed']", "article", "div[role='article']",
                    "input[placeholder*='Bạn đang nghĩ gì']", "input[placeholder*='What\\'s on your mind']", 
                    "div[aria-label*='Menu']", "a[aria-label*='Home']", "a[aria-label*='Trang chủ']",
                    "input[type='search']"
                };

                foreach (var selector in loggedInSelectors)
                {
                    try
                    {
                        var el = await page.QuerySelectorAsync(selector);
                        if (el != null && await el.IsVisibleAsync())
                        {
                            return true;
                        }
                    }
                    catch { }
                }

                if (emailInput == null && passInput == null && page.Url.Contains("facebook.com"))
                {
                    await Task.Delay(2000);
                    emailInput = await page.QuerySelectorAsync("input[name='email'], #email");
                    if (emailInput == null || !await emailInput.IsVisibleAsync())
                    {
                        return true;
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> LoginAsync(IPage page, string email, string password, string? twoFactorSecret)
        {
            Log("Đang mở trang chủ Facebook...");
            await page.GotoAsync("https://www.facebook.com/", new PageGotoOptions { WaitUntil = WaitUntilState.DOMContentLoaded });
            await Task.Delay(3000);

            if (await IsLoggedInAsync(page))
            {
                Log("Đã đăng nhập sẵn từ Session Profile cũ.");
                return true;
            }

            Log("Chưa đăng nhập. Tiến hành điền tài khoản...");
            var emailInput = await _selectors.FindElementAsync(page, "login.emailInput");
            if (emailInput == null)
            {
                var chkPassInput = await _selectors.FindElementAsync(page, "login.passwordInput");
                if (chkPassInput == null)
                {
                    Log("Không tìm thấy ô nhập email hay mật khẩu. Đang giả định đã đăng nhập.");
                    return true;
                }
                Log("Không tìm thấy ô nhập email. Thử tải lại trang...");
                return false;
            }

            if (emailInput != null)
            {
                await emailInput.ClickAsync();
                await page.Keyboard.DownAsync("Control");
                await page.Keyboard.PressAsync("a");
                await page.Keyboard.UpAsync("Control");
                await page.Keyboard.PressAsync("Backspace");
                await Task.Delay(300);

                Log("Đang nhập Email...");
                await emailInput.TypeAsync(email, new() { Delay = new Random().Next(50, 100) });
                await Task.Delay(500);
            }

            var passInput = await _selectors.FindElementAsync(page, "login.passwordInput");
            if (passInput != null)
            {
                await passInput.ClickAsync();
                await page.Keyboard.DownAsync("Control");
                await page.Keyboard.PressAsync("a");
                await page.Keyboard.UpAsync("Control");
                await page.Keyboard.PressAsync("Backspace");
                await Task.Delay(300);

                Log("Đang nhập Mật khẩu...");
                await passInput.TypeAsync(password, new() { Delay = new Random().Next(50, 100) });
                await Task.Delay(500);
            }

            Log("Click nút Đăng nhập...");
            var loginBtn = await _selectors.FindElementAsync(page, "login.loginButton");
            bool clicked = false;
            if (loginBtn != null)
            {
                try
                {
                    await loginBtn.ClickAsync(new ElementHandleClickOptions { Timeout = 3000 });
                    clicked = true;
                }
                catch
                {
                    Log("Click nút đăng nhập quá giờ. Chuyển sang ấn phím Enter...");
                }
            }

            if (!clicked)
            {
                await page.Keyboard.PressAsync("Enter");
            }

            Log("Đang chờ chuyển hướng (Chờ mã xác thực hoặc Trang chủ)...");
            
            bool is2FaRequired = false;
            for (int i = 0; i < 15; i++)
            {
                var codeInput = await _selectors.FindElementAsync(page, "login.twoFactorCodeInput");
                var tryAnother = await _selectors.FindElementAsync(page, "login.tryAnotherWay");
                if ((codeInput != null && await codeInput.IsVisibleAsync()) || (tryAnother != null && await tryAnother.IsVisibleAsync()))
                {
                    is2FaRequired = true;
                    break;
                }

                if (await IsLoggedInAsync(page)) break;
                await Task.Delay(1000);
            }

            if (is2FaRequired)
            {
                Log("Phát hiện màn hình yêu cầu mã xác thực 2FA...");
                
                var tryAnother = await _selectors.FindElementAsync(page, "login.tryAnotherWay");
                if (tryAnother != null)
                {
                    Log("Bấm 'Try Another Way' / 'Thử cách khác'...");
                    await tryAnother.ClickAsync(new ElementHandleClickOptions { Force = true });
                    await Task.Delay(2000);
                    
                    var authApp = await _selectors.FindElementAsync(page, "login.authAppOption");
                    if (authApp != null)
                    {
                        await authApp.ClickAsync(new ElementHandleClickOptions { Force = true });
                        await Task.Delay(2000);
                    }
                    
                    var continueBtn = await _selectors.FindElementAsync(page, "login.continueBtn");
                    if (continueBtn != null)
                    {
                        await continueBtn.ClickAsync(new ElementHandleClickOptions { Force = true });
                        await Task.Delay(2000);
                    }
                }

                var codeInput = await _selectors.FindElementAsync(page, "login.twoFactorCodeInput");
                if (codeInput != null)
                {
                    if (string.IsNullOrWhiteSpace(twoFactorSecret))
                    {
                        Log("LỖI: Tài khoản yêu cầu mã 2FA nhưng không có Chuỗi bí mật 2FA cung cấp.");
                        return false;
                    }

                    string code = GenerateTOTP(twoFactorSecret);
                    Log($"Tạo thành công mã OTP 2FA: {code}. Đang điền...");
                    await codeInput.ClickAsync();
                    await codeInput.FillAsync(code);
                    await Task.Delay(1000);

                    Log("Gửi mã xác nhận...");
                    await codeInput.PressAsync("Enter");
                    await Task.Delay(2000);

                    var submit2fa = await _selectors.FindElementAsync(page, "login.twoFactorSubmitButton");
                    if (submit2fa != null && await submit2fa.IsVisibleAsync())
                    {
                        await submit2fa.ClickAsync(new ElementHandleClickOptions { Force = true });
                        await Task.Delay(2000);
                    }

                    for (int i = 0; i < 12; i++)
                    {
                        if (await IsLoggedInAsync(page)) break;
                        var skipBtn = await _selectors.FindElementAsync(page, "login.saveBrowserSkipButton");
                        if (skipBtn != null && await skipBtn.IsVisibleAsync()) break;
                        await Task.Delay(1000);
                    }
                }
            }

            for (int i = 0; i < 3; i++)
            {
                var skipBtn = await _selectors.FindElementAsync(page, "login.saveBrowserSkipButton");
                if (skipBtn != null && await skipBtn.IsVisibleAsync())
                {
                    Log("Bỏ qua màn hình lưu trình duyệt/mật khẩu...");
                    await skipBtn.ClickAsync(new ElementHandleClickOptions { Force = true });
                    await Task.Delay(3000);
                }
                else break;
            }

            // Đồng bộ chuyển sang Tiếng Anh (US)
            await page.GotoAsync("https://www.facebook.com/settings/?tab=language", new PageGotoOptions { WaitUntil = WaitUntilState.DOMContentLoaded });
            await Task.Delay(3000);
            var checkEng = await _selectors.FindElementAsync(page, "language.englishUs");
            if (checkEng == null)
            {
                Log("Giao diện chưa phải tiếng Anh. Đang cố gắng chuyển đổi ngôn ngữ sang English (US)...");
                var checkViet = await _selectors.FindElementAsync(page, "language.tiengViet");
                if (checkViet != null)
                {
                    await checkViet.ClickAsync(new ElementHandleClickOptions { Force = true });
                    await Task.Delay(2000);
                    
                    var englishOption = await _selectors.FindElementAsync(page, "language.englishUs");
                    if (englishOption != null)
                    {
                        await englishOption.ClickAsync(new ElementHandleClickOptions { Force = true });
                        await Task.Delay(3000);
                    }
                }
            }

            await page.GotoAsync("https://www.facebook.com/");
            await Task.Delay(3000);

            if (await IsLoggedInAsync(page))
            {
                Log("Đăng nhập Facebook thành công!");
                return true;
            }

            Log("Đăng nhập thất bại. Vui lòng kiểm tra lại tài khoản hoặc 2FA.");
            return false;
        }

        public async Task RunPostTaskAsync(
            List<PostTask> tasks,
            string uid,
            string pass,
            string? twoFactorSecret,
            string profilePath,
            bool headless,
            int delayBetweenGroups,
            CancellationToken ct)
        {
            // Báo cáo danh sách các task ban đầu
            for (int i = 0; i < tasks.Count; i++)
            {
                var t = tasks[i];
                string snippet = t.Content;
                if (snippet.Length > 50) snippet = snippet.Substring(0, 50).Replace("\n", " ") + "...";
                OnPostScanned?.Invoke(new PostInfo
                {
                    Index = i + 1,
                    GroupUrl = t.GroupUrl,
                    PostUrl = t.GroupUrl,
                    ContentSnippet = snippet,
                    Status = "Chờ đăng",
                    ErrorDetail = ""
                });
            }

            if (!await InitializeBrowserAsync(profilePath, headless))
            {
                Log("Không thể khởi động trình duyệt Playwright. Tác vụ dừng.");
                return;
            }

            try
            {
                var page = await _browserContext!.NewPageAsync();
                
                if (!await LoginAsync(page, uid, pass, twoFactorSecret))
                {
                    Log("Đăng nhập thất bại. Dừng tác vụ.");
                    return;
                }

                for (int i = 0; i < tasks.Count; i++)
                {
                    if (ct.IsCancellationRequested)
                    {
                        Log("Tác vụ đã bị người dùng hủy.");
                        break;
                    }

                    var task = tasks[i];
                    OnPostStatusChanged?.Invoke(task.GroupUrl, "Đang đăng ⏳", "");
                    Log($"--------------------------------------------------");
                    Log($"[Task {i + 1}/{tasks.Count}] Đang chuẩn bị bài đăng cho nhóm: {task.GroupUrl}");

                    try
                    {
                        await page.GotoAsync(task.GroupUrl, new PageGotoOptions { WaitUntil = WaitUntilState.DOMContentLoaded });
                        await Task.Delay(4000);

                        // Phát hiện nếu hết phiên
                        if (!await IsLoggedInAsync(page))
                        {
                            Log("Phiên đăng nhập hết hạn. Đang tự động đăng nhập lại...");
                            if (await LoginAsync(page, uid, pass, twoFactorSecret))
                            {
                                await page.GotoAsync(task.GroupUrl, new PageGotoOptions { WaitUntil = WaitUntilState.DOMContentLoaded });
                                await Task.Delay(4000);
                            }
                            else
                            {
                                Log("Không thể đăng nhập lại. Bỏ qua nhóm này.");
                                continue;
                            }
                        }

                        // Mở composer
                        var openBtn = await _selectors.FindElementAsync(page, "composer.openButton");
                        if (openBtn != null)
                        {
                            Log("Bấm mở khung soạn thảo...");
                            await openBtn.ClickAsync(new ElementHandleClickOptions { Force = true });
                            await Task.Delay(3000);
                        }
                        else
                        {
                            Log("Không tìm thấy nút tạo bài viết. Có thể nhóm yêu cầu duyệt hoặc tài khoản không có quyền đăng.");
                            continue;
                        }

                        // Điền nội dung
                        var textbox = await _selectors.FindElementAsync(page, "composer.postTextbox");
                        if (textbox != null)
                        {
                            Log("Đang nhập nội dung bài viết...");
                            await textbox.FocusAsync();
                            await page.Keyboard.TypeAsync(task.Content + " ");
                            await Task.Delay(2000);
                        }
                        else
                        {
                            Log("Không tìm thấy khung nhập văn bản.");
                            continue;
                        }

                        // Upload ảnh nếu có
                        if (!string.IsNullOrWhiteSpace(task.ImageFolderPath) && Directory.Exists(task.ImageFolderPath))
                        {
                            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff" };
                            var images = Directory.GetFiles(task.ImageFolderPath, "*.*", SearchOption.TopDirectoryOnly)
                                .Where(file => imageExtensions.Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase))
                                .ToArray();

                            if (images.Length > 0)
                            {
                                Log($"Tìm thấy {images.Length} ảnh hợp lệ trong thư mục. Bắt đầu tải lên...");
                                
                                var photoVideoBtn = await _selectors.FindElementAsync(page, "composer.photoVideoBtn");
                                if (photoVideoBtn != null)
                                {
                                    await photoVideoBtn.ClickAsync(new ElementHandleClickOptions { Force = true });
                                    await Task.Delay(2000);
                                }

                                var fileInput = await page.QuerySelectorAsync("input[type='file'][multiple]");
                                fileInput ??= await page.QuerySelectorAsync("input[type='file']");
                                if (fileInput != null)
                                {
                                    await fileInput.SetInputFilesAsync(images);
                                    await Task.Delay(5000); // Chờ ảnh upload lên
                                }
                                else
                                {
                                    Log("Không tìm thấy input file để upload ảnh.");
                                }
                            }
                        }

                        // Lên lịch đăng
                        Log("Mở cấu hình lên lịch bài viết...");
                        var scheduleToggle = await _selectors.FindElementAsync(page, "composer.scheduleToggle");
                        if (scheduleToggle != null)
                        {
                            await scheduleToggle.ScrollIntoViewIfNeededAsync();
                            await Task.Delay(1000);
                            await scheduleToggle.ClickAsync(new ElementHandleClickOptions { Force = true });
                            await Task.Delay(3000);
                        }
                        else
                        {
                            Log("Không tìm thấy nút Lên lịch bài viết (Schedule post). Nhóm có thể không hỗ trợ lên lịch.");
                            continue;
                        }

                        // Chọn Date
                        var chooseDate = await _selectors.FindElementAsync(page, "scheduler.chooseDate");
                        if (chooseDate != null)
                        {
                            await chooseDate.ClickAsync(new ElementHandleClickOptions { Force = true });
                            await Task.Delay(2000);
                        }

                        var dateInput = await _selectors.FindElementAsync(page, "scheduler.dateInput");
                        if (dateInput != null)
                        {
                            Log($"Đang thiết lập ngày đăng: {task.DatePost}...");
                            await dateInput.FocusAsync();
                            // Clear
                            await page.Keyboard.DownAsync("Control");
                            await page.Keyboard.PressAsync("a");
                            await page.Keyboard.UpAsync("Control");
                            await page.Keyboard.PressAsync("Backspace");
                            await Task.Delay(500);

                            await dateInput.FillAsync(task.DatePost);
                            await Task.Delay(2000);
                        }

                        // Chọn Time
                        var chooseTime = await _selectors.FindElementAsync(page, "scheduler.chooseTime");
                        if (chooseTime != null)
                        {
                            await chooseTime.ClickAsync(new ElementHandleClickOptions { Force = true });
                            await Task.Delay(2000);
                        }

                        var timeInput = await _selectors.FindElementAsync(page, "scheduler.timeInput");
                        if (timeInput != null)
                        {
                            Log($"Đang thiết lập giờ đăng: {task.TimePost}...");
                            await timeInput.FocusAsync();
                            // Clear
                            await page.Keyboard.DownAsync("Control");
                            await page.Keyboard.PressAsync("a");
                            await page.Keyboard.UpAsync("Control");
                            await page.Keyboard.PressAsync("Backspace");
                            await Task.Delay(500);

                            await timeInput.FillAsync(task.TimePost);
                            await Task.Delay(1000);
                            await page.Keyboard.PressAsync("Enter");
                            await Task.Delay(2000);
                        }

                        // Nhấn nút xác nhận lên lịch
                        var confirmButton = await _selectors.FindElementAsync(page, "scheduler.confirmButton");
                        if (confirmButton != null)
                        {
                            Log("Bấm Xác nhận lên lịch (Schedule)...");
                            await confirmButton.ClickAsync(new ElementHandleClickOptions { Force = true });
                            Log("Đang chờ Facebook xử lý đặt lịch (Chờ 20 giây)...");
                            await Task.Delay(20000);
                            Log($"✅ Đã đặt lịch thành công cho nhóm: {task.GroupUrl}");
                            OnPostStatusChanged?.Invoke(task.GroupUrl, "Thành công ✅", "");
                        }
                        else
                        {
                            Log("❌ Không tìm thấy nút xác nhận Schedule.");
                            OnPostStatusChanged?.Invoke(task.GroupUrl, "Thất bại ❌", "Không tìm thấy nút Schedule");
                        }
                    }
                    catch (Exception ex)
                    {
                        Log($"❌ Lỗi khi đăng bài lên nhóm {task.GroupUrl}: {ex.Message}");
                        OnPostStatusChanged?.Invoke(task.GroupUrl, "Thất bại ❌", ex.Message);
                    }

                    // Chờ chuyển tiếp sang nhóm sau
                    if (i < tasks.Count - 1)
                    {
                        Log($"Nghỉ {delayBetweenGroups} giây trước khi chuyển sang nhóm tiếp theo...");
                        for (int sec = 0; sec < delayBetweenGroups; sec++)
                        {
                            if (ct.IsCancellationRequested) break;
                            await Task.Delay(1000);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"Lỗi nghiêm trọng trong quá trình xử lý: {ex.Message}");
            }
            finally
            {
                await CloseBrowserAsync();
                Log("Đã kết thúc phiên làm việc. Trình duyệt đã đóng.");
            }
        }

        public async Task CloseBrowserAsync()
        {
            try
            {
                if (_browserContext != null)
                {
                    await _browserContext.CloseAsync();
                    _browserContext = null;
                }
                if (_playwright != null)
                {
                    _playwright.Dispose();
                    _playwright = null;
                }
            }
            catch (Exception ex)
            {
                Log($"Lỗi khi đóng trình duyệt: {ex.Message}");
            }
        }
    }
}
