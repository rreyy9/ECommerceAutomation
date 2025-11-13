using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECommerceAutomation.Pages;

namespace ECommerceAutomation.Tests
{
    /// <summary>
    /// UI Test Cases using Selenium WebDriver
    /// </summary>
    [TestClass]
    public class UITests : BaseTest
    {
        /// <summary>
        /// Test Case 1: Login User with Correct Email and Password
        /// </summary>
        [TestMethod]
        [TestCategory("UI")]
        [TestCategory("Login")]
        public void Test01_LoginWithValidCredentials()
        {
            Log("Starting Test: Login User with Correct Email and Password");

            // Test Data
            string email = "testuser@example.com";
            string password = "Test@123";
            string expectedUsername = "Test User";

            try
            {
                // Step 1: Launch browser and navigate to homepage
                Log("Step 1: Navigating to homepage");
                NavigateTo(BaseUrl);

                // Step 2: Click "Signup / Login"
                Log("Step 2: Clicking Signup/Login link");
                var homePage = new HomePage(Driver);
                homePage.ClickSignupLogin();

                // Step 3: Enter valid credentials and log in
                Log($"Step 3: Entering credentials - Email: {email}");
                var loginPage = new LoginPage(Driver);
                loginPage.Login(email, password);

                // Step 3.5: Check for login error message (edge case handling)
                Log("Step 3.5: Checking for login errors");
                if (loginPage.IsLoginErrorMessageDisplayed())
                {
                    string errorMessage = loginPage.GetLoginErrorMessage();
                    Log($"Login error detected: {errorMessage}");
                    Assert.Fail($"Login failed with error: {errorMessage}. Please verify credentials are correct.");
                }

                // Step 4: Verify "Logged in as username" is visible
                Log("Step 4: Verifying user is logged in");
                Assert.IsTrue(homePage.IsUserLoggedIn(expectedUsername),
                    $"Expected 'Logged in as {expectedUsername}' to be visible");

                Log("Test PASSED: User successfully logged in");
            }
            catch (Exception ex)
            {
                Log($"Test FAILED: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Test Case 2: Search Product
        /// </summary>
        [TestMethod]
        [TestCategory("UI")]
        [TestCategory("Products")]
        public void Test02_SearchProduct()
        {
            Log("Starting Test: Search Product");

            // Test Data
            string productName = "Dress";

            try
            {
                // Step 1: Navigate to homepage
                Log("Step 1: Navigating to homepage");
                NavigateTo(BaseUrl);

                // Step 2: Navigate to "Products" page
                Log("Step 2: Clicking Products link");
                var homePage = new HomePage(Driver);
                homePage.ClickProducts();

                // Step 3: Enter product name in search input and click search
                Log($"Step 3: Searching for product: {productName}");
                var productsPage = new ProductsPage(Driver);
                productsPage.SearchProduct(productName);

                // Step 4: Verify "SEARCHED PRODUCTS" is visible
                Log("Step 4: Verifying 'SEARCHED PRODUCTS' title is visible");
                Assert.IsTrue(productsPage.IsSearchedProductsTitleVisible(),
                    "Expected 'SEARCHED PRODUCTS' title to be visible");

                // Step 5: Assert that relevant products are displayed
                Log("Step 5: Verifying products are displayed");
                Assert.IsTrue(productsPage.AreProductsDisplayed(),
                    "Expected at least one product to be displayed in search results");

                int productCount = productsPage.GetProductCount();
                Log($"Found {productCount} products matching '{productName}'");

                Log("Test PASSED: Product search completed successfully");
            }
            catch (Exception ex)
            {
                Log($"Test FAILED: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Test Case 4 (BONUS): Register New User
        /// </summary>
        [TestMethod]
        [TestCategory("UI")]
        [TestCategory("Registration")]
        public void Test04_RegisterNewUser()
        {
            Log("Starting Test: Register New User");

            // Test Data - Generate unique email with timestamp
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string name = "AutoTest User";
            string email = $"autotest_{timestamp}@example.com";
            string password = "Test@123";

            try
            {
                // Step 1: Navigate to homepage
                Log("Step 1: Navigating to homepage");
                NavigateTo(BaseUrl);

                // Step 2: Navigate to "Signup / Login"
                Log("Step 2: Clicking Signup/Login link");
                var homePage = new HomePage(Driver);
                homePage.ClickSignupLogin();

                // Step 3: Initiate signup with name and email
                Log($"Step 3: Entering signup details - Name: {name}, Email: {email}");
                var loginPage = new LoginPage(Driver);
                loginPage.InitiateSignup(name, email);

                // Step 4: Complete registration form with all required fields
                Log("Step 4: Filling registration form");
                var signupPage = new SignupPage(Driver);
                signupPage.FillRegistrationForm(
                    title: "Mr",
                    password: password,
                    day: "15",
                    month: "6",
                    year: "1990",
                    firstName: "Auto",
                    lastName: "Test",
                    company: "Test Company",
                    address1: "123 Test Street",
                    address2: "Suite 100",
                    country: "Australia",
                    state: "Victoria",
                    city: "Melbourne",
                    zipcode: "3000",
                    mobileNumber: "0412345678"
                );

                // Step 5: Click Create Account
                Log("Step 5: Clicking Create Account button");
                signupPage.ClickCreateAccount();

                // Step 6: Verify "ACCOUNT CREATED!" message
                Log("Step 6: Verifying 'ACCOUNT CREATED!' message");
                Assert.IsTrue(signupPage.IsAccountCreatedMessageVisible(),
                    "Expected 'ACCOUNT CREATED!' message to be visible");

                // Step 7: Click Continue
                Log("Step 7: Clicking Continue button");
                signupPage.ClickContinue();

                // Step 8: Verify "Logged in as username"
                Log("Step 8: Verifying user is logged in");
                Assert.IsTrue(homePage.IsUserLoggedIn(name),
                    $"Expected 'Logged in as {name}' to be visible");

                // Step 9: Delete account
                Log("Step 9: Deleting account");
                homePage.ClickDeleteAccount();

                // Step 10: Confirm deletion
                Log("Step 10: Verifying account deletion");
                Assert.IsTrue(signupPage.IsAccountDeletedMessageVisible(),
                    "Expected 'ACCOUNT DELETED!' message to be visible");

                Log("Test PASSED: User registration and deletion completed successfully");
            }
            catch (Exception ex)
            {
                Log($"Test FAILED: {ex.Message}");
                throw;
            }
        }
    }
}