using System.Text.RegularExpressions;
using eAgenda.Testes.E2E.Compartilhado;

namespace eAgenda.Testes.E2E.ModuloCompromisso;

[TestClass]
public sealed class CompromissoE2ETests : E2ETestsBase
{
    [TestMethod]
    public async Task DeveExibir_ListagemVazia_ParaUsuario_SemCompromissos()
    {
        // Arrange
        await RegistrarEEntrarAsync(
            "compromisso.listagem@teste.local",
            "Senha123!"
        );

        CompromissoListarPage listarPage = new(Page, UrlBase);

        // Act
        await listarPage.IrParaAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);
        await Expect(listarPage.Titulo).ToBeVisibleAsync();
        await Expect(listarPage.EstadoVazio).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveCadastrar_Compromisso_ComDadosValidos()
    {
        // Arrange
        await RegistrarEEntrarAsync(
            "compromisso.cadastro@teste.local",
            "Senha123!"
        );

        CompromissoFormPage formPage = new(Page, UrlBase);
        CompromissoListarPage listarPage = new(Page, UrlBase);

        // Act
        await formPage.IrParaCadastroAsync();

        await formPage.PreencherAsync(
            "Reunião com cliente",
            "2030-10-20",
            "14:00",
            "15:00",
            "Presencial",
            "Escritório"
        );

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);

        await Expect(
            listarPage.AssuntoDoCompromisso("Reunião com cliente")
        ).ToBeVisibleAsync();

        await Expect(listarPage.EstadoVazio).Not.ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveEditar_Compromisso_ComDadosValidos()
    {
        // Arrange
        await RegistrarEEntrarAsync(
            "compromisso.edicao@teste.local",
            "Senha123!"
        );

        await CadastrarCompromissoAsync(
            "Reunião com cliente",
            "2030-10-20",
            "14:00",
            "15:00",
            "Presencial",
            "Escritório"
        );

        CompromissoFormPage formPage = new(Page, UrlBase);
        CompromissoListarPage listarPage = new(Page, UrlBase);

        // Act
        await listarPage.EditarAsync("Reunião com cliente");

        await formPage.PreencherAsync(
            "Reunião atualizada",
            "2030-10-21",
            "15:00",
            "16:00",
            "Presencial",
            "Sala de reuniões"
        );

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);

        await Expect(
            listarPage.AssuntoDoCompromisso("Reunião atualizada")
        ).ToBeVisibleAsync();

        await Expect(
            listarPage.AssuntoDoCompromisso("Reunião com cliente")
        ).Not.ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveExcluir_Compromisso_SemVinculos()
    {
        // Arrange
        await RegistrarEEntrarAsync(
            "compromisso.exclusao@teste.local",
            "Senha123!"
        );

        await CadastrarCompromissoAsync(
            "Reunião com cliente",
            "2030-10-20",
            "14:00",
            "15:00",
            "Presencial",
            "Escritório"
        );

        CompromissoListarPage listarPage = new(Page, UrlBase);
        CompromissoExcluirPage excluirPage = new(Page);

        // Act
        await listarPage.ExcluirAsync("Reunião com cliente");

        await Expect(Page).ToHaveURLAsync(
            new Regex(
                $"{Regex.Escape(UrlBase)}/Compromisso/Excluir/.*"
            )
        );

        await Expect(
            excluirPage.MensagemConfirmacao
        ).ToBeVisibleAsync();

        await excluirPage.ConfirmarAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);

        await Expect(
            listarPage.AssuntoDoCompromisso("Reunião com cliente")
        ).Not.ToBeVisibleAsync();

        await Expect(listarPage.EstadoVazio).ToBeVisibleAsync();
    }

    private async Task CadastrarCompromissoAsync(
        string assunto,
        string dataOcorrencia,
        string horaInicio,
        string horaTermino,
        string tipo,
        string local)
    {
        CompromissoFormPage formPage = new(Page, UrlBase);
        CompromissoListarPage listarPage = new(Page, UrlBase);

        await formPage.IrParaCadastroAsync();

        await formPage.PreencherAsync(
            assunto,
            dataOcorrencia,
            horaInicio,
            horaTermino,
            tipo,
            local
        );

        await formPage.ConfirmarAsync();

        await Expect(
            listarPage.AssuntoDoCompromisso(assunto)
        ).ToBeVisibleAsync();
    }
}