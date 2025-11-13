# E-Commerce Test Automation Framework

A test automation framework for [Automation Exercise](https://www.automationexercise.com) built with C#, Selenium WebDriver, and MSTest.

---

## Setup Instructions

### Prerequisites

- **Visual Studio 2022** or later
- **.NET 8.0 SDK** or later
- **Google Chrome** (latest version)
- **Microsoft Edge** (latest version)

### Installation Steps

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd ECommerceAutomation
   ```

2. **Open the solution**
   - Double-click `ECommerceAutomation.sln` to open in Visual Studio

3. **Restore NuGet packages**
   - Visual Studio will automatically restore packages on first load
   - Or manually: Right-click Solution → **Restore NuGet Packages**

4. **Build the solution**
   - Press `Ctrl+Shift+B` or go to **Build → Build Solution**

5. **Ready to run tests!**

---

## How to Run the Tests

### Option 1: Using Visual Studio Test Explorer

1. Open **Test Explorer**: `Test` → `Test Explorer` (or press `Ctrl+E, T`)
2. Click **Run All** to execute all tests
3. View results and detailed logs in the Output tab

### Option 2: Using Command Line

```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run specific test categories
dotnet test --filter "TestCategory=UI"
dotnet test --filter "TestCategory=API"
dotnet test --filter "TestCategory=Chrome"
dotnet test --filter "TestCategory=Edge"
```

### Test Categories Available

- **UI** - All Selenium UI tests
- **API** - All API tests  
- **Chrome** - Chrome-specific tests
- **Edge** - Edge-specific tests
- **Login** - Login tests
- **Products** - Product search tests
- **Registration** - User registration tests

---

## Assumptions and Limitations

### Assumptions

1. **Test Site Availability**: The test site (https://www.automationexercise.com) is accessible and operational
2. **Test User Account**: A pre-existing test account is required for login tests: (Already created assumping website keeps data for a period of time)
   - Email: `kresuser@example.com`
   - Password: `Test@123`
   - Username: `Kres User`
3. **Browser Versions**: Latest stable versions of Chrome and Edge are installed

### Limitations

1. **Test User Credentials**: Login tests require manual account creation if default credentials are invalid
2. **Browser Support**: Limited to Chrome and Edge (Firefox and Safari not supported)
3. **Parallel Execution**: Tests can technically run in parallel, but the framework is not designed for it (BaseTest uses instance variables that aren't thread-safe; would require refactoring to support parallel execution)
4. **Hardcoded Configuration**: Base URLs and test data are hardcoded (not externalized to config files)
5. **Basic Reporting**: Uses standard MSTest output; no HTML reports or custom dashboards
6. **Screenshot Storage**: Failure screenshots saved to `bin/Debug/net8.0/Screenshots/` but not attached to test reports
7. **Single Environment**: No support for multiple environments (Dev/QA/Prod switching)

---

## Mobile Testing with Appium

Given experience with Appium, this framework can be adapted for mobile browser testing with the following setup:

### Prerequisites for Mobile Testing

1. **Appium Server**
   ```bash
   npm install -g appium
   appium
   ```

2. **Platform-Specific Requirements**
   - **Android**: Android Studio, Android SDK, Android emulator or physical device
   - **iOS**: Xcode, iOS Simulator (macOS only), physical device with Developer account

3. **NuGet Package**
   ```bash
   dotnet add package Appium.WebDriver
   ```

### Framework Modifications Needed

**1. Extend WebDriverFactory.cs** to support mobile browsers:
```csharp
public enum BrowserType
{
    Chrome,
    Edge,
    AndroidChrome,
    iOSSafari
}

// Add to CreateDriver method:
case BrowserType.AndroidChrome:
    var androidOptions = new AppiumOptions();
    androidOptions.PlatformName = "Android";
    androidOptions.AutomationName = "UiAutomator2";
    androidOptions.DeviceName = "Android Emulator"; // or device ID
    androidOptions.BrowserName = "Chrome";
    // Optional: androidOptions.App = ""; // Leave empty for browser testing
    driver = new AndroidDriver(new Uri("http://127.0.0.1:4723"), androidOptions);
    break;

case BrowserType.iOSSafari:
    var iosOptions = new AppiumOptions();
    iosOptions.PlatformName = "iOS";
    iosOptions.AutomationName = "XCUITest";
    iosOptions.DeviceName = "iPhone 15 Simulator"; // or physical device
    iosOptions.BrowserName = "Safari";
    driver = new IOSDriver(new Uri("http://127.0.0.1:4723"), iosOptions);
    break;
```

**2. Create mobile-specific test class** (e.g., `UITests_Mobile.cs`):
```csharp
[TestClass]
public class UITests_Mobile : BaseTest
{
    [TestInitialize]
    public override void Setup()
    {
        Driver = WebDriverFactory.CreateDriver(
            WebDriverFactory.BrowserType.AndroidChrome
        );
    }
    
    // Reuse existing test methods or add mobile-specific tests
}
```

**3. Consider mobile-specific adjustments:**
- Increased wait times for slower mobile networks
- Touch gestures vs clicks (though web tests usually work as-is)
- Viewport differences for responsive design
- Potential locator adjustments if site has mobile-specific elements

The existing Page Object Model and test logic can be reused with minimal changes once the WebDriver is configured for Appium.

---
