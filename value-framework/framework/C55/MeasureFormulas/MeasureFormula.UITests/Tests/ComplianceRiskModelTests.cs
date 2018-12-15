using System.Collections.Generic;
using System.Linq;
using MeasureFormula.UITests.Bases;
using MeasureFormula.UITests.enums;
using MeasureFormula.UITests.ExtensionMethods;
using NUnit.Framework;

namespace MeasureFormula.UITests.Tests
{
    public class ComplianceRiskModelTests : TestBase
    {
        private const string InvestmentName = "AutoTest_Investment_ComplianceRisk";

        public override void BeforeAll()
        {
            Actions.LoginAsAdmin(Driver);
        }

        [Test]
        public void CheckValueMeasureValuesAreConsistent()
        {
            var list = new List<string>();

            //get values in Value Model and Value Measure grids
            var questionnairesTab = Actions.OpenInvestmentQuestionnairesTab(Driver, InvestmentName);
            questionnairesTab.SelectValueModel(ValueModels.ComplianceRisk);
            var valueMeasureGrid = questionnairesTab.SelectValueDetailsTab();
            valueMeasureGrid.SelectValueMeasure(ValueMeasures.ComplianceRisk);
            list.Add(valueMeasureGrid.GetValue("Compliance Risk"));
            list.Add(questionnairesTab.GetValueChartValue);

            //get value on Answer Questionnaires dialog
            var answerQuestionnairesDialog = Actions.OpenQuestionnairesDialog(Driver, InvestmentName, ValueModels.ComplianceRisk);
            list.Add(answerQuestionnairesDialog.GetValue);
            questionnairesTab = answerQuestionnairesDialog.Close();

            //get value on Value Chart tab
            list.Add(questionnairesTab.GetValueChartValue);
            Assert.That(list.Count, Is.GreaterThan(1));

            var allValuesAreSame = list.Distinct().Count() == 1;
            Assert.IsTrue(allValuesAreSame, "Values differ.");
        }

    }
}
