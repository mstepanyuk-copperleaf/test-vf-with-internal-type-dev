using System;
using MeasureFormula.UITests.Bases;
using MeasureFormula.UITests.ExtensionMethods;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace MeasureFormula.UITests.Pages
{
    public class InvestmentSearchPage : PageBase
    {
        private const string PartialUrl = "Pages/AdvancedExpenditureSearch/Views/AdvancedExpenditureSearch.aspx";

        public InvestmentSearchPage(RemoteWebDriver driver) : base(driver)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(3));
            wait.Until(d => Driver.FindElementById("busyIndicator").GetAttribute("class").Contains("hide"));
        }

        public static InvestmentSearchPage NavigateToThisPageViaUrl(RemoteWebDriver driver)
        {
            driver.Navigate().GoToUrl(BaseWebsiteUrl + PartialUrl);
            return new InvestmentSearchPage(driver);
        }

        public string SearchForInvestment(string keyword)
        {
            var searchField = Driver.FindElementWait(By.ClassName("k-input"));
            searchField.SendKeys(keyword + Keys.Enter);

            //wait for the filtered list
            Driver.FindElementWait(By.XPath($"//a[contains(., '{keyword}')]"));
            var arrowButton =
                Driver.FindElementWait(By.XPath("(//button[@class='multiTargetLinkNav-button cl-float-right'])[1]"));
            arrowButton.Click();
            var url = Driver.FindElementWait(By.XPath("//a[@title='Alternative Value']")).GetAttribute("href");
            return url;
        }

    }
}
