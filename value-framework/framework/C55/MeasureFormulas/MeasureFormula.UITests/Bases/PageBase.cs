using System;
using System.Configuration;
using MeasureFormula.UITests.Pages;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using PageFactory = SeleniumExtras.PageObjects.PageFactory;

namespace MeasureFormula.UITests.Bases
{
    /// <summary>
    /// Base Page to define functionality shared by all pages,
    /// e.g., initialize elements on the page 
    /// </summary>
    public abstract class PageBase
    {
        protected readonly RemoteWebDriver Driver;
        protected WebDriverWait Wait;
        protected static readonly Uri BaseWebsiteUrl = new Uri(ConfigurationManager.AppSettings["baseWebsiteUrl"]);
        public NavigationBar MenuBar => new NavigationBar(Driver);
        public Uri Url => new Uri(Driver.Url);

        protected PageBase(RemoteWebDriver driver)
        {
            Driver = driver;
            PageFactory.InitElements(Driver, this);
            Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        }
    }
}
