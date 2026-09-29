# Graph Report - PlaywrightAutomation  (2026-09-28)

## Corpus Check
- 28 files · ~4,735 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 4 file(s) not represented in the graph (top: (none) 2, .runsettings 2)

## Summary
- 308 nodes · 515 edges · 14 communities
- Extraction: 98% EXTRACTED · 2% INFERRED · 0% AMBIGUOUS · INFERRED: 11 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Community Hubs (Navigation)
- BaseTest
- PlaywrightAutomation.Tests.Models
- ProductsPage
- TestUser
- TestSettings
- RegisterPage
- LoginPage
- CartPage
- PaymentPage
- CheckoutPage
- ProductDetailsPage
- PlaywrightAutomation.Tests
- .OpenCheckoutWithProductsAsync
- HeaderComponent

## God Nodes (most connected - your core abstractions)
1. `BaseTest` - 32 edges
2. `RegisterPage` - 28 edges
3. `TestSettings` - 21 edges
4. `TestUser` - 21 edges
5. `ProductsPage` - 18 edges
6. `CheckoutPage` - 14 edges
7. `PlaywrightAutomation.Tests.Models` - 13 edges
8. `PaymentPage` - 13 edges
9. `ProductDetailsPage` - 13 edges
10. `LoginPage` - 12 edges

## Surprising Connections (you probably didn't know these)
- `Decisiones de diseño` --references--> `BaseTest`  [INFERRED]
  README.md → PlaywrightAutomation.Tests/Fixtures/BaseTest.cs
- `Evidencias` --references--> `BaseTest`  [INFERRED]
  README.md → PlaywrightAutomation.Tests/Fixtures/BaseTest.cs
- `BaseTest` --references--> `TestSettings`  [EXTRACTED]
  PlaywrightAutomation.Tests/Fixtures/BaseTest.cs → PlaywrightAutomation.Tests/Configuration/TestSettings.cs
- `AuthenticatedTest` --inherits--> `BaseTest`  [EXTRACTED]
  PlaywrightAutomation.Tests/Fixtures/AuthenticatedTest.cs → PlaywrightAutomation.Tests/Fixtures/BaseTest.cs
- `AuthenticatedTest` --references--> `TestUser`  [EXTRACTED]
  PlaywrightAutomation.Tests/Fixtures/AuthenticatedTest.cs → PlaywrightAutomation.Tests/Models/TestUser.cs

## Import Cycles
- None detected.

## Communities (14 total, 0 thin omitted)

### Community 0 - "BaseTest"
Cohesion: 0.06
Nodes (30): BrowserNewContextOptions, IAPIRequestContext, IAPIResponse, IAsyncDisposable, IPlaywright, List, PageTest, SetUp (+22 more)

### Community 1 - "PlaywrightAutomation.Tests.Models"
Cohesion: 0.09
Nodes (19): PlaywrightAutomation.Tests.Tests.Cart, PlaywrightAutomation.Tests.Utilities, PlaywrightAutomation.Tests.Tests.Products, PlaywrightAutomation.Tests.Tests.Authentication, PlaywrightAutomation.Tests.TestData, PlaywrightAutomation.Tests.Tests.Checkout, PlaywrightAutomation.Tests.Fixtures, PlaywrightAutomation.Tests.Pages.Components (+11 more)

### Community 2 - "ProductsPage"
Cohesion: 0.12
Nodes (20): ILocator, IPage, Task, ProductsPage, AddedToCartDialog, AllProductsTitle, ContinueShoppingButton, ProductCards (+12 more)

### Community 3 - "TestUser"
Cohesion: 0.11
Nodes (19): TestUser, Address, City, Company, Country, Email, FirstName, LastName (+11 more)

### Community 4 - "TestSettings"
Cohesion: 0.11
Nodes (19): PlaywrightAutomation.Tests.Configuration, InvalidOperationException, Lazy, ArtifactMode, Always, Off, OnFailure, TestSettings (+11 more)

### Community 5 - "RegisterPage"
Cohesion: 0.09
Nodes (23): ILocator, IPage, RegisterPage, AccountCreatedTitle, AccountDeletedTitle, AccountInformationTitle, AddressInput, CityInput (+15 more)

### Community 6 - "LoginPage"
Cohesion: 0.16
Nodes (15): SetUp, Task, ILocator, IPage, Task, LoginPage, EmailInput, ErrorMessage (+7 more)

### Community 7 - "CartPage"
Cohesion: 0.18
Nodes (11): ILocator, IPage, Task, CartPage, EmptyCartMessage, Items, ProceedToCheckoutButton, ILocator (+3 more)

### Community 8 - "PaymentPage"
Cohesion: 0.12
Nodes (14): PaymentCard, ILocator, IPage, Task, PaymentPage, CardNumberInput, CvcInput, ExpiryMonthInput (+6 more)

### Community 9 - "CheckoutPage"
Cohesion: 0.15
Nodes (12): ILocator, IPage, Task, CheckoutPage, AddressDetailsTitle, BillingAddress, CommentInput, DeliveryAddress (+4 more)

### Community 10 - "ProductDetailsPage"
Cohesion: 0.18
Nodes (11): ILocator, IPage, ProductDetailsPage, Availability, Brand, Category, Condition, Name (+3 more)

### Community 11 - "PlaywrightAutomation.Tests"
Cohesion: 0.20
Nodes (9): net8.0, coverlet.collector (6.0.0), Microsoft.NET.Test.Sdk (18.10.1), Microsoft.Playwright.NUnit (1.63.0), NUnit (5.0.0), NUnit3TestAdapter (6.3.0), NUnit.Analyzers (4.15.0), PlaywrightAutomation.Tests (+1 more)

### Community 12 - ".OpenCheckoutWithProductsAsync"
Cohesion: 0.42
Nodes (5): AuthenticatedTest, Category, Task, Test, CheckoutTests

### Community 13 - "HeaderComponent"
Cohesion: 0.22
Nodes (8): ILocator, IPage, Task, HeaderComponent, DeleteAccountLink, LoggedInUser, LogoutLink, SignupLoginLink

## Knowledge Gaps
- **114 isolated node(s):** `Off`, `OnFailure`, `Always`, `Current`, `BaseUrl` (+109 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 152 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `BaseTest` connect `BaseTest` to `PlaywrightAutomation.Tests.Models`, `ProductsPage`, `TestUser`, `TestSettings`, `RegisterPage`, `LoginPage`, `CartPage`, `PaymentPage`, `CheckoutPage`, `ProductDetailsPage`, `.OpenCheckoutWithProductsAsync`, `HeaderComponent`?**
  _High betweenness centrality (0.694) - this node is a cross-community bridge._
- **Why does `RegisterPage` connect `RegisterPage` to `BaseTest`, `PlaywrightAutomation.Tests.Models`, `TestUser`?**
  _High betweenness centrality (0.157) - this node is a cross-community bridge._
- **Why does `TestSettings` connect `TestSettings` to `BaseTest`?**
  _High betweenness centrality (0.134) - this node is a cross-community bridge._
- **Are the 2 inferred relationships involving `BaseTest` (e.g. with `Decisiones de diseño` and `Evidencias`) actually correct?**
  _`BaseTest` has 2 INFERRED edges - model-reasoned connections that need verification._
- **What connects `Off`, `OnFailure`, `Always` to the rest of the system?**
  _114 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `BaseTest` be split into smaller, more focused modules?**
  _Cohesion score 0.06387921022067364 - nodes in this community are weakly interconnected._
- **Should `PlaywrightAutomation.Tests.Models` be split into smaller, more focused modules?**
  _Cohesion score 0.08780487804878048 - nodes in this community are weakly interconnected._