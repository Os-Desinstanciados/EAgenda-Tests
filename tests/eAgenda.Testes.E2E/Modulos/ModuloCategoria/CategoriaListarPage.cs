using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.ModuloCategoria;

public sealed class CategoriaListarPage(
    IPage page,
    string urlBase
)
{
    public string Url => $"{urlBase}/Categoria/Listar";

    public ILocator Titulo => page.GetByRole(
        AriaRole.Heading,
        new() { Name = "Listagem de Categorias" }
    );

    public ILocator EstadoVazio => page.GetByText(
        "Nenhuma categoria cadastrada.",
        new() { Exact = true }
    );

    public ILocator TituloDaCategoria(string titulo) => page.GetByRole(
        AriaRole.Heading,
        new() { Name = titulo, Exact = true }
    );

    public async Task IrParaAsync()
    {
        await page.GotoAsync(Url);
    }

    public async Task EditarAsync(string titulo)
    {
        await CardPorTitulo(titulo).GetByRole(
            AriaRole.Link,
            new() { Name = "Editar", Exact = true }
        ).ClickAsync();
    }

    public async Task ExcluirAsync(string titulo)
    {
        await CardPorTitulo(titulo).GetByRole(
            AriaRole.Link,
            new() { Name = "Excluir", Exact = true }
        ).ClickAsync();
    }

    private ILocator CardPorTitulo(string titulo)
    {
        ILocator tituloCategoria = TituloDaCategoria(titulo);

        return page.Locator(".card").Filter(new() { Has = tituloCategoria });
    }
}