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
    class CustomMsgPage
    {
        public IWebDriver driver { get; set; }
        public WebDriverWait wait;

        public CustomMsgPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        /***Page Object for CustomMessage****/

        [FindsBy(How = How.XPath, Using = "//div[@class='d-flex flex-row' and contains(text(), 'Custom')]")]
        public IWebElement menuCutomMsg;

        //[FindsBy(How = How.Id, Using = "mat-input-1")]
        //public IWebElement txtAreaCustomMsg;

        [FindsBy(How = How.XPath, Using = "//textarea")]
        public IWebElement txtAreaCustomMsg;

        //[FindsBy(How = How.Id, Using = "mat-radio-2")]
        //public IWebElement radioIncludeOfficeUrl;

        [FindsBy(How = How.XPath, Using = "//mat-radio-button[@value=1]")]
        public IWebElement radioIncludeOfficeUrl;

        [FindsBy(How = How.XPath, Using = "//p[contains(text(),'Liberty Tax-')]")]
        public IWebElement lblCutomMsgMirror;

        [FindsBy(How = How.XPath, Using = "//*[@id='create-message-template-form']//following-sibling::p")]
        public IWebElement lblMsgLength;

        [FindsBy(How = How.XPath, Using = "//span[contains(text(),'Approval')]/parent::node()")]
        public IWebElement btnSubmitForApproval;

        [FindsBy(How = How.XPath, Using = "//span[contains(text(),'Your message has been submitted')]")]
        public IWebElement sbCustomMessageAdd;

        [FindsBy(How = How.XPath, Using = "//div[contains(text(),'Pending')]")]
        public IWebElement lblPending;


        /***Page Object for DashBoard-Approved URL****/

        [FindsBy(How = How.XPath, Using = "(//mat-card[@class='m-3 mat-card'])[4]//following-sibling::mat-grid-tile//div[@class='d-flex w-100 align-content-start content-text-tile' and contains(text(), 'Liberty')]")]
        public IWebElement lstCutomMsgPending;

        [FindsBy(How = How.XPath, Using = "//mat-radio-button[@value=0]")]
        public IWebElement radioIncludeAppointmentUrl;

        /***Page Object for DashBoard-No URL****/

        [FindsBy(How = How.XPath, Using = "//mat-radio-button[@value=2]")]
        public IWebElement radioIncludeNoUrl;
    }
}
