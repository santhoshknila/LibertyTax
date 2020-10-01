using Allure.Commons;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LibertyTax.Common

   
{
    class CommonFunionality
    {

        protected static IWebDriver driver;
        public string[] arrFnName; 

        public string fnGetMethodName(String fnName)
        {
            try
            {
                arrFnName = fnName.Split('_');
                return arrFnName[4];
            }
            catch(Exception e)
            {
                LogWriter.Logger("MethodName is not recognized");
            }
            return arrFnName[4];
        }

        public void fnTakeScreenshot(IWebDriver driver)
        {
            try
            {
                var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Results");
                verifyResultFolderAvailable(path);
                CommonFunionality.driver = driver;
                var fileNameBase = $"{TestContext.CurrentContext.WorkerId}{DateTime.Now:yyyyMMdd_HHmmss}";
                //var artifactDirectory = Path.Combine(path, "Results");
                var fileName = Path.Combine(path, fileNameBase + ".png");
                Screenshot image = ((ITakesScreenshot)driver).GetScreenshot();
                image.SaveAsFile(fileName);
                AllureLifecycle.Instance.AddAttachment(fileName);
            }
            catch(Exception e)
            {
                LogWriter.Logger(e.Message);

            }
        }
        public static void verifyResultFolderAvailable(string path)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                if (!dir.Exists)
                {
                    dir.Create();
                }
            }
            catch { }
        }
    }
}
