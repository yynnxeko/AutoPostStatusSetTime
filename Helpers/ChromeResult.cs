using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Helpers
{
    public class ChromeResult
    {
        public ChromeDriver ChromeDriver { get; set; }

        public Label LBStatus { get; set; }

        public int IdProcess { get; set; }
    }
}
