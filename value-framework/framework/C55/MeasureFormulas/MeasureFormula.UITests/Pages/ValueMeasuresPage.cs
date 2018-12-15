using System;
using System.Collections.Generic;
using System.Linq;
using MeasureFormula.UITests.Bases;
using MeasureFormula.UITests.ExtensionMethods;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace MeasureFormula.UITests.Pages
{
    public class ValueMeasuresPage : PageBase
    {
        private const string PartialUrl = "Pages/ValueMeasuresEditor/Views/ValueMeasuresEditor.aspx";

        public ValueMeasuresPage(RemoteWebDriver driver) : base(driver){}

        public bool IsConsequenceUnitUsesValueUnit(IWebElement valueMeasureLink)
        {
            var expectedValueMeasure = valueMeasureLink.Text;
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(3));
            wait.Until(d =>
            {
                var currentValueMeasure = Driver.FindElement(By.Id("valueMeasureDescription")).Text;
                return currentValueMeasure.Contains(expectedValueMeasure);
            });
            var consequenceUnit = Driver.FindElementText(By.XPath("//common-directives-drop-down-list"
                                                                  + "[@data-selected-value='ctrl.consequenceUnitId']"));
            return consequenceUnit == "Value Unit";
        }

        
        public static ValueMeasuresPage NavigateToThisPageViaUrl(RemoteWebDriver driver)
        {
            driver.Navigate().GoToUrl(BaseWebsiteUrl + PartialUrl);
            return new ValueMeasuresPage(driver);
        }

        public List<IWebElement> GetRiskValueMeasuresPageLinks()
        {
            IJavaScriptExecutor js = Driver;
            js.ExecuteScript("window.scrollBy(0,400)", "");

            //click the header of the section
            Driver.FindElementWait(By.XPath("//p[@class='k-reset' and text()='Risk']")).Click();
            return Driver.FindElementsNullIfNotFound(By.XPath("//div[@data-template='ctrl.listItemDataTemplate']"
                                                              + "//span[contains(@class,'cl-enum-label-measurecategory-risk')]")).ToList();
        }

    }
}
