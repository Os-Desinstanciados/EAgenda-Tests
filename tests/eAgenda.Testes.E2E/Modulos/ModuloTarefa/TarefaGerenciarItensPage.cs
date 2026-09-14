using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.ModuloTarefa;

public sealed class TarefaGerenciarItensPage(IPage page)
{
    public ILocator NovoItem => page.GetByLabel("Novo Item");

    public ILocator EstadoVazio => page.GetByText(
        "Nenhum item cadastrado.",
        new() { Exact = true }
    );

    public ILocator Item(string titulo) => page.GetByText(
        titulo,
        new() { Exact = true }
    );

    public async Task AdicionarItemAsync(string titulo)
    {
        await NovoItem.FillAsync(titulo);

        await page.GetByRole(
            AriaRole.Button,
            new() { Name = "Adicionar", Exact = true }
        ).ClickAsync();
    }

    public async Task ConcluirItemAsync(string titulo)
    {
        await ItemPorTitulo(titulo).GetByRole(
            AriaRole.Button,
            new() { Name = "Concluir", Exact = true }
        ).ClickAsync();
    }

    public async Task RemoverItemAsync(string titulo)
    {
        await ItemPorTitulo(titulo).GetByRole(
            AriaRole.Button,
            new() { Name = "Remover", Exact = true }
        ).ClickAsync();
    }

    private ILocator ItemPorTitulo(string titulo)
    {
        ILocator item = Item(titulo);

        return page.Locator(".list-group-item").Filter(
            new() { Has = item }
        );
    }
}