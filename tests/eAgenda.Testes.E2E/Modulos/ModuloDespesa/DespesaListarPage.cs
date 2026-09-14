using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.ModuloDespesa;

public sealed class DespesaListarPage(
    IPage page,
    string urlBase
)
{
    public string Url => $"{urlBase}/Despesa/Listar";

    public ILocator Titulo => page.GetByRole(
        AriaRole.Heading,
        new() { Name = "Listagem de Despesas" }
    );

    public ILocator EstadoVazio => page.GetByText(
        "Nenhuma despesa cadastrada.",
        new() { Exact = true }
    );

    public ILocator DescricaoDaDespesa(string descricao) => page.GetByText(
        descricao,
        new() { Exact = true }
    );

    public async Task IrParaAsync()
    {
        await page.GotoAsync(Url);
    }

    public async Task EditarAsync(string descricao)
    {
        await CardPorDescricao(descricao).GetByRole(
            AriaRole.Link,
            new() { Name = "Editar", Exact = true }
        ).ClickAsync();
    }

    public async Task ExcluirAsync(string descricao)
    {
        await CardPorDescricao(descricao).GetByRole(
            AriaRole.Link,
            new() { Name = "Excluir", Exact = true }
        ).ClickAsync();
    }

    private ILocator CardPorDescricao(string descricao)
    {
        ILocator despesa = DescricaoDaDespesa(descricao);

        return page.Locator(".card").Filter(new() { Has = despesa });
    }
}