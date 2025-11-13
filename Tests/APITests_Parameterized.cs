using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECommerceAutomation.Helpers;
using System.Net;
using Newtonsoft.Json.Linq;

namespace ECommerceAutomation.Tests
{
    /// <summary>
    /// Parameterized API Test Cases - demonstrates data-driven API testing
    /// </summary>
    [TestClass]
    public class APITests_Parameterized
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
        /// Test Case: Verify Multiple API Endpoints
        /// Demonstrates parameterized API endpoint testing
        /// </summary>
        [TestMethod]
        [TestCategory("API")]
        [TestCategory("Parameterized")]
        [DataRow("/productsList", HttpStatusCode.OK, DisplayName = "GET Products List")]
        [DataRow("/brandsList", HttpStatusCode.OK, DisplayName = "GET Brands List")]
        public void Test_GetEndpoints_Parameterized(string endpoint, HttpStatusCode expectedStatusCode)
        {
            Log($"Starting Parameterized API Test: GET {endpoint}");

            try
            {
                // Step 1: Make GET request
                Log($"Step 1: Sending GET request to {endpoint}");
                var response = _apiHelper.ExecuteGet(endpoint);

                Log($"Response Status Code: {response.StatusCode}");
                Log($"Response Content Length: {response.Content?.Length ?? 0} characters");

                // Step 2: Assert response code
                Log($"Step 2: Verifying status code is {expectedStatusCode}");
                Assert.AreEqual(expectedStatusCode, response.StatusCode,
                    $"Expected status code {expectedStatusCode}, but got {response.StatusCode}");

                // Step 3: Validate response has content
                Log("Step 3: Validating response has content");
                Assert.IsNotNull(response.Content, "Response content should not be null");
                Assert.IsTrue(response.Content.Length > 0, "Response content should not be empty");

                // Step 4: Validate JSON structure
                Log("Step 4: Validating JSON structure");
                var jsonResponse = JObject.Parse(response.Content);
                Assert.IsNotNull(jsonResponse, "Response should be valid JSON");

                Log($"Test PASSED: GET {endpoint} completed successfully");
            }
            catch (Exception ex)
            {
                Log($"Test FAILED for {endpoint}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Test Case: Create Multiple Users with Different Data
        /// Demonstrates parameterized user creation via API
        /// </summary>
        [TestMethod]
        [TestCategory("API")]
        [TestCategory("Registration")]
        [TestCategory("Parameterized")]
        [DataRow("John", "Doe", "Australia", DisplayName = "Create User - John from Australia")]
        [DataRow("Jane", "Smith", "United States", DisplayName = "Create User - Jane from USA")]
        [DataRow("Bob", "Jones", "India", DisplayName = "Create User - Bob from India")]
        public void Test_CreateUser_Parameterized(string firstName, string lastName, string country)
        {
            Log($"Starting Parameterized API Test: Create User - {firstName} {lastName} from {country}");

            // Generate unique email with timestamp
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string email = $"{firstName.ToLower()}.{lastName.ToLower()}_{timestamp}@example.com";

            var userParameters = new Dictionary<string, string>
            {
                { "name", $"{firstName} {lastName}" },
                { "email", email },
                { "password", "Test@123" },
                { "title", "Mr" },
                { "birth_date", "15" },
                { "birth_month", "6" },
                { "birth_year", "1990" },
                { "firstname", firstName },
                { "lastname", lastName },
                { "company", "Test Company" },
                { "address1", "123 Test Street" },
                { "address2", "Suite 100" },
                { "country", country },
                { "zipcode", "12345" },
                { "state", "TestState" },
                { "city", "TestCity" },
                { "mobile_number", "1234567890" }
            };

            try
            {
                // Step 1: Make POST request
                Log($"Step 1: Sending POST request to create user: {email}");
                var response = _apiHelper.ExecutePost("/createAccount", userParameters);

                Log($"Response Status Code: {response.StatusCode}");
                Log($"Response Content: {response.Content}");

                // Step 2: Assert HTTP status code
                Log("Step 2: Verifying HTTP status code");
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode,
                    $"Expected status code 200, but got {response.StatusCode}");

                // Step 3: Parse and verify response
                Log("Step 3: Verifying response structure");
                var jsonResponse = JObject.Parse(response.Content);

                Assert.IsTrue(jsonResponse.ContainsKey("responseCode"),
                    "Response should contain 'responseCode' key");
                Assert.IsTrue(jsonResponse.ContainsKey("message"),
                    "Response should contain 'message' key");

                int responseCode = jsonResponse["responseCode"].Value<int>();
                string message = jsonResponse["message"].Value<string>();

                Log($"Response Code: {responseCode}");
                Log($"Response Message: {message}");

                // Verify successful creation (201) or already exists (400)
                Assert.IsTrue(responseCode == 201 || responseCode == 400,
                    $"Expected responseCode 201 or 400, but got {responseCode}");

                if (responseCode == 201)
                {
                    Assert.IsTrue(message.Contains("User created!"),
                        $"Expected message to contain 'User created!', but got '{message}'");
                    Log($"Test PASSED: User {firstName} {lastName} created successfully");
                }
                else
                {
                    Log($"Test PASSED: User {firstName} {lastName} already exists (expected behavior)");
                }
            }
            catch (Exception ex)
            {
                Log($"Test FAILED for {firstName} {lastName}: {ex.Message}");
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