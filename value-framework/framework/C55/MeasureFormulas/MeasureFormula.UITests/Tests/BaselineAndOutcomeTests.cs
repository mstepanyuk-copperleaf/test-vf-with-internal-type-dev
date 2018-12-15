using MeasureFormula.UITests.Bases;
using MeasureFormula.UITests.enums;
using MeasureFormula.UITests.ExtensionMethods;
using NUnit.Framework;

namespace MeasureFormula.UITests.Tests
{
    public class BaselineAndOutcomeTests : TestBase
    {
        private const string InvestmentName = "AutoTest_Investment_SafetyRisk";

        public override void BeforeAll()
        {
            Actions.LoginAsAdmin(Driver);
        }

        [Test]
        public void RiskValueModel_Spendline_OutcomeEqualsBaselineBeforeImpactYearAndZeroAfterImpactYear()
        {
            //Preconditions:
            // An investment exists which has:
            // - Safety Risk Value model (any risk-related model can be used)
            // - a forecast with 1 spend line with $20,000 for FY2019

            const string fy18 = "2018";
            var answerQuestionnairesDialog = Actions.OpenQuestionnairesDialog(Driver, InvestmentName, ValueModels.SafetyRisk);
            answerQuestionnairesDialog.SetStartDatePickerValue(fy18);

            var baselineFy18 = answerQuestionnairesDialog.ValuesforFY18[0];
            var outcomeFy18 = answerQuestionnairesDialog.ValuesforFY18[5];
            var baselineFy19 = answerQuestionnairesDialog.ValuesforFY19[0];
            var outcomeFy19 = answerQuestionnairesDialog.ValuesforFY19[5];
            var outcomeFy20 = answerQuestionnairesDialog.ValuesforFY20[5];

            Assert.AreEqual(baselineFy18, outcomeFy18);
            Assert.AreEqual(baselineFy19, outcomeFy19);
            Assert.That(outcomeFy20 == "0");
        }
    }
}
