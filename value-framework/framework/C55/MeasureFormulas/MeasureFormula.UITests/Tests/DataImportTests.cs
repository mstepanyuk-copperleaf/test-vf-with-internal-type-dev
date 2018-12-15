using System.IO;
using MeasureFormula.UITests.Bases;
using MeasureFormula.UITests.ExtensionMethods;
using MeasureFormula.UITests.Helpers;
using MeasureFormula.UITests.Pages;
using MeasureFormula.UITests.TestAttributes;
using NUnit.Framework;

namespace MeasureFormula.UITests.Tests
{
    public class DataImportTests : TestBase
    {
        private string ValueFrameworkFilePath => Path.GetTempPath() + "vflibrary-base-framework.xlsx";
        private AmazonS3DownloadFileHelper _sender;


        public DataImportTests()
        {
            _sender = new AmazonS3DownloadFileHelper();
        }

        public override void BeforeEach()
        {
            Actions.LoginAsAdmin(Driver);
        }


        [Test, CategorySmoke]
        public void Can_import_latest_value_framework_BASE()
        {
            _sender.DownloadValueFrameworkFile("vflibrary-base/dev/latest/framework-output.xlsx");
            var dataImportPage = DataImportPage.NavigateToThisPageViaUrl(Driver);
            Assert.IsTrue(dataImportPage.ImportFile(ValueFrameworkFilePath));

            var errorLogPage = ErrorLogPage.NavigateToThisPageViaUrl(Driver);
            var sussessfulImport = errorLogPage.GetDataImportMessageAndParse();
            Assert.IsTrue(sussessfulImport);
        }

        private static string GetLocalFilePath()
        {
            var assemblyLocalPath = AssemblyPathManager.SetupAssemblyPath();
            return new FileInfo(Path.Combine(assemblyLocalPath,
                @"..\..\..\..\..\..\tests\UITests_Test_Data_Loader.xlsx")).FullName;
        }


    }
}
