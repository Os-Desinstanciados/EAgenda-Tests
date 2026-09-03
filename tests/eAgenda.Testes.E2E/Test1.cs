using System.Text.RegularExpressions;
using Microsoft.Playwright.MSTest;

namespace eAgenda.Testes.E2E;

[TestClass]
public sealed class Test1 : PageTest
{
    [TestMethod]
    public async Task TestMethod1()
    {
        await Page.GotoAsync("https://playwright.dev");

        await Expect(Page).ToHaveTitleAsync(new Regex("Playwright"));
    }
}
