using eAgenda.Dominio.Modulos.ModuloCategoria;
using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.ModuloDespesa;

public sealed class DespesaFormPage(
    IPage page,
    string urlBase
)
{
    public string UrlCadastrar => $"{urlBase}/Despesa/Cadastrar";
    public string UrlEditar => $"{urlBase}/Despesa/Editar";

    public ILocator Descricao => page.GetByLabel("Descrição");
    public ILocator DataOcorrencia => page.GetByLabel("Data de Ocorrência");
    public ILocator Valor => page.GetByLabel("Valor");
    public ILocator FormaPagamento => page.GetByLabel("Forma de Pagamento");
    public ILocator Categorias => page.GetByLabel("Categorias");

    public async Task IrParaCadastroAsync()
    {
        await page.GotoAsync(UrlCadastrar);
    }

    public async Task PreencherAsync(
        string descricao,
        string dataOcorrencia,
        string valor,
        string formaPagamento,
        string categoria)
    {
        await Descricao.FillAsync(descricao);
        await DataOcorrencia.FillAsync(dataOcorrencia);
        await Valor.FillAsync(valor);

        await FormaPagamento.SelectOptionAsync(
            new SelectOptionValue() { Label = formaPagamento }
        );

        await Categorias.SelectOptionAsync(
            new SelectOptionValue() { Label = categoria }
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