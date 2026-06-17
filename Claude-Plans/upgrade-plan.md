# Kế Hoạch Nâng Cấp AutoPostStatusSetTime

> Mục tiêu: Nâng cấp từ công cụ thủ công, dễ bị đóng băng giao diện sang ứng dụng tự động mượt mà, dễ dùng — theo chuẩn kiến trúc của dự án **AutoDeletePost** (anh em cùng thư mục).

---

## So sánh hiện trạng

| Tiêu chí | AutoPostStatusSetTime (hiện tại) | AutoDeletePost (mục tiêu) |
|---|---|---|
| .NET version | 6.0 | 8.0 |
| Browser automation | Selenium 4.27 (sync) | Playwright 1.59 (async) |
| Giao diện khi chạy | **Đóng băng** | Mượt, cập nhật real-time |
| Selector quản lý | Hard-code trong C# | **selectors.json** — sửa không cần build lại |
| Log / trạng thái | MessageBox sau khi xong | Bảng log live + DataGridView |
| Nút hủy | Không có | Có (CancellationToken) |
| Xử lý lỗi | Nuốt exception im lặng | Retry, auto-relogin, fallback |
| Lưu cấu hình | `App.config` + file .txt | File .txt theo từng trường |
| Bảo mật | **Plain-text credentials** | Tách profile riêng |

---

## Các giai đoạn nâng cấp

### Giai đoạn 1 — Nền tảng kỹ thuật
*Ưu tiên cao nhất, ảnh hưởng mọi thứ còn lại*

#### 1.1 Nâng .NET 6 → .NET 8
- Sửa `WinFormsApp1.csproj`: `<TargetFramework>net8.0-windows</TargetFramework>`
- Cập nhật tất cả NuGet packages lên bản tương thích .NET 8

#### 1.2 Thay Selenium bằng Playwright
- Xóa: `Selenium.WebDriver`, `Selenium.Support`, `UndetectedChromeDriver`, `WebDriverManager`, `DotNetSeleniumExtras.WaitHelpers`
- Thêm: `Microsoft.Playwright` (v1.59+)
- Lợi ích:
  - Async thực sự → không freeze UI
  - Persistent browser context → không tạo lại session mỗi lần
  - Headless mode toggle
  - Built-in wait (không cần `Thread.Sleep`)

#### 1.3 Tạo selectors.json
Tách tất cả XPath/CSS selector ra file JSON (như AutoDeletePost):
```json
{
  "version": "1.0",
  "login": {
    "emailInput": ["#email", "input[name='email']"],
    "passwordInput": ["#pass", "input[name='pass']"],
    "submitButton": ["[name='login']", "button[type='submit']"],
    "twoFaInput": ["#approvals_code", "input[name='approvals_code']"]
  },
  "composer": {
    "openButton": ["[aria-label='Write something...']", "//span[text()='Write something...']"],
    "textArea": ["div[role='textbox'][contenteditable='true']"],
    "imageUpload": ["input[type='file'][accept*='image']"],
    "scheduleButton": ["[aria-label='Schedule post']", "[aria-label='Lên lịch bài viết']"]
  },
  "scheduler": {
    "dateInput": ["input[placeholder*='date']", "//label[text()='Date']/../input"],
    "timeInput": ["input[placeholder*='time']", "//label[text()='Time']/../input"],
    "confirmButton": ["[aria-label='Schedule']", "[aria-label='Lên lịch']"]
  }
}
```
- Cấu hình project copy file sang output: `<CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>`
- Tạo class `SelectorProvider.cs` (copy pattern từ AutoDeletePost)

---

### Giai đoạn 2 — Kiến trúc code
*Tách business logic ra khỏi Form1.cs*

#### 2.1 Tạo class `FacebookPostService.cs`
```
Helpers/
├── FacebookPostService.cs    ← Logic chính (thay ChromeHepler.cs)
├── SelectorProvider.cs       ← Đọc selectors.json
└── PostTask.cs               ← Data model cho mỗi nhóm cần đăng
```

**FacebookPostService** nên:
- Constructor nhận `IBrowser`/`IBrowserContext` từ Playwright
- Expose events: `OnLog`, `OnTaskScanned`, `OnTaskStatusChanged`
- Phương thức chính: `RunPostTaskAsync(IEnumerable<PostTask> tasks, CancellationToken ct)`
- Phương thức phụ: `LoginAsync()`, `IsLoggedInAsync()`, `PostToGroupAsync(PostTask task)`

#### 2.2 Data model `PostTask.cs`
```csharp
public record PostTask(
    string GroupUrl,
    string Content,
    string DatePost,       // MM/DD/YYYY
    string TimePost,       // HH:MM AM/PM
    string ImageFolderPath
);
```

#### 2.3 Form1.cs chỉ làm UI
- Subscribe events từ `FacebookPostService`
- Gọi `Task.Run()` để chạy service trên background thread
- Dùng `BeginInvoke()` để update UI từ event handler

---

### Giai đoạn 3 — Giao diện người dùng
*Tăng trải nghiệm sử dụng*

#### 3.1 Bố cục mới (tham khảo AutoDeletePost)
```
┌─────────────────────────────────────────────────────────────────┐
│  [LEFT PANEL - 580px]              [RIGHT PANEL - 620px]        │
│                                                                 │
│  UID|Pass|2FA Secret               ┌─ DataGridView ──────────┐  │
│  [_________________________]       │STT│Nhóm│Ngày│Giờ│Status│  │
│                                    │ 1 │... │... │...│ ⏳    │  │
│  Thư mục ảnh                       │ 2 │... │... │...│ ✅    │  │
│  [_________________________]       │ 3 │... │... │...│ ❌    │  │
│                                    └───────────────────────────┘ │
│  Link Nhóm (mỗi dòng 1 link)                                    │
│  [_________________________]       ┌─ Logs ──────────────────┐  │
│                                    │[10:23:01] Đăng nhập OK  │  │
│  Nội dung (phân cách bằng |)       │[10:23:05] Đăng nhóm 1.. │  │
│  [_________________________]       │[10:23:12] ✅ Thành công │  │
│                                    └───────────────────────────┘ │
│  Ngày đăng    Giờ đăng                                          │
│  [__________] [__________]                                      │
│                                                                 │
│  Delay giữa nhóm: [___] giây                                    │
│                                                                 │
│  [ ] Lưu đăng nhập  [ ] Headless Mode                          │
│                                                                 │
│  [🚀 Bắt đầu]  [🛑 Hủy]                                        │
└─────────────────────────────────────────────────────────────────┘
```

#### 3.2 DataGridView — bảng tiến trình
Cột: **STT | Nhóm | Nội dung (truncated) | Ngày | Giờ | Trạng thái | Chi tiết**

Trạng thái:
- `Chờ đăng` (màu xám)
- `Đang đăng ⏳` (màu xanh dương)
- `Thành công ✅` (màu xanh lá)
- `Thất bại ❌` (màu đỏ — kèm lý do trong cột Chi tiết)

#### 3.3 Log panel real-time
- Background đen, text lime-green (monospace)
- Format: `[HH:mm:ss] Message`
- Auto-scroll xuống dòng mới nhất
- Copy tất cả bằng chuột phải → "Copy logs"

#### 3.4 Nút Hủy
- `CancellationTokenSource _cts` — hủy giữa chừng an toàn
- Khi hủy: dừng sau khi hoàn thành nhóm hiện tại (không cắt giữa chừng)

#### 3.5 Headless Mode checkbox
- Ẩn cửa sổ Chrome khi chạy
- Tăng tốc ~40%, giảm RAM

---

### Giai đoạn 4 — Tính năng mới
*Sau khi nền tảng ổn định*

#### 4.1 Retry logic
```
Mỗi nhóm thất bại → retry tối đa 2 lần
  Lần 1: Sau 5 giây
  Lần 2: Sau 15 giây
  Nếu vẫn lỗi: đánh dấu ❌, ghi lý do, tiếp tục nhóm tiếp theo
```

#### 4.2 Validation đầu vào
Kiểm tra trước khi chạy:
- [ ] Số link nhóm = số ngày = số giờ (hoặc dùng 1 ngày/giờ cho tất cả)
- [ ] Thư mục ảnh tồn tại (nếu có nhập)
- [ ] Định dạng ngày hợp lệ (MM/DD/YYYY)
- [ ] Định dạng giờ hợp lệ (HH:MM AM/PM)
- [ ] Thông báo lỗi rõ ràng trước khi start

#### 4.3 "1 ngày/giờ cho tất cả" shortcut
Nếu chỉ nhập 1 ngày và 1 giờ nhưng có nhiều nhóm → tự động áp dụng cho tất cả.

#### 4.4 Progress bar
Thanh tiến trình tổng thể: `Đã đăng: 3/10 nhóm`

#### 4.5 Export kết quả
Nút "Xuất kết quả" → lưu CSV/TXT với danh sách nhóm + trạng thái sau khi xong.

---

### Giai đoạn 5 — Bảo mật & Ổn định
*Không đặt ưu tiên ngay nhưng cần ghi nhớ*

#### 5.1 Bảo vệ credentials
- Mã hóa `savedLoginInput.txt` bằng `DPAPI` (Windows Data Protection API) — mã hóa theo user account Windows, không cần thêm dependency
- Hoặc ít nhất: cảnh báo người dùng khi lưu 2FA secret

#### 5.2 Quản lý profile Playwright
- Profile lưu tại `./Profiles/{UID}/` (giống Selenium hiện tại)
- Reuse profile giữa các lần chạy → không cần login lại

#### 5.3 Xử lý Facebook thay đổi UI
- Khi selector thất bại → log cụ thể selector nào bị lỗi
- Gợi ý user cập nhật `selectors.json`
- Không crash toàn bộ app khi 1 selector fail

---

## Thứ tự thực hiện khuyến nghị

```
Tuần 1:
  ✅ Giai đoạn 1.1 — Nâng .NET 8
  ✅ Giai đoạn 1.2 — Playwright thay Selenium
  ✅ Giai đoạn 1.3 — selectors.json + SelectorProvider

Tuần 2:
  ✅ Giai đoạn 2   — Tái cấu trúc code (FacebookPostService)
  ✅ Giai đoạn 3.1-3.4 — UI mới với DataGridView + Log panel

Tuần 3:
  ✅ Giai đoạn 3.5 + 4.1-4.3 — Nút Hủy, Retry, Validation
  ✅ Giai đoạn 4.4-4.5 — Progress bar, Export

Tuần 4 (tùy chọn):
  ✅ Giai đoạn 5 — Bảo mật DPAPI
```

---

## File cần tạo mới

| File | Mục đích |
|---|---|
| `selectors.json` | Quản lý selector động |
| `Helpers/FacebookPostService.cs` | Business logic chính |
| `Helpers/SelectorProvider.cs` | Parse selectors.json |
| `Helpers/PostTask.cs` | Data model |

## File cần sửa lớn

| File | Thay đổi |
|---|---|
| `WinFormsApp1.csproj` | .NET 8, bỏ Selenium, thêm Playwright |
| `Form1.cs` | UI mới + kết nối service qua events |
| `Form1.Designer.cs` | Bố cục mới (DataGridView, Log panel, Cancel button) |

## File có thể xóa sau nâng cấp

| File | Lý do |
|---|---|
| `Helpers/ChromeHepler.cs` | Thay bởi FacebookPostService.cs |
| `Helpers/ChromeService.cs` | Không cần với Playwright |
| `Helpers/ChromeResult.cs` | Thay bởi PostTask.cs |

---

## Rủi ro & lưu ý

1. **Facebook thay đổi UI thường xuyên** — selectors.json giải quyết vấn đề này nhưng vẫn cần cập nhật định kỳ.
2. **Playwright cần install browser lần đầu** — chạy `playwright install chromium` một lần sau khi publish.
3. **Headless mode có thể bị Facebook detect** — giữ option chạy có giao diện khi gặp sự cố.
4. **Cookie hiện tại sẽ không dùng được** — chuyển sang Playwright Persistent Context (profile-based session).
