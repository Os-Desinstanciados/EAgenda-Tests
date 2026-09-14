using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.ModuloContato;

public sealed class ContatoListarPage(
    IPage page,
    string urlBase
)
{
    public string Url => $"{urlBase}/Contato/Listar";

    public ILocator Nome => page.GetByRole(
        AriaRole.Heading,
        new() { Name = "Listagem de Contatos" }
    );

    public ILocator EstadoVazio => page.GetByText(
        "Nenhum contato cadastrado.",
        new() { Exact = true }
    );

    public ILocator NomeDoContato(string nome) => page.GetByText(
        nome,
        new() { Exact = true }
    );

    public async Task IrParaAsync()
    {
        await page.GotoAsync(Url);
    }

    public async Task EditarAsync(string nome)
    {
        await CardPorNome(nome).GetByRole(
            AriaRole.Link,
            new() { Name = "Editar", Exact = true }
        ).ClickAsync();
    }

    public async Task ExcluirAsync(string nome)
    {
        await CardPorNome(nome).GetByRole(
            AriaRole.Link,
            new() { Name = "Excluir", Exact = true }
        ).ClickAsync();
    }

    private ILocator CardPorNome(string nome)
    {
        ILocator nomeContato = NomeDoContato(nome);

        return page.Locator(".card").Filter(new() { Has = nomeContato });
    }
}