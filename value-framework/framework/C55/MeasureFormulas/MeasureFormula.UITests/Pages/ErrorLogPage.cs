using System;
using System.Text.RegularExpressions;
using System.Threading;
using MeasureFormula.UITests.Bases;
using MeasureFormula.UITests.ExtensionMethods;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace MeasureFormula.UITests.Pages
{
    public class ErrorLogPage : PageBase
    {
        private const string PartialUrl = "Pages/Tools/ErrorLog/Views/ErrorLog.aspx";

        public ErrorLogPage(RemoteWebDriver driver) : base(driver)
        {
        }

        public static ErrorLogPage NavigateToThisPageViaUrl(RemoteWebDriver driver)
        {
            driver.Navigate().GoToUrl(BaseWebsiteUrl + PartialUrl);
            return new ErrorLogPage(driver);
        }

        public bool GetDataImportMessageAndParse()
        {
            Thread.Sleep(TimeSpan.FromSeconds(5)); //might need to wait until the import finished
            var importMessage = Driver.FindElementWait(By.XPath("(//span[@ng-bind='dataItem.fullMessage' and contains(., 'Universal Import')])[1]"), timeout:20).Text;
            var regex = new Regex(@"^((?!Unable to import).)*$"); //check that no failures happened
            var match = regex.Match(importMessage);
            return match.Success;
        }
    }
}
