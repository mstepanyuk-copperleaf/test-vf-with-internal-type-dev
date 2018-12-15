using MeasureFormula.UITests.Bases;
using MeasureFormula.UITests.ExtensionMethods;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace MeasureFormula.UITests.Pages
{
    public class LoginPage : PageBase
    {
        private const string PartialUrl = "Login/UserLogin.aspx?";

        public LoginPage(RemoteWebDriver driver) : base(driver){}

        public static LoginPage NavigateToThisPageViaUrl(RemoteWebDriver driver)
        {
            driver.Navigate().GoToUrl(BaseWebsiteUrl + PartialUrl);
            return new LoginPage(driver);
        }

        public HomePage LogIn(string username, string password)
        {
            LoginField.Clear();
            LoginField.SendKeys(username);
            var passwordField = Driver.FindElementWait(By.Id("password"));
            passwordField.Clear();
            passwordField.SendKeys(password);
            LoginButton.Click();
            return new HomePage(Driver);
        }

        public IWebElement LoginField => Driver.FindElementById("username");
        public IWebElement PasswordField => Driver.FindElementById("password");
        public IWebElement LoginButton => Driver.FindElementById("btnSubmit");
    }
}
