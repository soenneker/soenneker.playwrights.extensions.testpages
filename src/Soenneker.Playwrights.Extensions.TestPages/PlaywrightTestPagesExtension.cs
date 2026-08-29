using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Soenneker.Extensions.String;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;

namespace Soenneker.Playwrights.Extensions.TestPages;

/// <summary>
/// Extension methods for Playwright Pages during tests
/// </summary>
public static class PlaywrightTestPagesExtension
{
    /// <summary>
    /// Navigates to and Wait For Ready.
    /// </summary>
    /// <param name="page">Browser page to inspect or control.</param>
    /// <param name="url">URL of the resource to target.</param>
    /// <param name="readyLocatorFactory">Callback used by goto and wait for ready.</param>
    /// <param name="expectedTitle">Expected Title for the goto and wait for ready operation.</param>
    /// <returns>A task that completes when the goto and wait for ready operation is complete.</returns>
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
    /// Gets route url.
    /// </summary>
    /// <param name="baseUrl">URL of the base to target.</param>
    /// <param name="route">Route for the get route url operation.</param>
    /// <returns>The requested text.</returns>
    public static string GetRouteUrl(this string baseUrl, string route)
    {
        if (route == "/")
            return baseUrl.TrimEnd('/');

        return $"{baseUrl.TrimEnd('/')}/{route.TrimStart('/')}";
    }

    /// <summary>
    /// Opens page.
    /// </summary>
    /// <param name="page">Browser page to inspect or control.</param>
    /// <param name="baseUrl">URL of the base to target.</param>
    /// <param name="route">Route for the open page operation.</param>
    /// <param name="readyLocatorFactory">Callback used by open page.</param>
    /// <param name="assertion">Callback used by open page.</param>
    /// <returns>A task that completes when the open page operation is complete.</returns>
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
    /// Opens page.
    /// </summary>
    /// <param name="page">Browser page to inspect or control.</param>
    /// <param name="baseUrl">URL of the base to target.</param>
    /// <param name="route">Route for the open page operation.</param>
    /// <param name="assertion">Callback used by open page.</param>
    /// <returns>A task that completes when the open page operation is complete.</returns>
    public static async ValueTask OpenPage(this IPage page, string baseUrl, string route, Func<IPage, Task> assertion)
    {
        await page.GotoAsync(baseUrl.GetRouteUrl(route), new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        }).NoSync();

        await assertion(page).NoSync();
    }

    /// <summary>
    /// Returns the value produced by visible Menu.
    /// </summary>
    /// <param name="page">Browser page to inspect or control.</param>
    /// <returns>The resulting locator.</returns>
    public static ILocator VisibleMenu(this IPage page)
    {
        return page.Locator("[role='menu']:visible")
                   .Last;
    }
}
