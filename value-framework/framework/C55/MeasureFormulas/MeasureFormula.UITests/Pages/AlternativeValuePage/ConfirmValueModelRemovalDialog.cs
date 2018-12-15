using MeasureFormula.UITests.Bases;
using MeasureFormula.UITests.ExtensionMethods;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace MeasureFormula.UITests.Pages.AlternativeValuePage
{
    public class ConfirmValueModelRemovalDialog : PageBase
    {
        public ConfirmValueModelRemovalDialog(RemoteWebDriver driver) : base(driver)
        {
            Wait.Until(d => ExpectedConditions.ElementToBeClickable(By.XPath("//button[@title='Delete']")));
        }

        public AlternativeValuePage Delete()
        {
            var deleteButton = Driver.FindElementWait(By.XPath("//button[@title='Delete']"));
            var executor = (IJavaScriptExecutor) Driver;
            executor.ExecuteScript("arguments[0].click();", deleteButton);

            Wait.Until(d => Driver.FindElements(By.XPath("//div[@class='k-overlay']")).Count == 0);
            return new AlternativeValuePage(Driver);
        }
    }
}