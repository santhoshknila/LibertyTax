using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LibertyTax.Common;
using LibertyTax.PageObjects;
using System.Configuration;
using System.Diagnostics;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using Allure.Commons;
using NUnit.Allure.Steps;

namespace LibertyTax.TestClasses
{
    [TestFixture, Order(1)]
    [AllureNUnit]
    [AllureSuite("Login & Setup Suite")]

    public class LoginClass
    {
        public IWebDriver driver;

        LoginPage loginObj;
        WebDriverWait wait;
        CommonFunionality objCommonFn;
        ExcelUtility excel;

        protected static CustomMessage basePage;

        public string SheetName = ConfigurationManager.AppSettings["SheetName"];
        public static string xlFilePath = ConfigurationManager.AppSettings["XlFilePath"];
        public static string browser = ConfigurationManager.AppSettings["Browser"];
        public string fullFilePath = Path.Combine((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath.Split(new string[] { "/bin" }, StringSplitOptions.None)[0]
            , xlFilePath).Replace("\\@", "");


        [OneTimeSetUp]
        public void SetUp()
        {
            try
            {
                          
                driver = Browsers.Init();
                LogWriter.Logger("Browser has been initialized");
                loginObj = new LoginPage(driver);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                objCommonFn = new CommonFunionality();
                excel = new ExcelUtility(fullFilePath);
                string parent = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));

                var configJson = Path.Combine(parent, "allureConfig.Template.json");
                if (File.Exists(configJson))
                {
                    Environment.SetEnvironmentVariable("ALLURE_CONFIG", configJson);
                    Environment.CurrentDirectory = Path.GetDirectoryName(GetType().Assembly.Location);
                    AllureLifecycle.Instance.CleanupResultDirectory();

                }
                LogWriter.Logger(" Setup has been done successfully.");
            }
            catch (Exception e)
            {
                LogWriter.Logger(e.Message);
                Assert.Fail();
            }

        }

        [Test(Description = "To check whether user able to login into the application"), Order(1)]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureStep("1. Open the URL \n 2. Enter the username and password \n 3. Click on Login button")]
        public void fnLoginIntoApplication()
        {
            try
            {

                String currentMethodName = objCommonFn.fnGetMethodName(MethodBase.GetCurrentMethod().Name);

                //Get UserName
                var UserName = excel.GetCellDataPosition(SheetName, currentMethodName, "UserName");
                LogWriter.Logger("User Name from Excel " + UserName);

                //Get Password
                var Password = excel.GetCellDataPosition(SheetName, currentMethodName, "Password");
                LogWriter.Logger("User Password  from Excel" + Password);

                loginObj.txtUsername.SendKeys(UserName);
                loginObj.txtPassword.SendKeys(Password);
                loginObj.btnLogin.Click();

                wait.Until(ExpectedConditions.TextToBePresentInElement(loginObj.lblDashboard, "Dashboard"));
                LogWriter.Logger("Login has been done successfully.");
                Assert.IsTrue(loginObj.lblDashboard.Displayed);
                Assert.IsTrue(driver.Title.Contains("FusionText"));
                basePage = new CustomMessage();
                basePage.setWebDriver(driver);
                LogWriter.Logger(" Dashboard has been redirected successfully");
            }
            catch (Exception e)
            {
                LogWriter.Logger(e.Message);
                Assert.Fail();
            }
        }
        

        [TearDown]
        public void fnScreenshot()
        {
            try
            {
                objCommonFn.fnTakeScreenshot(driver);
            }
            catch (Exception e)
            {
                LogWriter.Logger(e.Message);
            }

        }

    }
}
