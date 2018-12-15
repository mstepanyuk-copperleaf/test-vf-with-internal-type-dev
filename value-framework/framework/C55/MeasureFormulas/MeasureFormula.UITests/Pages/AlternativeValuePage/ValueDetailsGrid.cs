using System;
using System.Threading;
using MeasureFormula.UITests.Bases;
using MeasureFormula.UITests.enums;
using MeasureFormula.UITests.ExtensionMethods;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using SeleniumExtras.WaitHelpers;

namespace MeasureFormula.UITests.Pages.AlternativeValuePage
{
    public class ValueDetailsGrid : PageBase
    {
        public ValueDetailsGrid(RemoteWebDriver driver): base(driver) { }

        public void SelectValueMeasure(ValueMeasures valueMeasure)
        {
            switch (valueMeasure)
            {
                case ValueMeasures.FinancialRisk:
                    FinancialRiskValueMeasure.Click();
                    break;
                case ValueMeasures.FinancialRiskForTransmissionStation:
                    FinancialRiskForTransStationValueMeasure.Click();
                    break;
                case ValueMeasures.FinancialImpactCapital:
                    FinancialImpactCapitalValueMeasure.Click();
                    break;
                case ValueMeasures.ElectricReliabilityRisk:
                    ElectricReliabilityRiskValueMeasure.Click();
                    break;
                case ValueMeasures.EmployeeProductivityImpact:
                    EmployeeProductityImpactValueMeasure.Click();
                    break;
                case ValueMeasures.ComplianceRisk:
                    ComplianceRiskValueMeasure.Click();
                    break;
                case ValueMeasures.PublicPropertyRisk:
                    PublicPropertyRiskValueMeasure.Click();
                    break;
                case ValueMeasures.IndustrialSafetyRisk:
                    IndustrialSafetyRiskValueMeasure.Click();
                    break;
                case ValueMeasures.TransmissionReliabilityRisk:
                    TransmissionReliabilityRisk.Click();
                    break;
                case ValueMeasures.EnvironmentalRisk:
                    EnvironmentalRiskValueMeasure.Click();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(valueMeasure), valueMeasure, null);
            }
        }

        public InspectBreakdownDialog OpenInspectBreakdownDialog()
        {
            InspectBreakdownButton.Click();
            return new InspectBreakdownDialog(Driver);
        }

        public void SelectOptions(ValueDetailsTabValueFilters option)
        {
            Wait.Until(d => ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//div[@class='cl-busy-indicator-directive']")));
            var selectedByDefault = Driver.FindElementWait(By.XPath("(//span[contains(@class, 'k-dropdown-wrap')])[3]"));
            var toSelect = Enum.GetName(typeof(ValueDetailsTabValueFilters), option);
            if (selectedByDefault.Text == toSelect) return;

            Wait.Until(d =>
            {
                try
                {
                    selectedByDefault =
                        Driver.FindElement(By.XPath("(//span[contains(@class, 'k-dropdown-wrap')])[3]"));
                    selectedByDefault.Click();
                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
                catch (WebDriverException)
                {
                    return false;
                }
            });
            Thread.Sleep(TimeSpan.FromSeconds(1)); // need to wait for the Totals drop-down to be updated
            Driver.FindElementWait(By.XPath($"//li[text()='{toSelect}']")).Click();
        }

        public void SetStartDatePickerValue(string year)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(10));
            if (year == string.Empty || StartDatePickerValue == year) return;
            var datePicker = Driver.FindElementWait(By.XPath("//div[@data-role='toolbar']//input[@data-k-ng-model='rangedDatePickerCtrl.startDate']"));
            datePicker.Clear();
            datePicker.SendKeys(year);
            datePicker.SendKeys(Keys.Enter);
        }

        // value measures
        private IWebElement FinancialRiskValueMeasure => Driver.FindElementWait(By.XPath("//span[@class='cl-enum-label-measurecategory-risk']"));
        private IWebElement FinancialRiskForTransStationValueMeasure => Driver.FindElementWait(By.XPath("//span[@class='cl-enum-label-measurecategory-risk' and contains(., 'Financial Risk')]"));
        private IWebElement FinancialImpactCapitalValueMeasure => Driver.FindElementWait(By.XPath("//span[@class='cl-enum-label-measurecategory-benefit' and contains(., 'Capital')]"));
        private IWebElement ElectricReliabilityRiskValueMeasure => Driver.FindElementWait(By.XPath("//span[@class='cl-enum-label-measurecategory-benefit' and contains(., 'Electric')]"));
        private IWebElement EnvironmentalRiskValueMeasure => Driver.FindElementWait(By.XPath("//span[@class='cl-enum-label-measurecategory-risk' and contains(., 'Environmental')]"));
        private IWebElement EmployeeProductityImpactValueMeasure => Driver.FindElementWait(By.XPath("//div[@class='cl-resizable-grid-container']//span[@class='cl-enum-label-measurecategory-benefit']"));
        private IWebElement ComplianceRiskValueMeasure => Driver.FindElementWait(By.XPath("//span[@class='cl-enum-label-measurecategory-risk' and contains(., 'Compliance')]"));
        private IWebElement PublicPropertyRiskValueMeasure => Driver.FindElementWait(By.XPath("//span[@class='cl-enum-label-measurecategory-risk' and contains(., 'Property')]"));
        private IWebElement IndustrialSafetyRiskValueMeasure => Driver.FindElementWait(By.XPath("//span[@class='cl-enum-label-measurecategory-risk' and contains(., 'Industrial')]"));
        private IWebElement TransmissionReliabilityRisk => Driver.FindElementWait(By.XPath("//span[@class='cl-enum-label-measurecategory-risk' and contains(., 'Transmission Reliability Risk')]"));

        //toolbar buttons
        private IWebElement InspectBreakdownButton => Driver.FindElementWait(By.XPath("//div[@class='cl-resizable-grid-toolbar-container-with-title cl-container']//a[@title='Inspect Breakdown']"));

        public string GetValue(string measure)
        {
            var xpath =
                $"(//div[@kendo-grid='measureSetValueDetailsCtrl.valueDetailsGrid']//td[contains(.,'{measure}')]/following-sibling::td)[2]";
            return Driver.FindElementText(By.XPath(xpath)); 
        }

        public string GetCapexOutcomeValue =>
            Driver.FindElementText(By.XPath("(//div[@class='k-virtual-scrollable-wrap']//td[1]//span)[1]"));

        public string GetOpexOutcomeValue =>
            Driver.FindElementText(By.XPath("(//div[@class='k-virtual-scrollable-wrap']"
                                            + "//td[1]//span[@class='cl-impacted-value'])[2]"));

        public string GetTotalOutcomeValue => Driver.FindElementText(By.XPath("(//div[@class='k-virtual-scrollable-wrap']"
                                                                                                   + "//td[1]//span[@class='cl-impacted-value'])[3]"));
        public string GetTotalValueForSelectedFY => Driver.FindElementText(By.XPath("(//div[@class='k-virtual-scrollable-wrap']//span[@class='cl-impacted-value']"
                                                                                    + "|//span[@class='cl-impacted-value cl-risk-value__amount'])[1]"
                                                                                    + "|//td[@class='cl-numeric-col valueMeasureFiscalYearValue'][1]/span"));

        public string GetCurrentFiscalYearOutcomeValue =>
            Driver.FindElementText(By.XPath("(//div[@class='k-virtual-scrollable-wrap']//td[1]//span)[1]"));

        public string GetNextFiscalYearOutcomeValue =>
            Driver.FindElementText(By.XPath("(//div[@class='k-virtual-scrollable-wrap']//td[2]//span)[1]"));

        public string StartDatePickerValue => Driver.FindElementText(By.XPath("//div[@data-role='toolbar']"
                                                                              + "//input[@data-k-ng-model='rangedDatePickerCtrl.startDate']"));

        
    }
}
