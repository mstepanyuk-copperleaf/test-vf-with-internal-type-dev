using MeasureFormula.UITests.Bases;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace MeasureFormula.UITests.Pages
{
    public class InvestmentCloningPage : PageBase
    {
        public InvestmentCloningPage(RemoteWebDriver driver) : base(driver) { }

        public InvestmentDetailsPage Clone(string investmentName, string pwpPowerPlant)
        {
            var nameField = Driver.FindElementById("ctl00_mainContentBody_txtNewName");
            nameField.SendKeys(investmentName);

            var facilityField = Driver.FindElementById("ctl00_mainContentBody_ddlFacility");
            var dropDown = new SelectElement(facilityField);
            dropDown.SelectByText(pwpPowerPlant);

            var saveButton = Driver.FindElementByXPath("//div[@id='mpHeaderBtnBarDirective']//a[@title='Save']");
            saveButton.Click();
            return new InvestmentDetailsPage(Driver);
        }
    }
}
