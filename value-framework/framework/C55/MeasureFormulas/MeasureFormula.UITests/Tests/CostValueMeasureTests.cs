using System;
using System.Globalization;
using MeasureFormula.UITests.Bases;
using MeasureFormula.UITests.enums;
using MeasureFormula.UITests.ExtensionMethods;
using NUnit.Framework;

namespace MeasureFormula.UITests.Tests
{
    [Ignore("October 05. Needs refactoring to set up 10% inflation")]
    public class CostValueMeasureTests : TestBase
    {
        private const string PositiveSpendInvestmentName = "AutoTest_Investment_CostValueMeasure_Positive_Spend";
        private const string NegativeSpendInvestmentName = "AutoTest_Investment_CostValueMeasure_Negative_Spend";

        public override void BeforeAll()
        {
            Actions.LoginAsAdmin(Driver);
        }
        
        [Test]
        public void SumAndInflation_PositiveSpendInflatedDollars_CostValueMeasureDisplaysMatchingValue()
        {
            //Preconditions:
            // An investment exists which has:
            // Investment Cost Value model
            // a forecast with 2 spend lines of CAP account type with
            // - positive values $100,000 and $50,000 for FY2018
            // - positive values $10,000 and $20,000 for FY2019
            // 2 spend lines of O&M account type with 
            // - positive values $150,000 and $150,000 for FY2018
            // - positive values $1,000 and $3,000 for FY2019
            // note: when the values are entered they are inflated already

            var questionnairesTab = Actions.OpenInvestmentQuestionnairesTab(Driver, PositiveSpendInvestmentName);
            questionnairesTab.SelectValueModel(ValueModels.InvestmentCost);
            var valueMeasureGrid = questionnairesTab.SelectValueDetailsTab();
            valueMeasureGrid.SetStartDatePickerValue("2019");
            valueMeasureGrid.SelectOptions(ValueDetailsTabValueFilters.Outcome);

            const string expectedCapexOutcomeValue = "$30,000";
            const string expectedOpexOutcomeValue = "$4,000";
            double expectedTotalOutcomeValue = double.Parse(expectedCapexOutcomeValue.Substring(1, 6), NumberStyles.AllowThousands) 
                                             + double.Parse(expectedOpexOutcomeValue.Substring(1, 5), NumberStyles.AllowThousands);

            var capexOutcomeValue = valueMeasureGrid.GetCapexOutcomeValue;
            Assert.That(capexOutcomeValue, Is.EqualTo(expectedCapexOutcomeValue));

            var opexOutcomeValue = valueMeasureGrid.GetOpexOutcomeValue;
            Assert.That(opexOutcomeValue, Is.EqualTo(expectedOpexOutcomeValue));

            var totalOutcomeValue = valueMeasureGrid.GetTotalOutcomeValue;
            totalOutcomeValue = totalOutcomeValue.Substring(1).Replace(",", "");
            Assert.That(totalOutcomeValue, Is.EqualTo(expectedTotalOutcomeValue.ToString(CultureInfo.InvariantCulture)));
        }

        [Test]
        public void SumAndInflation_NegativeSpendInflatedDollars_CostValueMeasureDisplaysMatchingValue()
        {
            //Preconditions:
            // An investment exists which has
            // a forecast with 
            // - 2 spend lines of CAP account type with negative values -$100,000 and -$50,000 for FY18.
            // - 2 spend lines of CAP account type with negative values -$10,000 and -$20,000 for FY19.
            // - 2 spend lines of O&M account type with negative values -$20,000 and -$20,000 for FY18.
            // - 2 spend lines of O&M account type with negative values -$5,000 and -$7,000 for FY19.

            var questionnairesTab = Actions.OpenInvestmentQuestionnairesTab(Driver, NegativeSpendInvestmentName);
            questionnairesTab.SelectValueModel(ValueModels.InvestmentCost);
            var valueMeasureGrid = questionnairesTab.SelectValueDetailsTab();
            valueMeasureGrid.SetStartDatePickerValue("2019");
            valueMeasureGrid.SelectOptions(ValueDetailsTabValueFilters.Outcome);

            const string expectedCapexOutcomeValue = "($30,000)"; //for FY19
            const string expectedOpexOutcomeValue = "($12,000)";  //for FY19
            const string expectedTotalOutcomeValue = "($42,000)";  //for FY19

            var capexOutcomeValue = valueMeasureGrid.GetCapexOutcomeValue;
            Assert.That(capexOutcomeValue, Is.EqualTo(expectedCapexOutcomeValue));

            var opexOutcomeValue = valueMeasureGrid.GetOpexOutcomeValue;
            Assert.That(opexOutcomeValue, Is.EqualTo(expectedOpexOutcomeValue));

            var totalOutcomeValue = valueMeasureGrid.GetTotalOutcomeValue;
            Assert.That(totalOutcomeValue, Is.EqualTo(expectedTotalOutcomeValue));
        }

        [Test, Ignore("Requires inflation. Needs refactoring")]
        public void BenefitValueModel_CostValueMeasureDisplaysInflatedValue()
        {
            //Preconditions:
            // An investment exists with Benefit model (only Benefit-related value model should be selected)
            // default inflation rate is 10%

            const double inflationRate = 1.1;

            var urlToInflationPage = BaseWebsiteUrl
                                     + "Pages/Rates/Inflation/Views/InflationTimeSeriesEditor.aspx?selectedEntityId=461";
            Driver.Navigate().GoToUrl(urlToInflationPage);
            var questionnairesTab = Actions.OpenInvestmentQuestionnairesTab(Driver, PositiveSpendInvestmentName);
            questionnairesTab.SelectValueModel(ValueModels.BrandValueBenefit);
            var valueMeasureGrid = questionnairesTab.SelectValueDetailsTab();
            valueMeasureGrid.SelectOptions(ValueDetailsTabValueFilters.Outcome);
            var currentFyValue = valueMeasureGrid.GetCurrentFiscalYearOutcomeValue.Substring(1);
            var actualFiscalYear19Value = valueMeasureGrid.GetCurrentFiscalYearOutcomeValue.Substring(1).Replace(",", "");
            var actualFiscalYear20Value = valueMeasureGrid.GetNextFiscalYearOutcomeValue.Substring(1).Replace(",", "");

            var expectedFy19Value = double.Parse(currentFyValue) * Math.Pow(inflationRate, 0);
            var expectedFy20Value = double.Parse(currentFyValue) * Math.Pow(inflationRate, 1);

            Assert.That(actualFiscalYear19Value, Is.EqualTo(expectedFy19Value.ToString(CultureInfo.InvariantCulture)));
            Assert.That(actualFiscalYear20Value, Is.EqualTo(expectedFy20Value.ToString(CultureInfo.InvariantCulture)));
        }

    }
}
