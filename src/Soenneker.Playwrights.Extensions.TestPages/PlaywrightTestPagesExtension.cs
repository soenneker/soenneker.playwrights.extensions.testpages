using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Soenneker.Extensions.String;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;

namespace Soenneker.Playwrights.Extensions.TestPages;

/// <summary>
/// Provides navigation and locator helpers for Playwright page tests.
/// </summary>
public static class PlaywrightTestPagesExtension
{
    /// <summary>
    /// Navigates to a URL, waits for the ready locator to be visible, and optionally verifies the page title.
    /// </summary>
    /// <param name="page">Browser page to inspect or control.</param>
    /// <param name="url">URL of the resource to target.</param>
    /// <param name="readyLocatorFactory">Creates the locator that signals the page is ready.</param>
    /// <param name="expectedTitle">Optional exact page title to verify.</param>
    /// <returns>A task that completes after the page is ready and the optional title assertion succeeds.</returns>
    public static async ValueTask GotoAndWaitForReady(this IPage page, string url, Func<IPage, ILocator> readyLocatorFactory, string? expectedTitle = null)
    {
        await page.GotoAsync(url, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        }).NoSync();

        await Assertions.Expect(readyLocatorFactory(page))
                        .ToBeVisibleAsync().NoSync();

        if (expectedTitle.HasContent())
            await Assertions.Expect(page)
                            .ToHaveTitleAsync(expectedTitle).NoSync();
    }

    /// <summary>
    /// Joins a base URL and application route without duplicating the separating slash.
    /// </summary>
    /// <param name="baseUrl">URL of the base to target.</param>
    /// <param name="route">Application route to append.</param>
    /// <returns>The combined URL.</returns>
    public static string GetRouteUrl(this string baseUrl, string route)
    {
        if (route == "/")
            return baseUrl.TrimEnd('/');

        return $"{baseUrl.TrimEnd('/')}/{route.TrimStart('/')}";
    }

    /// <summary>
    /// Opens a route, waits for a page-specific locator to become visible, and invokes an assertion.
    /// </summary>
    /// <param name="page">Browser page to inspect or control.</param>
    /// <param name="baseUrl">URL of the base to target.</param>
    /// <param name="route">Route for the open page operation.</param>
    /// <param name="readyLocatorFactory">Creates the locator that signals the page is ready.</param>
    /// <param name="assertion">Assertion to run after the ready locator becomes visible.</param>
    /// <returns>A task that completes after the assertion finishes.</returns>
    public static async ValueTask OpenPage(this IPage page, string baseUrl, string route,
        Func<IPage, ILocator> readyLocatorFactory, Func<IPage, ValueTask> assertion)
    {
        await page.GotoAsync(baseUrl.GetRouteUrl(route), new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        }).NoSync();

        await Assertions.Expect(readyLocatorFactory(page))
                        .ToBeVisibleAsync()
                        .NoSync();

        await assertion(page).NoSync();
    }

    /// <summary>
    /// Opens a route and invokes an assertion after the DOM content has loaded.
    /// </summary>
    /// <param name="page">Browser page to inspect or control.</param>
    /// <param name="baseUrl">URL of the base to target.</param>
    /// <param name="route">Route for the open page operation.</param>
    /// <param name="assertion">Assertion to run after navigation.</param>
    /// <returns>A task that completes after the assertion finishes.</returns>
    public static async ValueTask OpenPage(this IPage page, string baseUrl, string route, Func<IPage, Task> assertion)
    {
        await page.GotoAsync(baseUrl.GetRouteUrl(route), new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        }).NoSync();

        await assertion(page).NoSync();
    }

    /// <summary>
    /// Locates the last visible ARIA menu on the page.
    /// </summary>
    /// <param name="page">Browser page to inspect or control.</param>
    /// <returns>A locator for the last visible element with <c>role="menu"</c>.</returns>
    public static ILocator VisibleMenu(this IPage page)
    {
        return page.Locator("[role='menu']:visible")
                   .Last;
    }
}
