using OpenQA.Selenium;
using ECommerceAutomation.Helpers;

namespace ECommerceAutomation.Pages
{
    /// <summary>
    /// Page Object Model for Products Page
    /// </summary>
    public class ProductsPage
    {
        private readonly IWebDriver _driver;

        // Locators
        private readonly By _searchInput = By.Id("search_product");
        private readonly By _searchButton = By.Id("submit_search");
        private readonly By _searchedProductsTitle = By.XPath("//h2[contains(text(),'Searched Products')]");
        private readonly By _productItems = By.ClassName("productinfo");

        public ProductsPage(IWebDriver driver)
        {
            _driver = driver;
        }

        /// <summary>
        /// Enter product name in search box
        /// </summary>
        public void EnterProductName(string productName)
        {
            var searchBox = TestHelper.WaitForElement(_driver, _searchInput);
            searchBox.Clear();
            searchBox.SendKeys(productName);
        }

        /// <summary>
        /// Click search button
        /// </summary>
        public void ClickSearch()
        {
            TestHelper.WaitAndClick(_driver, _searchButton);
        }

        /// <summary>
        /// Search for a product
        /// </summary>
        public void SearchProduct(string productName)
        {
            EnterProductName(productName);
            ClickSearch();
        }

        /// <summary>
        /// Verify 'Searched Products' title is visible
        /// </summary>
        public bool IsSearchedProductsTitleVisible()
        {
            try
            {
                var title = TestHelper.WaitForElement(_driver, _searchedProductsTitle);
                return title.Displayed;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get count of displayed products
        /// </summary>
        public int GetProductCount()
        {
            var products = _driver.FindElements(_productItems);
            return products.Count;
        }

        /// <summary>
        /// Verify products are displayed
        /// </summary>
        public bool AreProductsDisplayed()
        {
            return GetProductCount() > 0;
        }
    }
}