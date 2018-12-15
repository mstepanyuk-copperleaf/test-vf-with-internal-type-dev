using System;
using System.Collections.Generic;
using MeasureFormula.UITests.Bases;
using MeasureFormula.UITests.ExtensionMethods;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace MeasureFormula.UITests.Pages.AlternativeValuePage
{
    public class AnswerQuestionnairesDialog : PageBase
    {
        public string Title => Driver.FindElementText(By.XPath("//span[@class='k-window-title']"));
        public string GetValue => Driver.FindElementText(By.XPath("(//dd[@data-ng-show='!msBreakdownInspectorCtrl.isForBaseline'])[1]"));
        public List<string> ValuesforFY18 => Driver.FindElementsText(By.XPath("//td[@fiscal-year='2018' and @class='cl-numeric-col gridYearValue']"
                                                                                     + "/div[not(contains(@class, 'cl-colored-cell'))]"));
        public List<string> ValuesforFY19 => Driver.FindElementsText(By.XPath("//td[@fiscal-year='2019' and @class='cl-numeric-col gridYearValue']"
                                                                                     + "/div[not(contains(@class, 'cl-colored-cell'))]"));
        public List<string> ValuesforFY20 => Driver.FindElementsText(By.XPath("//td[@fiscal-year='2020' and @class='cl-numeric-col gridYearValue']"
                                                                                     + "/div[not(contains(@class, 'cl-colored-cell'))]"));

        public string StartDatePickerValue => Driver.FindElementText(By.XPath("//div[@class='cl-breakdown-container']"
                                                                              + "//input[@kendo-date-picker='rangedDatePickerCtrl.startDatePicker']"));

        public void SetStartDatePickerValue(string year)
        {
            if (year == string.Empty || StartDatePickerValue == year) return;
            var datePicker = Driver.FindElementWait(By.XPath("//div[@class='cl-breakdown-container']"
                                                             + "//input[@kendo-date-picker='rangedDatePickerCtrl.startDatePicker']"));
            datePicker.Clear();
            datePicker.SendKeys(year);
            datePicker.SendKeys(Keys.Enter);
        }

        public AnswerQuestionnairesDialog(RemoteWebDriver driver) : base(driver)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
            wait.Until(d => Driver.FindElement(By.XPath("//div[@class='k-widget k-window']")) != null);
        }

        public QuestionnairesTab Close()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
            wait.Until(d => Driver.FindElementById("busyIndicator").GetAttribute("class").Contains("hide"));

            var closeButton =
                Driver.FindElementWait(By.XPath("(//div[@class='k-window-actions']//a)[2]"));
            closeButton.Click();
            return new QuestionnairesTab(Driver);
        }
    }
}