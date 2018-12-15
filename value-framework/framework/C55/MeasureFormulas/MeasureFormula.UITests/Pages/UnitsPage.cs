using MeasureFormula.UITests.Bases;
using MeasureFormula.UITests.ExtensionMethods;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace MeasureFormula.UITests.Pages
{
    public class UnitsPage : PageBase
    {
        private const string PartialUrl = "Modules/Admin/Units.aspx";

        public UnitsPage(RemoteWebDriver driver) : base(driver) { }

        public static UnitsPage NavigateToThisPageViaUrl(RemoteWebDriver driver)
        {
            driver.Navigate().GoToUrl(BaseWebsiteUrl + PartialUrl);
            return new UnitsPage(driver);
        }

        public string ValueUnitDescription => Driver.FindElementText(By.XPath("//td[text()='Value Unit']/following-sibling::td[3]"));
    }
}
