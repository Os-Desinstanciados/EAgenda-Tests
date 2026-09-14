using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.ModuloCompromisso;

public sealed class CompromissoExcluirPage(IPage page)
{
    public ILocator MensagemConfirmacao => page.GetByText(
        "Deseja realmente excluir este compromisso?",
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