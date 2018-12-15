using System;
using System.Collections.Generic;
using System.Linq;
using MeasureFormula.UITests.Bases;
using MeasureFormula.UITests.enums;
using MeasureFormula.UITests.ExtensionMethods;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using SeleniumExtras.WaitHelpers;

namespace MeasureFormula.UITests.Pages.AlternativeValuePage
{
    public class AddValueModelsDialog : PageBase
    {
        public AddValueModelsDialog(RemoteWebDriver driver) : base(driver)
        {
            Wait.Until(d => ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='k-widget k-window']")));
        }

        public List<IWebElement> AvailableValueModels => Driver.FindElementsNullIfNotFound(By.XPath("//input[@type='checkbox']")).ToList();
        private IWebElement CyberSecurityRisk => Driver.FindElementWait(By.XPath("//span[@class='cl-measure-set-name' and text()='Cyber Security Risk']"));

        public AlternativeValuePage AddValueModel(ValueModels model)
        {
            IWebElement checkboxToSelect;
            switch (model)
            {
                case ValueModels.CyberSecurityRisk:
                    checkboxToSelect = CyberSecurityRisk;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(model), model, null);
            }
            checkboxToSelect.Click();
            var addButton = Driver.FindElementWait(By.XPath("//button[@title='Add']"));
            addButton.Click();
            Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='cl-notification-success']")));
            return new AlternativeValuePage(Driver);
        }

        public AlternativeValuePage AddAllValueModels()
        {
            foreach (var model in AvailableValueModels)
            {
                model.Click();
            }
            var addButton = Driver.FindElementWait(By.XPath("//button[@title='Add']"));
            addButton.Click();

            Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='cl-notification-success']")));
            return new AlternativeValuePage(Driver);
        }

        

    }
}
