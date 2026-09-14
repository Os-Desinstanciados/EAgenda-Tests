using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.ModuloDespesa;

public sealed class DespesaExcluirPage(IPage page)
{
    public ILocator MensagemConfirmacao => page.GetByText(
        "Deseja realmente excluir esta despesa?",
        new() { Exact = true }
    );

    public async Task ConfirmarAsync()
    {
        await page.GetByRole(
            AriaRole.Button,
            new() { Name = "Confirmar", Exact = true }
        ).ClickAsync();
    }
}