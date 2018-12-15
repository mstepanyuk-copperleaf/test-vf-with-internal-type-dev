using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using MeasureFormula.UITests.Bases;
using MeasureFormula.UITests.enums;
using MeasureFormula.UITests.ExtensionMethods;
using MeasureFormula.UITests.Helpers;
using NUnit.Framework;
using Octokit;

namespace MeasureFormula.UITests.Tests
{
    public class ValueModelCalculationsFromExcelTests : TestBase
    {
        private ExcelParser _excelParser;
        private static DirectoryInfo _directory;

        public override void BeforeAll()
        {
            DownloadSampleCalcFiles();
            Actions.LoginAsUser(Driver);
        }

        public override void AfterEach()
        {
            _excelParser.Close();
        }

        public override void AfterAll()
        {
            _directory.Delete(recursive: true);
            base.AfterAll();
        }

        [Test]
        [TestCaseSource(nameof(GetListOfSampleCalcFilesAndSaveToFile))]
        public void GenericTest_for_Value_Models(string sampleCalcFileName)
        {
            double actualResult;

            //get a value model name from sample calculations file
            _excelParser = new ExcelParser(sampleCalcFileName);
            var modelName = _excelParser.GetTabName();
            var investmentName = $"AutoTest_Investment_{modelName}";

            //open an investment with a required value model
            var questionnairesTab = Actions.OpenInvestmentQuestionnairesTab(Driver, investmentName);
            questionnairesTab.SelectValueModel(modelName.Replace('_', ' '));
            var valueMeasureGrid = questionnairesTab.SelectValueDetailsTab();

            //choose a required fiscal year
            var FY = "20" + _excelParser.GetFiscalYear();
            valueMeasureGrid.SetStartDatePickerValue(FY);

            var expectedBaselineTotal = _excelParser.GetTotalValue(indexOffset:1);
            if (expectedBaselineTotal != null)
            {
                valueMeasureGrid.SelectOptions(ValueDetailsTabValueFilters.Baseline);
                var value = valueMeasureGrid.GetTotalValueForSelectedFY;
                if (value.Contains("$"))
                {
                    value = value.Substring(1);
                }
                actualResult = double.Parse(value);
                Assert.AreEqual(expectedBaselineTotal, actualResult);
            }

            var expectedOutcomeTotal = _excelParser.GetTotalValue(indexOffset:2);
            if (expectedOutcomeTotal != null)
            {
                valueMeasureGrid.SelectOptions(ValueDetailsTabValueFilters.Outcome);
                var value = valueMeasureGrid.GetTotalValueForSelectedFY;
                if (value.Contains("$"))
                {
                    value = value.Substring(1);
                }
                actualResult = double.Parse(value);
                Assert.AreEqual(expectedOutcomeTotal, actualResult);
            }

            var expectedChangeTotal = _excelParser.GetTotalValue(indexOffset: 3);
            if (expectedBaselineTotal == null) return;
            {
                valueMeasureGrid.SelectOptions(ValueDetailsTabValueFilters.Change);
                var value = valueMeasureGrid.GetTotalValueForSelectedFY;
                if (value.Contains("$"))
                {
                    value = value.Substring(1);
                }
                actualResult = double.Parse(value);
                Assert.AreEqual(expectedChangeTotal, actualResult);
            }
        }

        private static IEnumerable<string> GetListOfSampleCalcFilesAndSaveToFile()
        {
            DownloadSampleCalcFiles();

            const string pathToFolder = "C:\\temp_folder_for_downloaded_files\\";
            DirectoryInfo directory = new DirectoryInfo(pathToFolder);
            FileInfo[] files = directory.GetFiles("*SampleCalc*"); //
            if(files.Length == 0) throw new ApplicationException("No files found.");
            return files.Select(x => x.Name);
        }

        private static void DownloadSampleCalcFiles()
        {
            _directory = Directory.CreateDirectory("C:\\temp_folder_for_downloaded_files\\");

            var gitHubClient = new GitHubClient(new ProductHeaderValue("my-auto-test"));
            var tokenAuth = new Credentials("4de7529233af3ff96002f3a16b4331302b5f2935");
            gitHubClient.Credentials = tokenAuth;
            var repos = gitHubClient.Repository.GetAllForCurrent().Result;
            var repoId = repos.Single(x => x.Name == "vflibrary-base" && x.Fork == false).Id;
            var contents = gitHubClient.Repository.Content.GetAllContents(repoId, "value-framework/tests").Result;

            var filteredFiles = contents.Where(x => x.Name.Contains("_SampleCalc_"));
            foreach (var content in filteredFiles)
            {
                var myWebClient = new WebClient();
                byte[] myDataBuffer = myWebClient.DownloadData(content.DownloadUrl);
                File.WriteAllBytes($"{_directory.FullName}\\{content.Name}", myDataBuffer);
            }
        }
    }
}
