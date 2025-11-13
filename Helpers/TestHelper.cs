using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ECommerceAutomation.Helpers
{
    /// <summary>
    /// Common utility methods for test execution
    /// </summary>
    public static class TestHelper
    {
        private const int MaxRetries = 3;
        private const int RetryDelayMs = 1000;

        /// <summary>
        /// Waits for an element to be visible and returns it with retry logic
        /// </summary>
        public static IWebElement WaitForElement(IWebDriver driver, By locator, int timeoutSeconds = 15)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));

            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                try
                {
                    return wait.Until(drv =>
                    {
                        try
                        {
                            var element = drv.FindElement(locator);
                            return element.Displayed ? element : null;
                        }
                        catch (StaleElementReferenceException)
                        {
                            return null;
                        }
                        catch (NoSuchElementException)
                        {
                            return null;
                        }
                    });
                }
                catch (WebDriverTimeoutException) when (attempt < MaxRetries)
                {
                    Thread.Sleep(RetryDelayMs);
                    continue;
                }
            }

            throw new WebDriverTimeoutException($"Element with locator '{locator}' not found after {MaxRetries} attempts");
        }

        /// <summary>
        /// Waits for element to be clickable and clicks it with retry logic
        /// </summary>
        public static void WaitAndClick(IWebDriver driver, By locator, int timeoutSeconds = 15)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));

            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                try
                {
                    var element = wait.Until(drv =>
                    {
                        try
                        {
                            var el = drv.FindElement(locator);
                            return el.Displayed && el.Enabled ? el : null;
                        }
                        catch (StaleElementReferenceException)
                        {
                            return null;
                        }
                    });
                    element.Click();
                    return; // Success, exit method
                }
                catch (ElementClickInterceptedException) when (attempt < MaxRetries)
                {
                    // Element might be covered by another element, scroll and retry
                    try
                    {
                        var element = driver.FindElement(locator);
                        ScrollToElement(driver, element);
                        Thread.Sleep(RetryDelayMs);
                    }
                    catch
                    {
                        Thread.Sleep(RetryDelayMs);
                    }
                }
                catch (StaleElementReferenceException) when (attempt < MaxRetries)
                {
                    Thread.Sleep(RetryDelayMs);
                    continue;
                }
            }

            throw new Exception($"Failed to click element with locator '{locator}' after {MaxRetries} attempts");
        }

        /// <summary>
        /// Takes a screenshot and saves it to the Screenshots folder
        /// </summary>
        public static string TakeScreenshot(IWebDriver driver, string testName)
        {
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var fileName = $"{testName}_{timestamp}.png";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots", fileName);

            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Screenshots"));
            screenshot.SaveAsFile(filePath);

            return filePath;
        }

        /// <summary>
        /// Scrolls element into view
        /// </summary>
        public static void ScrollToElement(IWebDriver driver, IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
            Thread.Sleep(500); // Brief pause after scroll
        }
    }
}