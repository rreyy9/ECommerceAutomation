using ECommerceAutomation.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Linq;

namespace ECommerceAutomation.Tests
{
    /// <summary>
    /// Base test class with common setup and teardown logic
    /// </summary>
    [TestClass]
    public class BaseTest
    {
        protected IWebDriver Driver;
        public TestContext TestContext { get; set; }
        protected const string BaseUrl = "https://www.automationexercise.com";

        /// <summary>
        /// Test context property for logging and test information
        /// </summary>
        public TestContext TestContextInstance
        {
            get { return TestContext; }
            set { TestContext = value; }
        }

        /// <summary>
        /// Initialize WebDriver before each test
        /// Can be overridden to specify browser type
        /// </summary>
        [TestInitialize]
        public virtual void Setup()
        {
            // Default to Chrome, can be overridden in derived classes
            var browserType = WebDriverFactory.BrowserType.Chrome;

            // Check if browser type is specified in test context (for data-driven tests)
            if (TestContext.Properties.Contains("BrowserType"))
            {
                Enum.TryParse(TestContext.Properties["BrowserType"].ToString(), out browserType);
            }

            Log($"Initializing {browserType} browser...");
            Driver = WebDriverFactory.CreateDriver(browserType);
            Log("Browser initialized successfully");
        }

        /// <summary>
        /// Clean up after each test
        /// </summary>
        [TestCleanup]
        public virtual void Teardown()
        {
            if (Driver != null)
            {
                // Take screenshot on failure
                if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed)
                {
                    Log("Test failed - capturing screenshot");
                    var screenshotPath = TestHelper.TakeScreenshot(Driver, TestContext.TestName);
                    Log($"Screenshot saved: {screenshotPath}");
                }

                Log("Closing browser...");
                Driver.Quit();
                Driver.Dispose();
                Log("Browser closed successfully");
            }
        }

        /// <summary>
        /// Helper method to log messages to test output
        /// </summary>
        protected void Log(string message)
        {
            TestContext?.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
        }

        /// <summary>
        /// Navigate to a specific URL
        /// </summary>
        protected void NavigateTo(string url)
        {
            Log($"Navigating to: {url}");
            Driver.Navigate().GoToUrl(url);
            Log("Navigation complete");
        }
    }
}