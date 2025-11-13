using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECommerceAutomation.Helpers;
using ECommerceAutomation.Pages;

namespace ECommerceAutomation.Tests
{
    /// <summary>
    /// UI Tests executed on Edge browser
    /// </summary>
    [TestClass]
    public class UITests_Edge : BaseTest
    {
        [TestInitialize]
        public override void Setup()
        {
            Log("Setting up Edge browser for test execution");
            Driver = WebDriverFactory.CreateDriver(WebDriverFactory.BrowserType.Edge);
            Log("Edge browser initialized successfully");
        }

        /// <summary>
        /// Test Case 1: Login User with Correct Email and Password - Edge
        /// </summary>
        [TestMethod]
        [TestCategory("UI")]
        [TestCategory("Login")]
        [TestCategory("Edge")]
        public void Test01_LoginWithValidCredentials_Edge()
        {
            Log("Starting Test: Login User with Correct Email and Password (Edge)");

            string email = "kresuser@example.com";
            string password = "Test@123";
            string expectedUsername = "Kres User";

            try
            {
                Log("Step 1: Navigating to homepage");
                NavigateTo(BaseUrl);

                Log("Step 2: Clicking Signup/Login link");
                var homePage = new HomePage(Driver);
                homePage.ClickSignupLogin();

                Log($"Step 3: Entering credentials - Email: {email}");
                var loginPage = new LoginPage(Driver);
                loginPage.Login(email, password);

                Log("Step 3.5: Checking for login errors");
                if (loginPage.IsLoginErrorMessageDisplayed())
                {
                    string errorMessage = loginPage.GetLoginErrorMessage();
                    Log($"Login error detected: {errorMessage}");
                    Assert.Fail($"Login failed with error: {errorMessage}. Please verify credentials are correct.");
                }

                Log("Step 4: Verifying user is logged in");
                Assert.IsTrue(homePage.IsUserLoggedIn(expectedUsername),
                    $"Expected 'Logged in as {expectedUsername}' to be visible");

                Log("Test PASSED: User successfully logged in on Edge");
            }
            catch (Exception ex)
            {
                Log($"Test FAILED on Edge: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Test Case 2: Search Product - Edge
        /// </summary>
        [TestMethod]
        [TestCategory("UI")]
        [TestCategory("Products")]
        [TestCategory("Edge")]
        public void Test02_SearchProduct_Edge()
        {
            Log("Starting Test: Search Product (Edge)");

            string productName = "Dress";

            try
            {
                Log("Step 1: Navigating to homepage");
                NavigateTo(BaseUrl);

                Log("Step 2: Clicking Products link");
                var homePage = new HomePage(Driver);
                homePage.ClickProducts();

                Log($"Step 3: Searching for product: {productName}");
                var productsPage = new ProductsPage(Driver);
                productsPage.SearchProduct(productName);

                Log("Step 4: Verifying 'SEARCHED PRODUCTS' title is visible");
                Assert.IsTrue(productsPage.IsSearchedProductsTitleVisible(),
                    "Expected 'SEARCHED PRODUCTS' title to be visible");

                Log("Step 5: Verifying products are displayed");
                Assert.IsTrue(productsPage.AreProductsDisplayed(),
                    "Expected at least one product to be displayed in search results");

                int productCount = productsPage.GetProductCount();
                Log($"Found {productCount} products matching '{productName}'");

                Log("Test PASSED: Product search completed successfully on Edge");
            }
            catch (Exception ex)
            {
                Log($"Test FAILED on Edge: {ex.Message}");
                throw;
            }
        }
    }
}