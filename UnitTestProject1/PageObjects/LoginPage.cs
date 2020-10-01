using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibertyTax.PageObjects
{
    class LoginPage
    {

        public IWebDriver driver { get; set; }
        public WebDriverWait wait;

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        /***Page Object for Login****/
        [FindsBy(How = How.Id, Using = "userNameInput")]
        public IWebElement txtUsername;

        [FindsBy(How = How.Id, Using = "passwordInput")]
        public IWebElement txtPassword;

        [FindsBy(How = How.Id, Using = "submitButton")]
        public IWebElement btnLogin;

        [FindsBy(How = How.XPath, Using = "//a[@class='page-title-text']")]
        public IWebElement lblDashboard;


    }
}
