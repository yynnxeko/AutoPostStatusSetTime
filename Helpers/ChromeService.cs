using Microsoft.Win32;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Helpers
{
    internal class ChromeService
    {
        private struct UNICODE_STRING
        {
            public ushort Length;

            public ushort MaximumLength;

            public IntPtr Buffer;
        }

        private struct PROCESS_BASIC_INFORMATION
        {
            public IntPtr Reserved1;

            public IntPtr PebBaseAddress;

            public IntPtr Reserved2;

            public IntPtr Reserved3;

            public IntPtr UniqueProcessId;

            public IntPtr Reserved4;
        }

        public static string GetChromeDriverVersion(bool isUseChromePortable = false, string pathChromePortableDir = "")
        {
            try
            {
                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
                try
                {
                    ChromeOptions chromeOptions = new ChromeOptions();
                    chromeDriverService.SuppressInitialDiagnosticInformation = true;
                    chromeDriverService.HideCommandPromptWindow = true;
                    chromeOptions.AddArgument("--headless");
                    if (isUseChromePortable)
                    {
                        string pathChromePortable = pathChromePortableDir + "\\GoogleChromePortable\\App\\Chrome-bin\\chrome.exe";
                        string pathChromePortable64 = pathChromePortableDir + "\\GoogleChromePortable64\\App\\Chrome-bin\\chrome.exe";
                        if (File.Exists(pathChromePortable))
                        {
                            chromeOptions.BinaryLocation = pathChromePortable;
                        }
                        else if (File.Exists(pathChromePortable64))
                        {
                            chromeOptions.BinaryLocation = pathChromePortable64;
                        }
                    }
                    ChromeDriver chromeDriver = new ChromeDriver(chromeDriverService, chromeOptions);
                    string result = (chromeDriver.Capabilities.GetCapability("chrome") as Dictionary<string, object>)["chromedriverVersion"].ToString().Split(' ')[0];
                    chromeDriver.Quit();
                    return result;
                }
                catch
                {
                    List<int> list = new List<int> { chromeDriverService.ProcessId };
                    foreach (ManagementBaseObject item2 in new ManagementObjectSearcher($"Select * From Win32_Process Where ParentProcessID={chromeDriverService.ProcessId}").Get())
                    {
                        int item = Convert.ToInt32(item2["ProcessID"]);
                        list.Add(item);
                    }
                    foreach (int item3 in list)
                    {
                        Process.GetProcessById(Convert.ToInt32(item3)).Kill();
                    }
                }
            }
            catch
            {
            }
            return null;
        }

        private static string GetCommandLine(Process process)
        {
            IntPtr hProcess = OpenProcess(1040, bInheritHandle: false, process.Id);
            if (hProcess == IntPtr.Zero)
            {
                return string.Empty;
            }
            try
            {
                return GetProcessCommandLine(hProcess);
            }
            finally
            {
                CloseHandle(hProcess);
            }
        }

        private static string GetProcessCommandLine(IntPtr hProcess)
        {
            IntPtr pPeb = GetPebAddress(hProcess);
            IntPtr rtlUserProcParamsAddress = IntPtr.Zero;
            if (pPeb == IntPtr.Zero)
            {
                return string.Empty;
            }
            if (!ReadProcessMemory(hProcess, (IntPtr)((long)pPeb + 32), out rtlUserProcParamsAddress, (uint)IntPtr.Size, out int lpNumberOfBytesRead))
            {
                return string.Empty;
            }
            if (rtlUserProcParamsAddress == IntPtr.Zero)
            {
                return string.Empty;
            }
            UNICODE_STRING commandLine = default(UNICODE_STRING);
            if (!ReadProcessMemory(hProcess, (IntPtr)((long)rtlUserProcParamsAddress + 112), out commandLine, (uint)Marshal.SizeOf(typeof(UNICODE_STRING)), out lpNumberOfBytesRead))
            {
                return string.Empty;
            }
            byte[] commandLineBytes = new byte[commandLine.Length];
            if (!ReadProcessMemory(hProcess, commandLine.Buffer, commandLineBytes, (uint)commandLineBytes.Length, out lpNumberOfBytesRead))
            {
                return string.Empty;
            }
            return Encoding.Unicode.GetString(commandLineBytes);
        }

        public static string GetPathChrome(bool isChromePortable = false, string pathChromePortableDir = "")
        {
            try
            {
                if (isChromePortable)
                {
                    string text = pathChromePortableDir + "\\GoogleChromePortable\\App\\Chrome-bin\\chrome.exe";
                    if (File.Exists(text))
                    {
                        return text;
                    }
                    string result = pathChromePortableDir + "\\GoogleChromePortable64\\App\\Chrome-bin\\chrome.exe";
                    if (File.Exists(text))
                    {
                        return result;
                    }
                }
                string text2 = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe";
                string text3 = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe";
                if (File.Exists(text2))
                {
                    return text2;
                }
                if (File.Exists(text3))
                {
                    return text3;
                }
                using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\chrome.exe");
                if (registryKey != null)
                {
                    object value = registryKey.GetValue("");
                    if (!string.IsNullOrEmpty(value.ToString()))
                    {
                        return value.ToString();
                    }
                }
            }
            catch (Exception)
            {
            }
            return "";
        }

        private static IntPtr GetPebAddress(IntPtr hProcess)
        {
            PROCESS_BASIC_INFORMATION pbi = default(PROCESS_BASIC_INFORMATION);
            NtQueryInformationProcess(hProcess, 0, ref pbi, Marshal.SizeOf(pbi), out var _);
            return pbi.PebBaseAddress;
        }

        [DllImport("ntdll.dll")]
        private static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, ref PROCESS_BASIC_INFORMATION processInformation, int processInformationLength, out int returnLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(int processAccess, bool bInheritHandle, int processId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, out IntPtr lpBuffer, uint dwSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, out UNICODE_STRING lpBuffer, uint dwSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint dwSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        public static Point GetSizeChrome(int column, int row)
        {
            column = ((column < 5) ? 5 : ((column > 20) ? 20 : column));
            row = ((row < 2) ? 2 : ((row > 10) ? 10 : row));
            int width = Screen.PrimaryScreen.WorkingArea.Width;
            int height = Screen.PrimaryScreen.WorkingArea.Height;
            int x = width / column + 15;
            int y = height / row + 10;
            return new Point(x, y);
        }

        public static Point GetPositionChrome(int positionIndex, int column, int row)
        {
            column = ((column < 5) ? 5 : ((column > 20) ? 20 : column));
            row = ((row < 2) ? 2 : ((row > 10) ? 10 : row));
            int width = Screen.PrimaryScreen.WorkingArea.Width;
            int height = Screen.PrimaryScreen.WorkingArea.Height;
            Point result = default(Point);
            while (positionIndex >= column * row)
            {
                positionIndex -= column * row;
            }
            switch (row)
            {
                case 1:
                    result.Y = 0;
                    break;
                case 2:
                    if (positionIndex < column)
                    {
                        result.Y = 0;
                    }
                    else if (positionIndex < column * 2)
                    {
                        result.Y = height / 2;
                        positionIndex -= column;
                    }
                    break;
                case 3:
                    if (positionIndex < column)
                    {
                        result.Y = 0;
                    }
                    else if (positionIndex < column * 2)
                    {
                        result.Y = height / 3;
                        positionIndex -= column;
                    }
                    else if (positionIndex < column * 3)
                    {
                        result.Y = height / 3 * 2;
                        positionIndex -= column * 2;
                    }
                    break;
                case 4:
                    if (positionIndex < column)
                    {
                        result.Y = 0;
                    }
                    else if (positionIndex < column * 2)
                    {
                        result.Y = height / 4;
                        positionIndex -= column;
                    }
                    else if (positionIndex < column * 3)
                    {
                        result.Y = height / 4 * 2;
                        positionIndex -= column * 2;
                    }
                    else if (positionIndex < column * 4)
                    {
                        result.Y = height / 4 * 3;
                        positionIndex -= column * 3;
                    }
                    break;
                case 5:
                    if (positionIndex < column)
                    {
                        result.Y = 0;
                    }
                    else if (positionIndex < column * 2)
                    {
                        result.Y = height / 5;
                        positionIndex -= column;
                    }
                    else if (positionIndex < column * 3)
                    {
                        result.Y = height / 5 * 2;
                        positionIndex -= column * 2;
                    }
                    else if (positionIndex < column * 4)
                    {
                        result.Y = height / 5 * 3;
                        positionIndex -= column * 3;
                    }
                    else
                    {
                        result.Y = height / 5 * 4;
                        positionIndex -= column * 4;
                    }
                    break;
            }
            int num = width / column;
            result.X = positionIndex * num - 10;
            return result;
        }

        public static int GetIndexOfPossitionApp(ref List<int> listPossition)
        {
            int result = 0;
            lock (listPossition)
            {
                for (int i = 0; i < listPossition.Count; i++)
                {
                    if (listPossition[i] == 0)
                    {
                        result = i;
                        listPossition[i] = 1;
                        break;
                    }
                }
            }
            return result;
        }

        public static bool KillAllChromeByProfileName(string profileName, bool chromeProfile = false)
        {
            bool result = false;
            try
            {
                Process[] processesByName = Process.GetProcessesByName("chrome");
                foreach (Process process in processesByName)
                {
                    string commandLine = GetCommandLine(process);
                    if (!string.IsNullOrEmpty(commandLine))
                    {
                        if (chromeProfile && commandLine.Contains(profileName) && commandLine.Contains("--user-data-dir"))
                        {
                            result = true;
                            process.Kill();
                        }
                        else if (!chromeProfile && commandLine.Contains(profileName))
                        {
                            result = true;
                            process.Kill();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return result;
        }
    }
}
