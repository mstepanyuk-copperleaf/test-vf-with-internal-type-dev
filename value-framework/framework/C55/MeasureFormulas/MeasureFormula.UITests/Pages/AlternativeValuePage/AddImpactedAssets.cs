using System.Linq;
using MeasureFormula.UITests.Bases;
using MeasureFormula.UITests.ExtensionMethods;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace MeasureFormula.UITests.Pages.AlternativeValuePage
{
    public class AddImpactedAssetsDialog : PageBase
    {
        public AddImpactedAssetsDialog(RemoteWebDriver driver) : base(driver)
        {
            Wait.Until(d => ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='k-widget k-window']")));
        }

        public AlternativeValuePage AddAsset(int index)
        {
            var list = Driver.FindElementsNullIfNotFound(By.XPath("//input[@type='checkbox']")).ToList();
            list[index].Click();

            var okButton = Driver.FindElementWait(By.XPath("//button[@title='OK']"));
            okButton.Click();
            return new AlternativeValuePage(Driver);
        }
    }
}
