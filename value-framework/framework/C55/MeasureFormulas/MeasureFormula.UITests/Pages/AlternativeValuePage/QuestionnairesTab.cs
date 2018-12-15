using System;
using System.Collections.Generic;
using System.Linq;
using MeasureFormula.UITests.Bases;
using MeasureFormula.UITests.enums;
using MeasureFormula.UITests.ExtensionMethods;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace MeasureFormula.UITests.Pages.AlternativeValuePage
{
    public class QuestionnairesTab : PageBase
    {
        public QuestionnairesTab(RemoteWebDriver driver) : base(driver)
        {
            WaitForComponentToLoad();
        }

        public void WaitForComponentToLoad()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
            wait.Until(d => Driver.FindElementsNullIfNotFound(By.XPath("//div[@class='cl-enum-icon-measuresetquestionnairestatus-completewithnoanswers inlineBlock']")) != null);
        }

        public bool IsDisplayed => Driver.FindElementOrNull(By.XPath("//li[@id='questionnairesTab']")).GetAttribute("class").Contains("k-state-active");

        public string GetValueChartValue => Driver.FindElementText(By.XPath("(//td[@class='textRight'])[1]"));

        public void SelectValueModel(ValueModels valueModel)
        {
            IWebElement element;
            switch (valueModel)
            {
                case ValueModels.FinancialRisk:
                    element = FinancialRiskModel;
                    break;
                case ValueModels.CyberSecurityRisk:
                    element = CyberSecurityRiskModel;
                    break;
                case ValueModels.ComplianceRisk:
                    element = ComplianceRiskModel;
                    break;
                case ValueModels.EnvironmentalRisk:
                    element = EnvironmentalRiskModel;
                    break;
                case ValueModels.PublicPropertyRisk:
                    element = PublicPropertyRiskModel;
                    break;
                case ValueModels.InvestmentCost:
                    element = InvestmentCostModel;
                    break;
                case ValueModels.BrandValueBenefit:
                    element = BrandValueBenefitModel;
                    break;
                case ValueModels.SafetyRisk:
                    element = SafetyRiskModel;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(valueModel), valueModel, null);
            }
            element.Click();
        }

        public void SelectValueModel(string valueModel)
        {
            var valuModelRecord = Driver.FindElementWait(By.XPath($"//span[text()='{valueModel}']"));
            valuModelRecord.Click();
        }

        public AnswerQuestionnairesDialog OpenAnswerQuestionnairesDialog()
        {
            var button = Driver.FindElementWait(By.XPath("(//a[@title='Answer Questionnaires' and not(contains(@class, 'k-state-disabled'))])[1]"));
            button.Click();
            return new AnswerQuestionnairesDialog(Driver);
        }

        public void SelectValueSummaryTab()
        {
            ValueSummaryTab.Click();
        }

        public ValueDetailsGrid SelectValueDetailsTab()
        {
            if (!ValueDetailsTab.GetAttribute("class").Contains("k-state-active"))
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(3));
                wait.Until(d => !Driver.FindElement(By.XPath("(//div[@class='cl-busy-indicator-directive'])[2]")).Displayed);
                ValueMeasureDetailsTab.Click();
            }
            return new ValueDetailsGrid(Driver);
        }

        public List<IWebElement> ValueModelsForAsset => Driver.FindElementsNullIfNotFound(By.XPath("//div[@class='cl-container cl-overflow-hidden']"
                                                                                                   + "//span[@class='cl-label__asset ' "
                                                                                                   + "and contains(@title, 'AutoTest_Asset_Type')]")).ToList();

        //value models on Questionnaires tab
        private IWebElement CyberSecurityRiskModel => Driver.FindElementByXPath("//span[text()='Cyber Security Risk']");
        private IWebElement FinancialRiskModel => Driver.FindElementWait(By.XPath("//span[text()='Financial Risk']"));
        private IWebElement ComplianceRiskModel => Driver.FindElementWait(By.XPath("//span[text()='Compliance Risk']"));
        private IWebElement EnvironmentalRiskModel => Driver.FindElementWait(By.XPath("//span[text()='Environmental Risk']"));
        private IWebElement PublicPropertyRiskModel => Driver.FindElementWait(By.XPath("//span[text()='Public Property Risk']"));
        private IWebElement InvestmentCostModel => Driver.FindElementWait(By.XPath("//span[text()='Investment Cost']"));
        private IWebElement BrandValueBenefitModel => Driver.FindElementWait(By.XPath("//span[text()='Brand Value Benefit']"));
        private IWebElement SafetyRiskModel => Driver.FindElementWait(By.XPath("//span[text()='Safety Risk']"));

        //child grids
        private IWebElement ValueSummaryTab => Driver.FindElementById("valueSummaryTab");
        private IWebElement ValueMeasureDetailsTab => Driver.FindElementWait(By.Id("valueMeasureDetailsTab"));
        private IWebElement ValueDetailsTab => Driver.FindElementWait(By.Id("valueMeasureDetailsTab"));

        
    }
}
