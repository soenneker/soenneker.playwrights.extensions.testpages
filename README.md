[![](https://img.shields.io/nuget/v/soenneker.playwrights.extensions.testpages.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.playwrights.extensions.testpages/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.playwrights.extensions.testpages/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.playwrights.extensions.testpages/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.playwrights.extensions.testpages.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.playwrights.extensions.testpages/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.playwrights.extensions.testpages/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.playwrights.extensions.testpages/actions/workflows/codeql.yml)

# Soenneker.Playwrights.Extensions.TestPages

Small Playwright helpers for navigating pages in browser tests, waiting for a page-specific ready element, and running assertions.

## Installation

```bash
dotnet add package Soenneker.Playwrights.Extensions.TestPages
```

## Usage

```csharp
using Microsoft.Playwright;
using Soenneker.Playwrights.Extensions.TestPages;

await page.GotoAndWaitForReady(
    "https://localhost:5001/account",
    static page => page.GetByRole(AriaRole.Heading, new() { Name = "Account" }),
    expectedTitle: "Account");
```

`OpenPage` joins a base URL and route, waits for `DOMContentLoaded`, and then invokes your assertion. The overload with a ready-locator factory also waits for that locator to become visible.

```csharp
await page.OpenPage(
    "https://localhost:5001",
    "/settings",
    static page => page.GetByTestId("settings-form"),
    async page =>
    {
        await Assertions.Expect(page.GetByLabel("Email notifications"))
                        .ToBeCheckedAsync();
    });
```

Use `GetRouteUrl` when you only need the same base URL/route joining behavior:

```csharp
string url = "https://localhost:5001/".GetRouteUrl("/orders");
// https://localhost:5001/orders
```

`VisibleMenu()` returns the last visible element with `role="menu"`, which is useful when a page keeps hidden menu instances in the DOM.
