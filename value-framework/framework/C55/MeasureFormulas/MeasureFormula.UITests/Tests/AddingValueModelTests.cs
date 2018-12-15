using System.Collections.Generic;
using System.Linq;
using MeasureFormula.UITests.Bases;
using MeasureFormula.UITests.enums;
using MeasureFormula.UITests.ExtensionMethods;
using MeasureFormula.UITests.Pages.AlternativeValuePage;
using NUnit.Framework;
using OpenQA.Selenium;

namespace MeasureFormula.UITests.Tests
{
    public class AddingValueModelTests : TestBase
    {
        private AlternativeValuePage _alternativeValuePage;
        private string _alternativeValuePageUrl;
        private List<IWebElement> _assetsList;

        public override void BeforeAll()
        {
            Actions.LoginAsAdmin(Driver);
        }

        public override void AfterEach()
        {
            if (Driver.Url != _alternativeValuePageUrl)
            {
                _alternativeValuePage = AlternativeValuePage.NavigateToThisPageViaUrl(Driver, _alternativeValuePageUrl);
            }

            //remove added value models and assets
            _alternativeValuePage.SelectInvestment();
            var valueModelsList = _alternativeValuePage.AddedValueModels;
            _assetsList = _alternativeValuePage.AddedAssets.ToList();

            if (valueModelsList.Any())
            {
                _alternativeValuePage.DeleteValueModel();
            }

            if (_assetsList.Any())
            {
                _alternativeValuePage.DeleteAssets();
            }
        }

        [Test]
        public void CanAddValueModelToInvestmentWithNoForecast()
        {
            const string investmentName = "AutoTest_Investment_with_no_forecast";
            
            //open Alternative Value Page for the investment
            _alternativeValuePage = Actions.OpenAlternativeValuePage(Driver, investmentName);
            _alternativeValuePageUrl = _alternativeValuePage.Url;

            //add a value model
            var initialValueModelsList = _alternativeValuePage.AddedValueModelNames;
            _alternativeValuePage.AddValueModelToInvestment(ValueModels.CyberSecurityRisk);
            var updatedValueModelsList = _alternativeValuePage.AddedValueModelNames;

            Assert.Greater(updatedValueModelsList.Count, initialValueModelsList.Count);
            Assert.That(updatedValueModelsList.Contains("Cyber Security Risk"));

            //switch to Questionnaire tab without any errors
            var questionnairesTab =
                _alternativeValuePage.OpenQuestionnairesTab(_alternativeValuePage.QuestionnaireTabUrl);
            Assert.IsTrue(questionnairesTab.IsDisplayed);
        }

        [Test]
        public void CanAddAllValueModelsToInvestment()
        {
            const string investmentName = "AutoTest_Investment_to_add_all_models";

            //open Alternative Value Page for the investment
            _alternativeValuePage = Actions.OpenAlternativeValuePage(Driver, investmentName);
            _alternativeValuePageUrl = _alternativeValuePage.Url;
            _alternativeValuePage.SelectInvestment();

            //check # of initially added value models if any
            var initialValueModelsCount = _alternativeValuePage.AddedValueModelNames.Count;

            var dialog = _alternativeValuePage.OpenAddValueModelsDialog();
            var availableValueModelsCount = dialog.AvailableValueModels.Count;
            var expectedValueModelsCount = initialValueModelsCount + availableValueModelsCount;

            //add all value models
            dialog.AddAllValueModels();
            var updatedValueModelsCount = _alternativeValuePage.AddedValueModelNames.Count;

            Assert.AreEqual(expectedValueModelsCount, updatedValueModelsCount);
        }

        [Test]
        public void CanAddAssetToInvestment()
        {
            const string investmentName = "AutoTest_Investment_to_add_asset";
            const string assetName = "AutoTest_Asset_Type";
            const int expectedNumberOfValueModels = 2;

            //open Alternative Value Page for the investment
            _alternativeValuePage = Actions.OpenAlternativeValuePage(Driver, investmentName);

            _alternativeValuePageUrl = _alternativeValuePage.Url;
            var addImpactedAssetsDialog = _alternativeValuePage.OpenAddImpactedAssetsDialog();
            _alternativeValuePage = addImpactedAssetsDialog.AddAsset(index:0); //assetName: "AutoTest_Asset_Type";
            var addedlistOfValueModels = _alternativeValuePage.GetValueModelsForSelectedAsset(assetName).Count;

            Assert.AreEqual(expectedNumberOfValueModels, addedlistOfValueModels);

            var questionnairesTab = _alternativeValuePage.OpenQuestionnairesTab(_alternativeValuePage.QuestionnaireTabUrl);
            addedlistOfValueModels = questionnairesTab.ValueModelsForAsset.Count;
            Assert.AreEqual(expectedNumberOfValueModels, addedlistOfValueModels);
        }
    }
}
