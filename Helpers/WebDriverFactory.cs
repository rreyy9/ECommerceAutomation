using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;

namespace ECommerceAutomation.Helpers
{
    /// <summary>
    /// Factory class to create WebDriver instances for different browsers
    /// </summary>
    public class WebDriverFactory
    {
        public enum BrowserType
        {
            Chrome,
            Edge
        }

        /// <summary>
        /// Creates and configures a WebDriver instance for the specified browser
        /// </summary>
        public static IWebDriver CreateDriver(BrowserType browserType)
        {
            IWebDriver driver;

            switch (browserType)
            {
                case BrowserType.Chrome:
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--start-maximized");
                    driver = new ChromeDriver(chromeOptions);
                    break;

                case BrowserType.Edge:
                    var edgeOptions = new EdgeOptions();
                    edgeOptions.AddArgument("--start-maximized");
                    driver = new EdgeDriver(edgeOptions);
                    break;

                default:
                    throw new ArgumentException($"Browser type {browserType} is not supported");
            }

            // Set implicit wait for all elements
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);

            return driver;
        }
    }
}