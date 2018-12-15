using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Threading;
using MeasureFormula.UITests.Helpers;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;


namespace MeasureFormula.UITests.Bases
{
    /// <summary>
    /// Base Class to define the functionality all test fixtures will share.
    /// </summary>
    public abstract class TestBase
    {
        protected readonly RemoteWebDriver Driver;
        protected static S3ScreenshotSender s3ScreenshotSender;
        protected static readonly string BaseWebsiteUrl = new Uri(ConfigurationManager.AppSettings["baseWebsiteUrl"]).ToString();
        private readonly string _initialWindowHandle;
        private static readonly string AssemblyLocalPath = AssemblyPathManager.SetupAssemblyPath();
        private static readonly string OutputFolderPath = AssemblyLocalPath + @"\..\..\..\Output\Screenshots\";

        protected TestBase()
        {
            var driverName = ConfigurationManager.AppSettings.Get("driver_name");
            Directory.CreateDirectory(OutputFolderPath);
            switch (driverName) {
                case "PhantomJS":
                    Driver = CreatePhantomDriver();
                    s3ScreenshotSender = new S3ScreenshotSender();
                    break;
                default:
                    Driver = CreateChromeDriver();
                    break;
            }

            _initialWindowHandle = Driver.CurrentWindowHandle;
        }

        [OneTimeSetUp]
        public virtual void BeforeAll() { }

        [SetUp]
        public virtual void BeforeEach() { }

        [TearDown]
        public virtual void AfterEach()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                AfterEachFailed();
            }
        }

        public virtual void AfterEachFailed()
        {
            SaveScreenshot();

            //ensure the driver is set to the original tab opened
            Driver.SwitchTo().Window(_initialWindowHandle);
        }

        [OneTimeTearDown]
        public virtual void AfterAll()
        {
            Driver.Quit();
        }

        private static RemoteWebDriver CreatePhantomDriver()
        {
            var pathToPhantom = ConfigurationManager.AppSettings.Get("phantom_executable_path");
            PhantomJSOptions options = new PhantomJSOptions();
            options.AddAdditionalCapability("phantomjs.page.settings.userAgent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36");
            options.AddAdditionalCapability("phantomjs.page.settings.resourceTimeout", "5000");
            var driver = new PhantomJSDriver(pathToPhantom, options);
            driver.Manage().Window.Size = new Size(1800, 900);

            return driver;
        }

        private static RemoteWebDriver CreateChromeDriver()
        {
            //get the path to chrome.exe file
            var pathToChromeExecutable = new FileInfo(Path.Combine(AssemblyLocalPath,
                @"..\..\..\packages\Selenium.WebDriver.ChromeDriver.2.38.0.1\driver\win32")).FullName;

            //set an option to disable 'Save Password' prompt in the browser
            var options = new ChromeOptions();
            options.AddUserProfilePreference("credentials_enable_service", false);
            options.AddUserProfilePreference("password_manager_enabled", false);

            var driver = new ChromeDriver(pathToChromeExecutable, options)
            {
                Url = BaseWebsiteUrl 
            }; 

            driver.Manage().Window.Size = new Size(1800, 900);
            return driver;
        }

        private void SaveScreenshot()
        {
            if (Driver == null) return;

            //generate a screenshot name
            var testName = TestContext.CurrentContext.Test.Name;
            var fileName = $"{DateTime.Now:yyyy-MM-dd_hh-mm}-{testName}.png";
            var fullPath = Path.Combine(OutputFolderPath, fileName);

            Screenshot screenshot = ((ITakesScreenshot) Driver).GetScreenshot();
            //need to wait until the screenshot is taken
            Thread.Sleep(TimeSpan.FromSeconds(0.5));

            //need to wait until the screenshot is saved as a file
            screenshot.SaveAsFile(fullPath, ScreenshotImageFormat.Png);
            Thread.Sleep(TimeSpan.FromSeconds(0.5));
            if (s3ScreenshotSender != null)
            {
                s3ScreenshotSender.SaveScreenshot(fullPath, fileName, testName).Wait();
            }
        }
    }
}
