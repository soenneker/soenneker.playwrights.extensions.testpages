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
    public static async ValueTask GotoAndWaitForReady(this IPage page, string url, Func<IPage, ILocator> readyLocatorFactory, string? expectedTitle = null)
    {
        await page.GotoAsync(url, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.Load
        });

        if (expectedTitle.HasContent())
            await Assertions.Expect(page)
                            .ToHaveTitleAsync(expectedTitle);

        await Assertions.Expect(readyLocatorFactory(page))
                        .ToBeVisibleAsync();
    }

    public static string GetRouteUrl(this string baseUrl, string route)
    {
        return route == "/" ? baseUrl : $"{baseUrl}{route.TrimStart('/')}";
    }

    public static async ValueTask OpenPage(this IPage page, string baseUrl, string route, Func<IPage, ValueTask> assertion)
    {
        await page.GotoAsync(baseUrl.GetRouteUrl(route), new PageGotoOptions
                  {
                      WaitUntil = WaitUntilState.Load
                  })
                  .NoSync();

        await assertion(page)
            .NoSync();
    }

    public static ILocator VisibleMenu(this IPage page)
    {
        return page.Locator("[role='menu']:visible")
                   .Last;
    }
}