using MeasureFormula.UITests.Bases;
using MeasureFormula.UITests.ExtensionMethods;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using SeleniumExtras.WaitHelpers;

namespace MeasureFormula.UITests.Pages
{
    public class InvestmentDetailsPage : PageBase
    {
        public InvestmentDetailsPage(RemoteWebDriver driver) : base(driver) { }

        public static InvestmentDetailsPage NavigateToThisPageViaUrl(RemoteWebDriver driver, string partialUrl)
        {
            driver.Navigate().GoToUrl(partialUrl);
            return new InvestmentDetailsPage(driver);
        }

        public AlternativeValuePage.AlternativeValuePage OpenAlternativeValuePage()
        {
            var arrowButton = Driver.FindElementByXPath("//tr[@id='alternativeItemRow']//div[@class='multiTargetLink-button']");
            arrowButton.Click();

            var alternativeValueOption = Driver.FindElementWait(By.XPath("//a[@title='Alternative Value']"));
            alternativeValueOption.Click();
            return new AlternativeValuePage.AlternativeValuePage(Driver);
        }

        public void Delete()
        {
            var deleteButton =
                Driver.FindElementWait(By.XPath("//div[@id='mpHeaderBtnBarDirective']//a[contains(@title, 'Delete this investment')]"));
            deleteButton.Click();

            Wait.Until(ExpectedConditions.ElementIsVisible(By.Id("msgBox")));
            var okButton = Driver.FindElementWait(By.XPath("//div[@id='msgBox']//input[@value='OK']"));
            okButton.Click();
        }

    }
}
