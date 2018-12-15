using System;
using MeasureFormula.UITests.Bases;
using MeasureFormula.UITests.ExtensionMethods;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace MeasureFormula.UITests.Pages
{
    public class DataImportPage : PageBase
    {
        private const string PartialUrl = "Modules/Admin/DataImport.aspx";

        public DataImportPage(RemoteWebDriver driver) : base(driver)
        {
            Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(3));
        }

        public static DataImportPage NavigateToThisPageViaUrl(RemoteWebDriver driver)
        {
            driver.Navigate().GoToUrl(BaseWebsiteUrl + PartialUrl);
            return new DataImportPage(driver);
        }

        public bool ImportFile(string filePath)
        {
            if(filePath == string.Empty) return false;

            var dragAndDropArea = Driver.FindElementWait(By.XPath("//input[@type='file']"));
            dragAndDropArea.SendKeys(filePath);
            var uploadButton = Driver.FindElementWait(By.XPath("//input[@value='Upload']"));
            uploadButton.Click();
            
            Wait.Until(d => Driver.FindElement(By.XPath("//td[contains(., 'Matching Columns')]")) != null);

            var importButton = Driver.FindElementWait(By.XPath("//div[@data-role='toolbar']//a[@title='Import']"));
            importButton.Click();

            Wait.Until(d => Driver.FindElement(By.Id("msgBoxTitle")) != null);
            var closeButton = Driver.FindElement(By.Id("errorDialogButton"));
            closeButton.Click();
            return true;
        }
    }
}
