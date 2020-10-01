using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibertyTax.Common
{
    public class Browsers
    {
        public static IWebDriver webDriver;
        public static string baseURL = ConfigurationManager.AppSettings["URL"];
        public static string browser = ConfigurationManager.AppSettings["Browser"];

        public static IWebDriver Init()
        {
            switch (browser)
            {
                case "Chrome":
                    webDriver = new ChromeDriver();
                    break;
                case "Firefox":
                    webDriver = new FirefoxDriver();
                    break;
            }
            webDriver.Manage().Window.Maximize();
            baseURL = "https://auth.qa.libertytax.net/adfs/oauth2/authorize/?client_id=https%3A%2F%2Ffusionservices.qa.libertytax.net%2Ftext%2F&redirect_uri=https%3A%2F%2Ffusion.qa.libertytax.net%2Ftext&response_type=id_token%20token&scope=openid%20profile%20allatclaims&state=61b65cd2283b40b7a23060ae4abb799d&nonce=3f2a6ef8a2504306810b2ff4cffe86a3";
            Goto(baseURL);
            return webDriver;
        }
        public static string Title
        {
            get { return webDriver.Title; }
        }
        public static IWebDriver getDriver
        {
            get { return webDriver; }
        }
        public static void Goto(string url)
        {
            webDriver.Url = url;
        }
        public static void Close()
        {
            webDriver.Quit();
        }
       

    }
}
