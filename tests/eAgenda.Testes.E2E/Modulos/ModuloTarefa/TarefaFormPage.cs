using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.ModuloTarefa;

public sealed class TarefaFormPage(
    IPage page,
    string urlBase
)
{
    public string UrlCadastrar => $"{urlBase}/Tarefa/Cadastrar";
    public string UrlEditar => $"{urlBase}/Tarefa/Editar";

    public ILocator Titulo => page.GetByLabel("Título");
    public ILocator Prioridade => page.GetByLabel("Prioridade");

    public async Task IrParaCadastroAsync()
    {
        await page.GotoAsync(UrlCadastrar);
    }

    public async Task PreencherAsync(
        string titulo,
        string prioridade)
    {
        await Titulo.FillAsync(titulo);

        await Prioridade.SelectOptionAsync(
            new SelectOptionValue() { Label = prioridade }
        );
    }

    public async Task ConfirmarAsync()
    {
        await page.GetByRole(
            AriaRole.Button,
            new() { Name = "Confirmar" }
        ).ClickAsync();
    }
}