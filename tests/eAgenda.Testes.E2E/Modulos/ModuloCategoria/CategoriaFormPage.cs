using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.ModuloCategoria;

public sealed class CategoriaFormPage(
    IPage page,
    string urlBase
)
{
    public string UrlCadastrar => $"{urlBase}/Categoria/Cadastrar";
    public string UrlEditar => $"{urlBase}/Categoria/Editar";

    public ILocator Titulo => page.GetByLabel("Título");

    public async Task IrParaCadastroAsync()
    {
        await page.GotoAsync(UrlCadastrar);
    }

    public async Task PreencherTituloAsync(string titulo)
    {
        await Titulo.FillAsync(titulo);
    }

    public async Task ConfirmarAsync()
    {
        await page.GetByRole(
            AriaRole.Button,
            new() { Name = "Confirmar" }
        ).ClickAsync();
    }
}