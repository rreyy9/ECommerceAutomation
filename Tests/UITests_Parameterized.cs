using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECommerceAutomation.Pages;

namespace ECommerceAutomation.Tests
{
    /// <summary>
    /// Parameterized UI Test Cases - demonstrates data-driven testing
    /// </summary>
    [TestClass]
    public class UITests_Parameterized : BaseTest
    {
        /// <summary>
        /// Test Case: Search Product with Multiple Search Terms
        /// Demonstrates parameterization with different product names
        /// </summary>
        [TestMethod]
        [TestCategory("UI")]
        [TestCategory("Products")]
        [TestCategory("Parameterized")]
        [DataRow("Dress", DisplayName = "Search for Dress")]
        [DataRow("Jeans", DisplayName = "Search for Jeans")]
        [DataRow("Shirt", DisplayName = "Search for Shirt")]
        public void Test_SearchProduct_Parameterized(string productName)
        {
            Log($"Starting Parameterized Test: Search Product - '{productName}'");

            try
            {
                // Step 1: Navigate to homepage
                Log("Step 1: Navigating to homepage");
                NavigateTo(BaseUrl);

                // Step 2: Navigate to "Products" page
                Log("Step 2: Clicking Products link");
                var homePage = new HomePage(Driver);
                homePage.ClickProducts();

                // Step 3: Search for product
                Log($"Step 3: Searching for product: {productName}");
                var productsPage = new ProductsPage(Driver);
                productsPage.SearchProduct(productName);

                // Step 4: Verify "SEARCHED PRODUCTS" is visible
                Log("Step 4: Verifying 'SEARCHED PRODUCTS' title is visible");
                Assert.IsTrue(productsPage.IsSearchedProductsTitleVisible(),
                    "Expected 'SEARCHED PRODUCTS' title to be visible");

                // Step 5: Assert that products are displayed
                Log("Step 5: Verifying products are displayed");
                Assert.IsTrue(productsPage.AreProductsDisplayed(),
                    $"Expected at least one product to be displayed for search term '{productName}'");

                int productCount = productsPage.GetProductCount();
                Log($"Found {productCount} products matching '{productName}'");

                Log($"Test PASSED: Product search for '{productName}' completed successfully");
            }
            catch (Exception ex)
            {
                Log($"Test FAILED for '{productName}': {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Test Case: Login with Multiple Credential Sets
        /// Demonstrates parameterization for login testing with different users
        /// </summary>
        [TestMethod]
        [TestCategory("UI")]
        [TestCategory("Login")]
        [TestCategory("Parameterized")]
        [DataRow("kresuser@example.com", "Test@123", "Kres User", DisplayName = "Login as Kres User")]
        // Add more users here as needed:
        // [DataRow("user2@example.com", "Pass@456", "User Two", DisplayName = "Login as User Two")]
        public void Test_Login_Parameterized(string email, string password, string expectedUsername)
        {
            Log($"Starting Parameterized Test: Login - '{email}'");

            try
            {
                // Step 1: Navigate to homepage
                Log("Step 1: Navigating to homepage");
                NavigateTo(BaseUrl);

                // Step 2: Click "Signup / Login"
                Log("Step 2: Clicking Signup/Login link");
                var homePage = new HomePage(Driver);
                homePage.ClickSignupLogin();

                // Step 3: Enter credentials and log in
                Log($"Step 3: Entering credentials - Email: {email}");
                var loginPage = new LoginPage(Driver);
                loginPage.Login(email, password);

                // Step 3.5: Check for login error message
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

                Log($"Test PASSED: User '{expectedUsername}' successfully logged in");
            }
            catch (Exception ex)
            {
                Log($"Test FAILED for '{email}': {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Test Case: Cross-Browser Parameterized Search
        /// Demonstrates both browser and data parameterization
        /// </summary>
        [TestMethod]
        [TestCategory("UI")]
        [TestCategory("Products")]
        [TestCategory("Parameterized")]
        [TestCategory("CrossBrowser")]
        [DataRow("Chrome", "Dress", DisplayName = "Chrome - Search Dress")]
        [DataRow("Chrome", "Jeans", DisplayName = "Chrome - Search Jeans")]
        [DataRow("Edge", "Dress", DisplayName = "Edge - Search Dress")]
        [DataRow("Edge", "Jeans", DisplayName = "Edge - Search Jeans")]
        public void Test_CrossBrowser_SearchProduct(string browserName, string productName)
        {
            Log($"Starting Cross-Browser Parameterized Test: {browserName} - Search '{productName}'");

            try
            {
                // Override browser based on parameter
                Driver?.Quit();
                var browserType = Enum.Parse<Helpers.WebDriverFactory.BrowserType>(browserName);
                Driver = Helpers.WebDriverFactory.CreateDriver(browserType);
                Log($"Browser switched to: {browserName}");

                // Step 1: Navigate to homepage
                Log("Step 1: Navigating to homepage");
                NavigateTo(BaseUrl);

                // Step 2: Navigate to Products page
                Log("Step 2: Clicking Products link");
                var homePage = new HomePage(Driver);
                homePage.ClickProducts();

                // Step 3: Search for product
                Log($"Step 3: Searching for product: {productName}");
                var productsPage = new ProductsPage(Driver);
                productsPage.SearchProduct(productName);

                // Step 4: Verify results
                Log("Step 4: Verifying 'SEARCHED PRODUCTS' title is visible");
                Assert.IsTrue(productsPage.IsSearchedProductsTitleVisible(),
                    "Expected 'SEARCHED PRODUCTS' title to be visible");

                Assert.IsTrue(productsPage.AreProductsDisplayed(),
                    $"Expected products for '{productName}' on {browserName}");

                int productCount = productsPage.GetProductCount();
                Log($"Found {productCount} products on {browserName} for '{productName}'");

                Log($"Test PASSED: {browserName} - Search '{productName}' completed successfully");
            }
            catch (Exception ex)
            {
                Log($"Test FAILED for {browserName} - '{productName}': {ex.Message}");
                throw;
            }
        }
    }
}