using System;
using System.Collections.Generic;
using MeasureFormula.UITests.Bases;
using MeasureFormula.UITests.ExtensionMethods;
using MeasureFormula.UITests.Pages;
using NUnit.Framework;

namespace MeasureFormula.UITests.Tests
{
    public class InvestmentTests : TestBase
    {
        private readonly List<Uri> _investmentsToRemoveLinks = new List<Uri>();
        private InvestmentDetailsPage _investmentDetailsPage;


        public override void BeforeAll()
        {
            Actions.LoginAsAdmin(Driver);
        }

        public override void AfterEach()
        {
            foreach (var link in _investmentsToRemoveLinks)
            {
                _investmentDetailsPage = new InvestmentDetailsPage(Driver);
                Driver.Navigate().GoToUrl(link);
                _investmentDetailsPage.Delete();
            }
            
        }

        [Test]
        public void CanCloneInvestmentWithAddedValueModel()
        {
            const string investmentName = "AutoTest_Investment_to_clone";
            const string expectedValueModelName = "Simple Financial Risk";
            const int expectedValueModelCount = 1;

            var clonedInvestmentName = investmentName + "_cloned_" + $"{DateTime.Now:yyyy-MM-dd_hh-mm}";
            const string clonedInvestmentFacility = "PWP / Power Plant";

            //open the existing investment with the alternative and added value model
            var investmentDetailsPage = Actions.OpenInvestmentDetailsPage(Driver, investmentName);

            //clone the investment
            investmentDetailsPage.MenuBar.OpenHoverLinkPanel(index: 3);
            var investmentCloningPage = investmentDetailsPage.MenuBar.OpenInvestmentCloningPage();
            investmentDetailsPage = investmentCloningPage.Clone(clonedInvestmentName, clonedInvestmentFacility);
            _investmentsToRemoveLinks.Add(investmentDetailsPage.Url);

            //check the cloned investment has the expected value model
            var alternativeValuePage = investmentDetailsPage.OpenAlternativeValuePage();
            alternativeValuePage.SelectInvestment();

            Assert.AreEqual(expectedValueModelCount, alternativeValuePage.AddedValueModelNames.Count);
            Assert.AreEqual(expectedValueModelName, alternativeValuePage.AddedValueModelNames[0]);
        }
    }
}
