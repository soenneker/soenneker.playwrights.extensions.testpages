[![](https://img.shields.io/nuget/v/soenneker.playwrights.extensions.testpages.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.playwrights.extensions.testpages/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.playwrights.extensions.testpages/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.playwrights.extensions.testpages/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.playwrights.extensions.testpages.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.playwrights.extensions.testpages/)

# Soenneker.Playwrights.Extensions.TestPages

Extension methods for Playwright Pages during tests.

## Install

```bash
dotnet add package Soenneker.Playwrights.Extensions.TestPages
```

## Quick start

```csharp
using Soenneker.Playwrights.Extensions.TestPages;

IPage page = /* obtain from your application */;
await page.GotoAndWaitForReady("value", /* supply readyLocatorFactory */ default!);
```

Navigates to and Wait For Ready.

## What you get

- `PlaywrightTestPagesExtension` — Extension methods for Playwright Pages during tests.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `PlaywrightTestPagesExtension.GotoAndWaitForReady(page, url, readyLocatorFactory, expectedTitle)` | Navigates to and Wait For Ready. | A task that completes when the goto and wait for ready operation is complete. |
| `PlaywrightTestPagesExtension.OpenPage(page, baseUrl, route, readyLocatorFactory, assertion)` | Opens page. | A task that completes when the open page operation is complete. |
| `PlaywrightTestPagesExtension.OpenPage(page, baseUrl, route, assertion)` | Opens page. | A task that completes when the open page operation is complete. |
| `PlaywrightTestPagesExtension.VisibleMenu(page)` | Returns the value produced by visible Menu. | The resulting locator. |
