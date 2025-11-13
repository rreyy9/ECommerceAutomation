using OpenQA.Selenium;
using ECommerceAutomation.Helpers;

namespace ECommerceAutomation.Pages
{
    /// <summary>
    /// Page Object Model for Login Page
    /// </summary>
    public class LoginPage
    {
        private readonly IWebDriver _driver;

        // Locators for Login section
        private readonly By _loginEmailInput = By.XPath("//input[@data-qa='login-email']");
        private readonly By _loginPasswordInput = By.XPath("//input[@data-qa='login-password']");
        private readonly By _loginButton = By.XPath("//button[@data-qa='login-button']");

        // Locators for Signup section
        private readonly By _signupNameInput = By.XPath("//input[@data-qa='signup-name']");
        private readonly By _signupEmailInput = By.XPath("//input[@data-qa='signup-email']");
        private readonly By _signupButton = By.XPath("//button[@data-qa='signup-button']");

        // Error message locators
        private readonly By _loginErrorMessage = By.XPath("//p[contains(text(),'Your email or password is incorrect')]");

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
        }

        /// <summary>
        /// Enter login email
        /// </summary>
        public void EnterLoginEmail(string email)
        {
            var emailField = TestHelper.WaitForElement(_driver, _loginEmailInput);
            emailField.Clear();
            emailField.SendKeys(email);
        }

        /// <summary>
        /// Enter login password
        /// </summary>
        public void EnterLoginPassword(string password)
        {
            var passwordField = TestHelper.WaitForElement(_driver, _loginPasswordInput);
            passwordField.Clear();
            passwordField.SendKeys(password);
        }

        /// <summary>
        /// Click login button
        /// </summary>
        public void ClickLogin()
        {
            TestHelper.WaitAndClick(_driver, _loginButton);
        }

        /// <summary>
        /// Complete login with email and password
        /// </summary>
        public void Login(string email, string password)
        {
            EnterLoginEmail(email);
            EnterLoginPassword(password);
            ClickLogin();
        }

        /// <summary>
        /// Check if login error message is displayed
        /// </summary>
        public bool IsLoginErrorMessageDisplayed()
        {
            try
            {
                var errorElement = _driver.FindElement(_loginErrorMessage);
                return errorElement.Displayed;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get login error message text
        /// </summary>
        public string GetLoginErrorMessage()
        {
            try
            {
                var errorElement = _driver.FindElement(_loginErrorMessage);
                return errorElement.Text;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Enter signup name
        /// </summary>
        public void EnterSignupName(string name)
        {
            var nameField = TestHelper.WaitForElement(_driver, _signupNameInput);
            nameField.Clear();
            nameField.SendKeys(name);
        }

        /// <summary>
        /// Enter signup email
        /// </summary>
        public void EnterSignupEmail(string email)
        {
            var emailField = TestHelper.WaitForElement(_driver, _signupEmailInput);
            emailField.Clear();
            emailField.SendKeys(email);
        }

        /// <summary>
        /// Click signup button
        /// </summary>
        public void ClickSignup()
        {
            TestHelper.WaitAndClick(_driver, _signupButton);
        }

        /// <summary>
        /// Complete signup form with name and email
        /// </summary>
        public void InitiateSignup(string name, string email)
        {
            EnterSignupName(name);
            EnterSignupEmail(email);
            ClickSignup();
        }
    }
}