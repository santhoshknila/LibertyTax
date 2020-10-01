using Allure.Commons;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using NUnit.Allure.Steps;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LibertyTax.Common;
using LibertyTax.PageObjects;

namespace LibertyTax.TestClasses
{
    [TestFixture, Order(2)]
    [AllureNUnit]
    [AllureSuite("Custom Message Suite")]

    public class CustomMessage
    {
        protected static IWebDriver driver;

        public void setWebDriver(IWebDriver driver)
        {
            CustomMessage.driver = driver;
        }

        CustomMsgPage pageObjCustom;
        LoginPage pageObjLogin;
        WebDriverWait wait;
        ExcelUtility excel;
        CommonFunionality objCommonFn;


        public int len = 0;
        public static string xlFilePath = ConfigurationManager.AppSettings["XlFilePath"];
        public string SheetName = ConfigurationManager.AppSettings["SheetName"];
        public string CustMsg = "";
        public string ofcUrl = ConfigurationManager.AppSettings["OfcUrl"];
        public string appUrl = ConfigurationManager.AppSettings["AppUrl"];
        public string noUrl = ConfigurationManager.AppSettings["NoUrl"];
        public string fullFilePath = Path.Combine((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath.Split(new string[] { "/bin" }, StringSplitOptions.None)[0]
            , xlFilePath).Replace("\\@", "");

        [OneTimeSetUp]
        public void SetUp()
        {
            try
            {
                pageObjCustom = new CustomMsgPage(driver);
                pageObjLogin = new LoginPage(driver);
                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(100));
                excel = new ExcelUtility(fullFilePath);
                objCommonFn = new CommonFunionality();


            }
            catch (Exception e)
            {
                LogWriter.Logger(e.Message);
                Assert.Fail();
            }

        }


        [AllureSeverity(SeverityLevel.critical)]
        [AllureStep("1. Click on Custom Message menu from LHS \n 2. Verify Custom Message screen is appeared. \n 3. Verify Include Office URL is selected by default \n 4. Enter Custom Message")]
        [Test(Description = "To check whether Custom Message is page is redirected"), Order(1)]
        public void fnVerifyCustomPage()
        {
            try
            {
                String currentMethodName = objCommonFn.fnGetMethodName(MethodBase.GetCurrentMethod().Name);

                //Get Custome Message
                CustMsg = excel.GetCellDataPosition(SheetName, currentMethodName, "Message");
                CustMsg = CustMsg + LogWriter.RandomString(2);
                pageObjCustom.menuCutomMsg.Click();
                Assert.IsTrue(pageObjCustom.txtAreaCustomMsg.Displayed);
                Assert.IsTrue(pageObjCustom.radioIncludeOfficeUrl.GetAttribute("selected") == "true");
                pageObjCustom.txtAreaCustomMsg.SendKeys(CustMsg);
                Assert.IsTrue(pageObjCustom.lblCutomMsgMirror.Text.Equals("Liberty Tax- " + CustMsg));
                LogWriter.Logger(" Custome Msg Entered.");
                len = CustMsg.Length;
                LogWriter.Logger("Custome Message Length " + len);
            }
            catch (Exception e)
            {
                LogWriter.Logger(e.Message);
                Assert.Fail();
            }
        }
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStep("1.VErify the Mirror panel having same text of user entered \n 2.Verify the length of the cusomt message text and mirror message length is same")]
        [Test(Description = "To check whether length of the entered message is same when comparing the original and mirror panel"), Order(2)]
        public void fnVerifyLengthOfMsg()
        {
            try
            {
                String s = pageObjCustom.lblMsgLength.Text.Substring(0, pageObjCustom.lblMsgLength.Text.IndexOf("/"));
                int lenfromcaption = int.Parse(s);
                Assert.AreEqual(len, lenfromcaption, "Message length are same as appeared in caption");
                LogWriter.Logger(" Custome Msg Length Validated.");
            }
            catch (Exception e)
            {
                LogWriter.Logger(e.Message);
            }
        }

        [AllureSeverity(SeverityLevel.critical)]
        [AllureStep("1. Click on Submit on button \n 2. Verify success message is displayed \n 3. Verify dashboard screen is redirected.")]
        [Test(Description = "To check whether user able to save the custom message with Include Office URL option"), Order(3)]
        public void fnAddCutomMessage()
        {
            try
            {
                Thread.Sleep(5000);
                pageObjCustom.btnSubmitForApproval.Click();
                Thread.Sleep(5000);
                Assert.IsTrue(pageObjCustom.sbCustomMessageAdd.Displayed);
                wait.Until(ExpectedConditions.TextToBePresentInElement(pageObjLogin.lblDashboard, "Dashboard"));
                ////Assert.IsTrue(pageObjLogin.lblDashboard.Displayed);
                LogWriter.Logger(" Custome Msg Submited.");
            }
            catch (Exception e)
            {
                LogWriter.Logger(e.Message);
                Assert.Fail();
            }
        }

        [AllureSeverity(SeverityLevel.normal)]
        [AllureStep("1. Goto Pending section \n 2. Verify newly added custom message is displayed in table")]
        [Test(Description = "To check whether newly added message is displayed in Pending list"), Order(4)]
        public void fnVerifyNewlyAddedMsg()
        {
            try
            {
                Thread.Sleep(3000);
                Assert.IsTrue(pageObjCustom.lstCutomMsgPending.Text.Contains("Liberty Tax- " + CustMsg + ofcUrl));
                LogWriter.Logger(" Office URL Custome Msg Added in Dashboard.");
            }
            catch (Exception e)
            {
                LogWriter.Logger(e.Message);
                Assert.Fail();
            }
        }

        [AllureSeverity(SeverityLevel.critical)]
        [AllureStep("1.Enter cutom message \n 2.Select option as appointment URL \n 3. Click on Submit button \n 5.Verify success message is displayed. \n 6. Verify dashboard is redirected")]
        [Test(Description = "To check whether user able to save the custom message with appointment URL"), Order(5)]
        public void fnVerifyCustomePagewithAppURL()
        {
            try {
                Thread.Sleep(3000);
                fnVerifyCustomPage();
                fnVerifyLengthOfMsg();
                Thread.Sleep(8000);
                pageObjCustom.radioIncludeAppointmentUrl.Click();
                ////Assert.IsTrue(pageObjCustom.radioIncludeAppointmentUrl.GetAttribute("selected") == "true");
                LogWriter.Logger(" Click Appointment URL Radio Button.");
                fnAddCutomMessage();
                LogWriter.Logger(" Custome Msg Submited.");
            }
            catch (Exception e)
            {
                LogWriter.Logger(e.Message);
                Assert.Fail();
            }
        }
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStep("1. Goto Pending section \n 2. Verify newly added custom message is displayed in table")]
        [Test(Description = "To check whether newly added message is displayed in Pending list"), Order(6)]
        public void fnVerifyNewlyAddedAppURLMsg()
        {
            try { 
            Thread.Sleep(3000);
            Assert.IsTrue(pageObjCustom.lstCutomMsgPending.Text.Contains("Liberty Tax- " + CustMsg + appUrl));
                LogWriter.Logger(" Appointment URL Custome Msg Added in Dashboard.");
            }
            catch (Exception e)
            {
                LogWriter.Logger(e.Message);
                Assert.Fail();
            }
        }
        [AllureSeverity(SeverityLevel.critical)]
        [AllureStep("1.Enter cutom message \n 2.Select option as NO URL \n 3. Click on Submit button \n 5.Verify success message is displayed. \n 6. Verify dashboard is redirected")]

        [Test(Description = "To check whether user able to save the custom message with NO URL option"), Order(7)]
        public void fnVerifyCustomePagewithNoURL()
        {
            try
            {
                Thread.Sleep(3000);
                fnVerifyCustomPage();
                fnVerifyLengthOfMsg();
                Thread.Sleep(8000);
                pageObjCustom.radioIncludeNoUrl.Click();
                ////Assert.IsTrue(pageObjCustom.radioIncludeNoUrl.GetAttribute("selected") == "true");
                LogWriter.Logger(" Click No URL Radio Button.");
                fnAddCutomMessage();
                LogWriter.Logger(" Custome Msg Submited.");
            }
            catch (Exception e)
            {
                LogWriter.Logger(e.Message);
                Assert.Fail();
            }

        }
        [AllureSeverity(SeverityLevel.normal)]
        [AllureStep("1. Goto Pending section \n 2. Verify newly added custom message is displayed in table")]
        [Test(Description = "To check whether newly added message is displayed in Pending list"), Order(8)]
        public void fnVerifyNewlyAddedNopURLMsg()
        {
            try
            {
                Thread.Sleep(3000);
                Assert.IsTrue(pageObjCustom.lstCutomMsgPending.Text.Contains("Liberty Tax- " + CustMsg + noUrl));
                LogWriter.Logger(" No URL Custome Msg Added in Dashboard.");
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
