using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.ModuloCompromisso;

public sealed class CompromissoListarPage(
    IPage page,
    string urlBase
)
{
    public string Url => $"{urlBase}/Compromisso/Listar";

    public ILocator Titulo => page.GetByRole(
        AriaRole.Heading,
        new() { Name = "Listagem de Compromissos" }
    );

    public ILocator EstadoVazio => page.GetByText(
        "Nenhum compromisso cadastrado.",
        new() { Exact = true }
    );

    public ILocator AssuntoDoCompromisso(string assunto) => page.GetByText(
        assunto,
        new() { Exact = true }
    );

    public async Task IrParaAsync()
    {
        await page.GotoAsync(Url);
    }

    public async Task EditarAsync(string assunto)
    {
        await CardPorAssunto(assunto).GetByRole(
            AriaRole.Link,
            new() { Name = "Editar", Exact = true }
        ).ClickAsync();
    }

    public async Task ExcluirAsync(string assunto)
    {
        await CardPorAssunto(assunto).GetByRole(
            AriaRole.Link,
            new() { Name = "Excluir", Exact = true }
        ).ClickAsync();
    }

    private ILocator CardPorAssunto(string assunto)
    {
        ILocator compromisso = AssuntoDoCompromisso(assunto);

        return page.Locator(".card").Filter(new() { Has = compromisso });
    }
}