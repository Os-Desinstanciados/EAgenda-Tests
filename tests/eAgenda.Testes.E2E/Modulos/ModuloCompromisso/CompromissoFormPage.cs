using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.ModuloCompromisso;

public sealed class CompromissoFormPage(
    IPage page,
    string urlBase
)
{
    public string UrlCadastrar => $"{urlBase}/Compromisso/Cadastrar";
    public string UrlEditar => $"{urlBase}/Compromisso/Editar";

    public ILocator Assunto => page.GetByLabel("Assunto");
    public ILocator DataOcorrencia => page.GetByLabel("Data de Ocorrência");
    public ILocator HoraInicio => page.GetByLabel("Hora de Início");
    public ILocator HoraTermino => page.GetByLabel("Hora de Término");
    public ILocator Tipo => page.GetByLabel("Tipo de Compromisso");
    public ILocator Local => page.GetByLabel("Local");

    public async Task IrParaCadastroAsync()
    {
        await page.GotoAsync(UrlCadastrar);
    }

    public async Task PreencherAsync(
        string assunto,
        string dataOcorrencia,
        string horaInicio,
        string horaTermino,
        string tipo,
        string local)
    {
        await Assunto.FillAsync(assunto);
        await DataOcorrencia.FillAsync(dataOcorrencia);
        await HoraInicio.FillAsync(horaInicio);
        await HoraTermino.FillAsync(horaTermino);
        await Tipo.SelectOptionAsync(new SelectOptionValue()
        {
            Label = tipo
        });
        await Local.FillAsync(local);
    }

    public async Task ConfirmarAsync()
    {
        await page.GetByRole(
            AriaRole.Button,
            new() { Name = "Confirmar" }
        ).ClickAsync();
    }
}