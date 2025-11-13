using OpenQA.Selenium;
using ECommerceAutomation.Helpers;

namespace ECommerceAutomation.Pages
{
    /// <summary>
    /// Page Object Model for Home Page
    /// </summary>
    public class HomePage
    {
        private readonly IWebDriver _driver;

        // Locators
        private readonly By _signupLoginLink = By.XPath("//a[@href='/login']");
        private readonly By _productsLink = By.XPath("//a[@href='/products']");
        private readonly By _loggedInAsText = By.XPath("//a[contains(text(),'Logged in as')]");
        private readonly By _deleteAccountLink = By.XPath("//a[@href='/delete_account']");

        public HomePage(IWebDriver driver)
        {
            _driver = driver;
        }

        /// <summary>
        /// Navigate to home page
        /// </summary>
        public void NavigateToHomePage()
        {
            _driver.Navigate().GoToUrl("https://www.automationexercise.com");
        }

        /// <summary>
        /// Click on Signup/Login link
        /// </summary>
        public void ClickSignupLogin()
        {
            TestHelper.WaitAndClick(_driver, _signupLoginLink);
        }

        /// <summary>
        /// Click on Products link
        /// </summary>
        public void ClickProducts()
        {
            TestHelper.WaitAndClick(_driver, _productsLink);
        }

        /// <summary>
        /// Verify user is logged in
        /// </summary>
        public bool IsUserLoggedIn(string username)
        {
            var element = TestHelper.WaitForElement(_driver, _loggedInAsText);
            return element.Text.Contains(username);
        }

        /// <summary>
        /// Click Delete Account link
        /// </summary>
        public void ClickDeleteAccount()
        {
            TestHelper.WaitAndClick(_driver, _deleteAccountLink);
        }
    }
}