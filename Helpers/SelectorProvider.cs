using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace WinFormsApp1.Helpers
{
    public class SelectorProvider
    {
        private readonly Dictionary<string, Dictionary<string, string[]>> _selectors = new();
        private readonly string _configFilePath;

        public SelectorProvider()
        {
            // Tìm file selectors.json trong thư mục ứng dụng hoặc thư mục project
            _configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "selectors.json");
            ReloadSelectors();
        }

        public async Task<IElementHandle?> FindElementAsync(IPage page, string selectorKey)
        {
            var parts = selectorKey.Split('.');
            if (parts.Length != 2) return null;

            var selectors = GetSelectors(parts[0], parts[1]);
            if (selectors == null || selectors.Length == 0) return null;

            foreach (var selector in selectors)
            {
                try
                {
                    var element = await page.QuerySelectorAsync(selector);
                    if (element != null && await element.IsVisibleAsync()) return element;
                }
                catch
                {
                    // Bỏ qua lỗi cú pháp selector và thử cái tiếp theo
                }
            }
            return null;
        }

        public string[] GetSelectors(string category, string key)
        {
            if (_selectors.TryGetValue(category, out var categorySelectors))
            {
                if (categorySelectors.TryGetValue(key, out var selectors))
                {
                    return selectors;
                }
            }
            return Array.Empty<string>();
        }

        public void ReloadSelectors()
        {
            try
            {
                // Nếu file ở AppDomain.BaseDirectory không có, thử tìm ở thư mục project cha
                string finalPath = _configFilePath;
                if (!File.Exists(finalPath))
                {
                    var fallbackPath = Path.Combine(Directory.GetCurrentDirectory(), "selectors.json");
                    if (File.Exists(fallbackPath))
                    {
                        finalPath = fallbackPath;
                    }
                }

                if (File.Exists(finalPath))
                {
                    var json = File.ReadAllText(finalPath);
                    var doc = JsonDocument.Parse(json);
                    _selectors.Clear();

                    foreach (var prop in doc.RootElement.EnumerateObject())
                    {
                        if (prop.Name == "version" || prop.Name == "lastUpdated") continue;

                        var dict = new Dictionary<string, string[]>();
                        foreach (var s in prop.Value.EnumerateObject())
                        {
                            var arr = new List<string>();
                            foreach (var item in s.Value.EnumerateArray())
                            {
                                arr.Add(item.GetString()!);
                            }
                            dict[s.Name] = arr.ToArray();
                        }
                        _selectors[prop.Name] = dict;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading selectors.json: {ex.Message}");
            }
        }
    }
}
