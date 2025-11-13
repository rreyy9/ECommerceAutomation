using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ECommerceAutomation.Helpers;

namespace ECommerceAutomation.Pages
{
    /// <summary>
    /// Page Object Model for Signup/Registration Page
    /// </summary>
    public class SignupPage
    {
        private readonly IWebDriver _driver;

        // Account Information Locators
        private readonly By _titleMr = By.Id("id_gender1");
        private readonly By _titleMrs = By.Id("id_gender2");
        private readonly By _passwordInput = By.Id("password");
        private readonly By _dayDropdown = By.Id("days");
        private readonly By _monthDropdown = By.Id("months");
        private readonly By _yearDropdown = By.Id("years");
        private readonly By _newsletterCheckbox = By.Id("newsletter");
        private readonly By _offersCheckbox = By.Id("optin");

        // Address Information Locators
        private readonly By _firstNameInput = By.Id("first_name");
        private readonly By _lastNameInput = By.Id("last_name");
        private readonly By _companyInput = By.Id("company");
        private readonly By _address1Input = By.Id("address1");
        private readonly By _address2Input = By.Id("address2");
        private readonly By _countryDropdown = By.Id("country");
        private readonly By _stateInput = By.Id("state");
        private readonly By _cityInput = By.Id("city");
        private readonly By _zipcodeInput = By.Id("zipcode");
        private readonly By _mobileNumberInput = By.Id("mobile_number");

        // Button Locators
        private readonly By _createAccountButton = By.XPath("//button[@data-qa='create-account']");

        // Success Message Locators
        private readonly By _accountCreatedMessage = By.XPath("//h2[@data-qa='account-created']");
        private readonly By _continueButton = By.XPath("//a[@data-qa='continue-button']");
        private readonly By _accountDeletedMessage = By.XPath("//h2[@data-qa='account-deleted']");

        public SignupPage(IWebDriver driver)
        {
            _driver = driver;
        }

        /// <summary>
        /// Select title (Mr/Mrs)
        /// </summary>
        public void SelectTitle(string title)
        {
            if (title.Equals("Mr", StringComparison.OrdinalIgnoreCase))
            {
                TestHelper.WaitAndClick(_driver, _titleMr);
            }
            else
            {
                TestHelper.WaitAndClick(_driver, _titleMrs);
            }
        }

        /// <summary>
        /// Fill complete registration form
        /// </summary>
        public void FillRegistrationForm(string title, string password, string day, string month, string year,
            string firstName, string lastName, string company, string address1, string address2,
            string country, string state, string city, string zipcode, string mobileNumber)
        {
            // Account Information
            SelectTitle(title);
            _driver.FindElement(_passwordInput).SendKeys(password);

            // Date of Birth
            var daySelect = new SelectElement(_driver.FindElement(_dayDropdown));
            daySelect.SelectByValue(day);

            var monthSelect = new SelectElement(_driver.FindElement(_monthDropdown));
            monthSelect.SelectByValue(month);

            var yearSelect = new SelectElement(_driver.FindElement(_yearDropdown));
            yearSelect.SelectByValue(year);

            // Address Information
            _driver.FindElement(_firstNameInput).SendKeys(firstName);
            _driver.FindElement(_lastNameInput).SendKeys(lastName);
            _driver.FindElement(_companyInput).SendKeys(company);
            _driver.FindElement(_address1Input).SendKeys(address1);
            _driver.FindElement(_address2Input).SendKeys(address2);

            var countrySelect = new SelectElement(_driver.FindElement(_countryDropdown));
            countrySelect.SelectByValue(country);

            _driver.FindElement(_stateInput).SendKeys(state);
            _driver.FindElement(_cityInput).SendKeys(city);
            _driver.FindElement(_zipcodeInput).SendKeys(zipcode);
            _driver.FindElement(_mobileNumberInput).SendKeys(mobileNumber);
        }

        /// <summary>
        /// Click Create Account button
        /// </summary>
        public void ClickCreateAccount()
        {
            TestHelper.WaitAndClick(_driver, _createAccountButton);
        }

        /// <summary>
        /// Verify account created message is visible
        /// </summary>
        public bool IsAccountCreatedMessageVisible()
        {
            try
            {
                var message = TestHelper.WaitForElement(_driver, _accountCreatedMessage);
                return message.Displayed;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Click Continue button after account creation
        /// </summary>
        public void ClickContinue()
        {
            TestHelper.WaitAndClick(_driver, _continueButton);
        }

        /// <summary>
        /// Verify account deleted message is visible
        /// </summary>
        public bool IsAccountDeletedMessageVisible()
        {
            try
            {
                var message = TestHelper.WaitForElement(_driver, _accountDeletedMessage);
                return message.Displayed;
            }
            catch
            {
                return false;
            }
        }
    }
}