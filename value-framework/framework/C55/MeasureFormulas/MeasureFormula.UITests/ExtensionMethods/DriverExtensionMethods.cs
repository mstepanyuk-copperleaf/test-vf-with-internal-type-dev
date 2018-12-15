using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace MeasureFormula.UITests.ExtensionMethods
{
    public static class DriverExtensionMethods
    {
        /// <summary>
        /// A class containing custom methods that make Chrome Driver do extra actions
        /// e.g., wait for the element to become displayed and enabled, or return the text value of 
        /// the element
        /// </summary>

        /// <summary>
        /// A method to find a web element matching the selector, and wait until it is displayed and enabled 
        /// </summary>
        public static IWebElement FindElementWait(this RemoteWebDriver driver, By by, int timeout = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            IEnumerable<IWebElement> foundElements = null;
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

            wait.Until(d =>
            {
                try
                {
                    foundElements = driver.FindElements(by).Where(x => x.Displayed && x.Enabled).ToArray();
                    var isFound = foundElements.Any();

                    //poke the first element's property to make sure it is not stale
                    if (isFound) foundElements.First().Text.ToString(); 
                    return isFound;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            });
            return foundElements.First();

        }

        /// <summary>
        /// A method to find a text value of the found web element which is displayed and enabled 
        /// returns the first
        /// </summary>
        public static string FindElementText(this RemoteWebDriver driver, By by)
        {
            var element = FindElementWait(driver, by);
            return element.Text;
        }

        /// <summary>
        /// A method to find the text values of all web elements found which are displayed and enabled
        /// </summary>
        /// <returns></returns>
        public static List<string> FindElementsText(this RemoteWebDriver driver, By by)
        {
            var elements = driver.FindElementsExceptionIfNotFound(by);
            return elements.Select(element => element.Text).ToList();
        }

        /// <summary>
        /// A method to find web elements matching the selector, and wait until they are displayed and enabled 
        /// </summary>
        public static IEnumerable<IWebElement> FindElementsExceptionIfNotFound(this RemoteWebDriver driver, By by, int timeoutSeconds = 5)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
            IEnumerable<IWebElement> foundElements = null;

            wait.Until(d =>
            {
                try
                {
                    foundElements = driver.FindElements(@by).Where(x => x.Displayed && x.Enabled);
                    return foundElements.Any();
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
            });

            return foundElements;
        }

        /// <summary>
        /// Finds elements on the page or returns null
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IWebElement> FindElementsNullIfNotFound(this RemoteWebDriver driver, By by,
            int timeoutSeconds = 5)
        {
            IEnumerable<IWebElement> elements = new List<IWebElement>();
           try
            {
                elements = FindElementsExceptionIfNotFound(driver, by);
            }
            catch (WebDriverTimeoutException) { }

            return elements;
        }

        /// <summary>
        /// Finds an element on the page or returns null
        /// </summary>
        public static IWebElement FindElementOrNull(this RemoteWebDriver driver, By by, int timeoutSeconds = 5)
        {
            return FindElementsNullIfNotFound(driver, by, timeoutSeconds).First();
        }

        /// <summary>
        /// A method that pauses the code until a specified element is present and clickable on the page
        /// </summary>
        public static void WaitUntilElementIsClickable(this RemoteWebDriver driver, IWebElement element, int timeout = 5)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            wait.Until(ExpectedConditions.ElementToBeClickable(element));
        }

        /// <summary>
        /// A method that checks if the element has 'display:block' attribute
        /// </summary>
        public static bool IsElementVisible(this RemoteWebDriver driver, By by, int timeout = 5)
        {
            return driver.FindElement(by).GetAttribute("style").Replace(" ", "").Contains("display:block");
        }

    }
}
