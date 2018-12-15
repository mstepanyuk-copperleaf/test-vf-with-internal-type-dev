using System;
using MeasureFormula.UITests.Bases;
using MeasureFormula.UITests.ExtensionMethods;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace MeasureFormula.UITests.Pages.AlternativeValuePage
{
    public class InspectBreakdownDialog : PageBase
    {
        public InspectBreakdownDialog(RemoteWebDriver driver) : base(driver)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
            wait.Until(d => ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='k-widget k-window']")));
        }

        public string BaselineResult => Driver.FindElementText(By.XPath("(//table[@class='k-selectable']//div[@class='cl-colored-cell'])[1]" +
                                                                          "/following-sibling::div"));
        public string FinancialImpactOutcome => Driver.FindElementText(By.XPath("(//div[@class='cl-breakdown-grid-container k-pane']//td[@class='cl-numeric-col'])[1]"));
        public string EmpProductivityImpactOutcome => Driver.FindElementText(By.XPath("//div[@class='k-grid-content k-auto-scrollable']//tr[2]/td[@class='cl-numeric-col'][1]"));
        public string ComplianceRiskBaselineResult => Driver.FindElementText(By.XPath("(//div[@class='k-grid-content k-auto-scrollable']//td[1]/div)[2]"));

        public string ElectReliabilityRiskBaselineResult => Driver.FindElementText(By.XPath("(//div[@class='cl-breakdown-grid-container k-pane']//td[@class='cl-numeric-col'])[1]"));
        public string IndustrialSafetyRiskBaselineResult => Driver.FindElementText(By.XPath("(//div[@class='k-grid-content k-auto-scrollable']//td[@class='cl-numeric-col'][1]/div)[2]"));
        public string TransReliabilityRiskBaselineResult => Driver.FindElementText(By.XPath("(//div[@class='cl-breakdown-grid-container k-pane']//td[@class='cl-numeric-col']//div)[2]"));
        public string FinancialRiskConsequenceValue => Driver.FindElementText(By.XPath("(//div[@class='k-grid-content k-auto-scrollable']//tr[@class='k-alt'][1]//div)[3]"));

        public string FinancialRiskLikelihoodValue => Driver.FindElementText(By.XPath("(//div[@class='k-grid-content k-auto-scrollable']//tr[@class='k-alt'][1]" +
                                                                                       "//following-sibling::tr//div)[2]"));
        public string TransStationModelFinRiskConsequenceValue => Driver.FindElementText(By.XPath("(//div[@class='k-grid-content k-auto-scrollable']//tr[@class='k-alt']//div)[2]"));
        public string FinancialImpactCapitalConsequenceValue => Driver.FindElementText(By.XPath("(//div[@class='cl-breakdown-grid-container k-pane']//tr[@class='k-alt'])[2]//td[1]"));
        public string FinancialImpactCapitalLikelihoodValue => Driver.FindElementText(By.XPath("(//div[@class='cl-breakdown-grid-container k-pane']" +
                                                                                                "//div[@class='k-grid-content k-auto-scrollable']//tr)[3]//td[1]"));
        public string ElectReliabilityRiskConsequenceValue => Driver.FindElementText(By.XPath("//div[@class='cl-breakdown-grid-container k-pane']" +
                                                                                               "//div[@class='k-grid-content k-auto-scrollable']//tr[2]/td[1]"));
        public string ElectReliabilityRiskLikelihoodValue => Driver.FindElementText(By.XPath("//div[@class='k-grid-content k-auto-scrollable']//tr[3]/td[1]"));
        public string EmpProductivityImpactConsequenceValue => Driver.FindElementText(By.XPath("//div[@class='k-grid-content k-auto-scrollable']//tr[2]//td[@class='cl-numeric-col'][1]"));
        public string EmpProductivityImpactLikelihoodValue => Driver.FindElementText(By.XPath("//div[@class='k-grid-content k-auto-scrollable']//tr[3]/td[1]"));
        public string ComplianceRiskConsequenceValue => Driver.FindElementText(By.XPath("(//div[@class='k-grid-content k-auto-scrollable']//tr[@class='k-alt']//td[@class='cl-numeric-col'][1]//div)[2]"));
        public string ComplianceRiskLikelihoodValue => Driver.FindElementText(By.XPath("//div[@class='k-grid-content k-auto-scrollable']//tr[3]/td[1]/div[2]"));
        public string PublicPropertyRiskConsequenceValue => Driver.FindElementText(By.XPath("(//div[@class='k-grid-content k-auto-scrollable']//tr[@class='k-alt']//div)[2]"));
        public string PublicPropertyRiskLikelihoodValue => Driver.FindElementText(By.XPath("//div[@class='k-grid-content k-auto-scrollable']//tr[3]/td[1]/div[2]"));
        public string IndustrialSafetyRiskConsequenceValue => Driver.FindElementText(By.XPath("(//div[@class='cl-breakdown-grid-container k-pane']//td[@class='cl-numeric-col'][1]/div)[4]"));
        public string IndustrialSafetyRiskLikelihoodValue => Driver.FindElementText(By.XPath("(//div[@class='k-grid-content k-auto-scrollable']//tr[3]//div)[2]"));
        public string TransmissionReliabilityRiskConsequenceValue => Driver.FindElementText(By.XPath("(//div[@class='cl-breakdown-grid-container k-pane']//div[@class='k-grid-content k-auto-scrollable']//tr[@class='k-alt'][1]//div)[2]"));
        public string TransmissionReliabilityRiskLikelihoodValue => Driver.FindElementText(By.XPath("(//div[@class='k-grid-content k-auto-scrollable']//tr[3]//td[@class='cl-numeric-col']//div)[2]")); //(//div[@class='k-grid-content k-auto-scrollable']//tr[3]//div)[2]
                                                                                                                                                                                                         //(//div[@class='k-grid-content k-auto-scrollable']//tr[@class='k-alt'][1]//div)[4]                                                                      //
    }
}
