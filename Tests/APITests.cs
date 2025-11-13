using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECommerceAutomation.Helpers;
using System.Net;
using Newtonsoft.Json.Linq;

namespace ECommerceAutomation.Tests
{
    /// <summary>
    /// API Test Cases using RestSharp
    /// </summary>
    [TestClass]
    public class APITests
    {
        private APIHelper _apiHelper;
        private TestContext _testContext;

        public TestContext TestContext
        {
            get { return _testContext; }
            set { _testContext = value; }
        }

        [TestInitialize]
        public void Setup()
        {
            Log("Initializing API Helper");
            _apiHelper = new APIHelper();
        }

        /// <summary>
        /// Test Case 3: GET All Products List
        /// </summary>
        [TestMethod]
        [TestCategory("API")]
        [TestCategory("Products")]
        public void Test03_GetAllProductsList()
        {
            Log("Starting Test: GET All Products List");

            try
            {
                // Step 1: Make GET request to products list endpoint
                Log("Step 1: Sending GET request to /api/productsList");
                var response = _apiHelper.ExecuteGet("/productsList");

                Log($"Response Status Code: {response.StatusCode}");
                Log($"Response Content Length: {response.Content?.Length ?? 0} characters");

                // Step 2: Assert response code 200
                Log("Step 2: Verifying status code is 200 OK");
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode,
                    $"Expected status code 200, but got {(int)response.StatusCode}");

                // Step 3: Validate structure of returned product list
                Log("Step 3: Validating response structure");
                Assert.IsNotNull(response.Content, "Response content should not be null");
                Assert.IsTrue(response.Content.Length > 0, "Response content should not be empty");

                // Parse JSON response
                var jsonResponse = JObject.Parse(response.Content);

                // Verify response contains 'products' array
                Assert.IsTrue(jsonResponse.ContainsKey("products"),
                    "Response should contain 'products' key");

                var products = jsonResponse["products"] as JArray;
                Assert.IsNotNull(products, "Products should be an array");
                Assert.IsTrue(products.Count > 0, "Products array should not be empty");

                // Validate first product structure
                Log($"Step 4: Validating product structure (Total products: {products.Count})");
                var firstProduct = products[0] as JObject;

                Assert.IsTrue(firstProduct.ContainsKey("id"), "Product should have 'id' field");
                Assert.IsTrue(firstProduct.ContainsKey("name"), "Product should have 'name' field");
                Assert.IsTrue(firstProduct.ContainsKey("price"), "Product should have 'price' field");
                Assert.IsTrue(firstProduct.ContainsKey("brand"), "Product should have 'brand' field");
                Assert.IsTrue(firstProduct.ContainsKey("category"), "Product should have 'category' field");

                Log($"Sample Product - ID: {firstProduct["id"]}, Name: {firstProduct["name"]}, Price: {firstProduct["price"]}");
                Log($"Test PASSED: Retrieved and validated {products.Count} products");
            }
            catch (Exception ex)
            {
                Log($"Test FAILED: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Test Case 5 (BONUS): POST to Create/Register User
        /// </summary>
        [TestMethod]
        [TestCategory("API")]
        [TestCategory("Registration")]
        public void Test05_CreateUserAccount()
        {
            Log("Starting Test: POST to Create/Register User");

            // Test Data - Generate unique email with timestamp
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string email = $"apitest_{timestamp}@example.com";

            var userParameters = new Dictionary<string, string>
            {
                { "name", "API Kres User" },
                { "email", email },
                { "password", "ApiTest@123" },
                { "title", "Mr" },
                { "birth_date", "15" },
                { "birth_month", "6" },
                { "birth_year", "1990" },
                { "firstname", "API" },
                { "lastname", "Test" },
                { "company", "Test Company" },
                { "address1", "123 API Street" },
                { "address2", "Suite 200" },
                { "country", "Australia" },
                { "zipcode", "3000" },
                { "state", "Victoria" },
                { "city", "Melbourne" },
                { "mobile_number", "0412345678" }
            };

            try
            {
                // Step 1: Make POST request to create account endpoint
                Log("Step 1: Sending POST request to /api/createAccount");
                Log($"User Email: {email}");

                var response = _apiHelper.ExecutePost("/createAccount", userParameters);

                Log($"Response Status Code: {response.StatusCode}");
                Log($"Response Content: {response.Content}");

                // Step 2: Assert response code 200 (Note: API returns 200, not 201)
                Log("Step 2: Verifying status code");
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode,
                    $"Expected status code 200, but got {(int)response.StatusCode}");

                // Step 3: Verify response message
                Log("Step 3: Verifying response message");
                Assert.IsNotNull(response.Content, "Response content should not be null");

                // Parse JSON response
                var jsonResponse = JObject.Parse(response.Content);

                // Verify response code and message
                Assert.IsTrue(jsonResponse.ContainsKey("responseCode"),
                    "Response should contain 'responseCode' key");
                Assert.IsTrue(jsonResponse.ContainsKey("message"),
                    "Response should contain 'message' key");

                int responseCode = jsonResponse["responseCode"].Value<int>();
                string message = jsonResponse["message"].Value<string>();

                Log($"Response Code: {responseCode}");
                Log($"Response Message: {message}");

                // Verify successful user creation (responseCode 201 means created)
                Assert.AreEqual(201, responseCode,
                    $"Expected responseCode 201 (User created), but got {responseCode}");
                Assert.IsTrue(message.Contains("User created!"),
                    $"Expected message to contain 'User created!', but got '{message}'");

                Log("Test PASSED: User account created successfully via API");
            }
            catch (Exception ex)
            {
                Log($"Test FAILED: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Helper method to log messages
        /// </summary>
        private void Log(string message)
        {
            TestContext?.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
        }
    }
}