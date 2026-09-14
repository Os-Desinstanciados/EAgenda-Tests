using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.ModuloTarefa;

public sealed class TarefaListarPage(
    IPage page,
    string urlBase
)
{
    public string Url => $"{urlBase}/Tarefa/Listar";

    public ILocator EstadoVazio => page.GetByText(
        "Nenhuma tarefa cadastrada.",
        new() { Exact = true }
    );

    public ILocator TituloDaTarefa(string titulo) => page.GetByRole(
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

    public async Task GerenciarItensAsync(string titulo)
    {
        await CardPorTitulo(titulo).GetByRole(
            AriaRole.Link,
            new() { Name = "Itens", Exact = true }
        ).ClickAsync();
    }

    private ILocator CardPorTitulo(string titulo)
    {
        ILocator tarefa = TituloDaTarefa(titulo);

        return page.Locator(".card").Filter(new() { Has = tarefa });
    }
}