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
    /// Executes the goto and wait for ready operation.
    /// </summary>
    /// <param name="page">The page.</param>
    /// <param name="url">The url.</param>
    /// <param name="readyLocatorFactory">The ready locator factory.</param>
    /// <param name="expectedTitle">The expected title.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
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
    /// <param name="baseUrl">The base url.</param>
    /// <param name="route">The route.</param>
    /// <returns>The result of the operation.</returns>
    public static string GetRouteUrl(this string baseUrl, string route)
    {
        if (route == "/")
            return baseUrl.TrimEnd('/');

        return $"{baseUrl.TrimEnd('/')}/{route.TrimStart('/')}";
    }

    /// <summary>
    /// Executes the open page operation.
    /// </summary>
    /// <param name="page">The page.</param>
    /// <param name="baseUrl">The base url.</param>
    /// <param name="route">The route.</param>
    /// <param name="readyLocatorFactory">The ready locator factory.</param>
    /// <param name="assertion">The assertion.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
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
    /// Executes the open page operation.
    /// </summary>
    /// <param name="page">The page.</param>
    /// <param name="baseUrl">The base url.</param>
    /// <param name="route">The route.</param>
    /// <param name="assertion">The assertion.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static async ValueTask OpenPage(this IPage page, string baseUrl, string route, Func<IPage, Task> assertion)
    {
        await page.GotoAsync(baseUrl.GetRouteUrl(route), new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        }).NoSync();

        await assertion(page).NoSync();
    }

    /// <summary>
    /// Executes the visible menu operation.
    /// </summary>
    /// <param name="page">The page.</param>
    /// <returns>The result of the operation.</returns>
    public static ILocator VisibleMenu(this IPage page)
    {
        return page.Locator("[role='menu']:visible")
                   .Last;
    }
}
