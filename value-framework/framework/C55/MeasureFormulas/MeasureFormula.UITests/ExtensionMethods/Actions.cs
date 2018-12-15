using System.Net;
using MeasureFormula.UITests.enums;
using MeasureFormula.UITests.Pages;
using MeasureFormula.UITests.Pages.AlternativeValuePage;
using MeasureFormula.UITests.TestData;
using OpenQA.Selenium.Remote;

namespace MeasureFormula.UITests.ExtensionMethods
{
    /// <summary>
    /// A class containing methods encapsulating steps Chrome Driver requires to do as test pre-conditions
    /// e.g., log in and navigate to a particular page 
    /// </summary>
    public static class Actions
    {
        public static InspectBreakdownDialog OpenBreakdownDialog(
                                            RemoteWebDriver driver,
                                            string investmentName,
                                            ValueModels valueModel,
                                            ValueMeasures valueMeasure)
        {
            var questionnairesTab = OpenInvestmentQuestionnairesTab(driver, investmentName);
            questionnairesTab.SelectValueModel(valueModel);
            var valueDetailsTab = questionnairesTab.SelectValueDetailsTab();
            valueDetailsTab.SelectValueMeasure(valueMeasure);
            return valueDetailsTab.OpenInspectBreakdownDialog();
        }

        public static AnswerQuestionnairesDialog OpenQuestionnairesDialog(
                                        RemoteWebDriver driver,
                                        string investmentName,
                                        ValueModels valueModel)
        {
            var questionnairesTab = OpenInvestmentQuestionnairesTab(driver, investmentName);
            questionnairesTab.SelectValueModel(valueModel);
            return questionnairesTab.OpenAnswerQuestionnairesDialog();
        }

        public static QuestionnairesTab OpenInvestmentQuestionnairesTab(RemoteWebDriver driver, string investmentName)
        {
            var investmentUrl = SearchForInvestment(driver, investmentName);

            //open Alternative Value > Questionnaires Tab
            var alternativeValuePage = AlternativeValuePage.NavigateToThisPageViaUrl(driver, investmentUrl);
            return alternativeValuePage.OpenQuestionnairesTab(investmentUrl);
        }

        public static AlternativeValuePage OpenAlternativeValuePage(RemoteWebDriver driver, string investmentName)
        {
            var investmentUrl = SearchForInvestment(driver, investmentName);
            var page =  AlternativeValuePage.NavigateToThisPageViaUrl(driver, investmentUrl);
            page.SelectInvestment();
            return page;
        }


        public static InvestmentDetailsPage OpenInvestmentDetailsPage(RemoteWebDriver driver, string investmentName)
        {
            var investmentUrl = SearchForInvestment(driver, investmentName);
            return InvestmentDetailsPage.NavigateToThisPageViaUrl(driver, investmentUrl);
        }

        public static string SearchForInvestment(RemoteWebDriver driver, string investmentName)
        {
            var investmentSearchPage = InvestmentSearchPage.NavigateToThisPageViaUrl(driver);
            return investmentSearchPage.SearchForInvestment(investmentName);
        }

        public static HomePage LoginAsAdmin(RemoteWebDriver driver)
        {
            driver.Manage().Cookies.DeleteAllCookies();
            var loginPage = LoginPage.NavigateToThisPageViaUrl(driver);
            return loginPage.LogIn(LoginData.CopperleafAccount.Email, LoginData.CopperleafAccount.Password);
        }

        public static HomePage LoginAsUser(RemoteWebDriver driver)
        {
            driver.Manage().Cookies.DeleteAllCookies();
            var loginPage = LoginPage.NavigateToThisPageViaUrl(driver);
            return loginPage.LogIn(LoginData.CopperleafAccount.Email, LoginData.CopperleafAccount.Password);
        }
    }
}

