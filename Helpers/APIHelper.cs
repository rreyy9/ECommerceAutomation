using RestSharp;
using System.Net;

namespace ECommerceAutomation.Helpers
{
    /// <summary>
    /// Helper class for API testing operations
    /// </summary>
    public class APIHelper
    {
        private readonly RestClient _client;
        private const string BaseApiUrl = "https://automationexercise.com/api";

        public APIHelper()
        {
            _client = new RestClient(BaseApiUrl);
        }

        /// <summary>
        /// Execute a GET request
        /// </summary>
        public RestResponse ExecuteGet(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.Get);
            var response = _client.Execute(request);
            return response;
        }

        /// <summary>
        /// Execute a POST request with form data
        /// </summary>
        public RestResponse ExecutePost(string endpoint, Dictionary<string, string> parameters)
        {
            var request = new RestRequest(endpoint, Method.Post);

            // Add parameters as form data
            foreach (var param in parameters)
            {
                request.AddParameter(param.Key, param.Value);
            }

            var response = _client.Execute(request);
            return response;
        }

        /// <summary>
        /// Verify response status code
        /// </summary>
        public bool VerifyStatusCode(RestResponse response, HttpStatusCode expectedStatusCode)
        {
            return response.StatusCode == expectedStatusCode;
        }

        /// <summary>
        /// Verify response contains expected text
        /// </summary>
        public bool VerifyResponseContains(RestResponse response, string expectedText)
        {
            return response.Content?.Contains(expectedText) ?? false;
        }

        /// <summary>
        /// Check if response is successful (2xx status code)
        /// </summary>
        public bool IsSuccessful(RestResponse response)
        {
            return response.IsSuccessful;
        }

        /// <summary>
        /// Get response content as string
        /// </summary>
        public string GetResponseContent(RestResponse response)
        {
            return response.Content ?? string.Empty;
        }
    }
}