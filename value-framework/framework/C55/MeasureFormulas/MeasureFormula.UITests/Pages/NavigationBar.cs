using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace MeasureFormula.UITests.Pages
{
    public class NavigationBar
    {
        protected readonly RemoteWebDriver Driver;

        public NavigationBar(RemoteWebDriver driver)
        {
            Driver = driver;
        }

        public void OpenHoverLinkPanel(int index)
        {
            var breadcrumbElement =
                Driver.FindElementByXPath($"//div[@class='cl-master__breadcrumb-separator'][{index}]");
            breadcrumbElement.Click();

            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(3));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("hoverLinkPanel")));
        }

        public InvestmentCloningPage OpenInvestmentCloningPage()
        {
            var cloneOption = Driver.FindElementByXPath("//a[@title='Make a copy of this investment']");
            cloneOption.Click();
            return new InvestmentCloningPage(Driver);
        }

        
    }
}
