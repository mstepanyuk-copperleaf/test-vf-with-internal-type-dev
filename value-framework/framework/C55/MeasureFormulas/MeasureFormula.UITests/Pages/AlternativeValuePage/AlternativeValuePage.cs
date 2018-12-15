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
    public class AlternativeValuePage : PageBase
    {
        public AlternativeValuePage(RemoteWebDriver driver) : base(driver)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(3));
            wait.Until(d => Driver.FindElementById("busyIndicator").GetAttribute("class").Contains("hide"));
        }

        public IWebElement ValueModelsTab => Driver.FindElementById("valueModelsTab");
        public string Url => Driver.Url;
        public string QuestionnaireTabUrl => Url.Split('&')[0];
        public bool IsAdditionalValueModelsSectionDisplayed =>
            Driver.FindElements(By.XPath("//tr[contains(., 'Additional')]")).Count > 0;

        public List<string> AddedValueModelNames => AddedValueModels.Select(x => x.Text).Where((x, i) => i % 3 == 0).ToList();

        public IEnumerable<IWebElement> AddedValueModels => Driver.FindElementsNullIfNotFound(By.XPath(
                "//tr[contains(., 'Additional')]/following-sibling::tr/td[@role='gridcell' and not(contains(@style, 'display:none'))]"))
            .Distinct();
        public IEnumerable<IWebElement> AddedAssets => Driver.FindElementsNullIfNotFound(By.XPath("//span[@class='cl-label__asset ']")).ToList();

        public static AlternativeValuePage NavigateToThisPageViaUrl(RemoteWebDriver driver, string partialUrl)
        {
            driver.Navigate().GoToUrl(partialUrl);
            return new AlternativeValuePage(driver);
        }

        public ValueModelsTab OpenValueModelTab()
        {
            ValueModelsTab.Click();
            return new ValueModelsTab(Driver);
        }

        public QuestionnairesTab OpenQuestionnairesTab(string partialUrl)
        {
            Driver.Navigate().GoToUrl(partialUrl + "&tabId=questionnairesTab");
            return new QuestionnairesTab(Driver);
        }

        public void AddValueModelToInvestment(ValueModels model)
        {
            var addValueModelsDialog = OpenAddValueModelsDialog();
            addValueModelsDialog.AddValueModel(model);
        }

        public int GetAvailableValueModelsCount()
        {
            var addValueModelsDialog = OpenAddValueModelsDialog();
            return addValueModelsDialog.AvailableValueModels.Count;
        }

        public AddValueModelsDialog OpenAddValueModelsDialog()
        {
            var addButton = Driver.FindElementWait(By.XPath("//section//a[@title='Add Value Models']"));
                addButton.Click();
            return new AddValueModelsDialog(Driver);
        }

        public void SelectInvestment()
        {
            var investment =
                Driver.FindElementWait(By.XPath(
                    "//table[@class='k-selectable']//div[@class='multiTargetLinkNav-hyperlink']"));
            investment.Click();
        }

        public void DeleteValueModel()
        {
            var list = AddedValueModels;
            while (list.Any())
            {
                list.First().Click();
                var deleteButton = Driver.FindElementWait(By.XPath(
                                    "//section[@class='cl-k__resizable-grid-pane--borderless k-pane']" +
                                    "//span[contains(@class, 'cl-k__icon--delete')]"));
                deleteButton.Click();
                ConfirmDeletion();

                if(!IsAdditionalValueModelsSectionDisplayed) break;
                list = AddedValueModels;
            }
        }

        public void DeleteAssets()
        {
            while (AddedAssets.Any())
            {
                AddedAssets.First().Click();
                var deleteIcon = Driver.FindElementWait(By.XPath("//div[@class='cl-k__resizable-grid-pane--borderless']"
                                                                 + "//span[@class='k-sprite k-icon cl-k__icon--delete']"));
                deleteIcon.Click();
                ConfirmDeletion();
            }
        }

        private void ConfirmDeletion()
        {
            var confirmRemovalDialog = new ConfirmValueModelRemovalDialog(Driver);
            confirmRemovalDialog.Delete();
        }

        public AddImpactedAssetsDialog OpenAddImpactedAssetsDialog()
        {
            var assetsButton = Driver.FindElementWait(By.XPath("//div//a[contains(@title, 'Select Assets')]"));
            assetsButton.Click();
            return new AddImpactedAssetsDialog(Driver);
        }

        public List<string> GetValueModelsForSelectedAsset(string assetName)
        {
            Wait.Until(d => Driver.FindElements(By.XPath("//div[@class='k-overlay']")).Count == 0);
            var asset =
                Driver.FindElementWait(By.XPath($"//span[@title='{assetName}'][1]"));
            asset.Click();
            var list = Driver.
                FindElementsText(By.XPath("//div[@data-k-options='measureSetGridCtrl.measureSetGridOptions']//td[@role='gridcell' "
                                                    + "and not(contains(@style, 'display:none'))]")).ToList();
            return list.Where((x, i) => i % 3 == 0).ToList();
        }
    }
}
