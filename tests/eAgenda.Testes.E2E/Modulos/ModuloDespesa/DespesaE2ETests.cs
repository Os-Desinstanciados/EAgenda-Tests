using System.Text.RegularExpressions;
using eAgenda.Testes.E2E.Compartilhado;
using eAgenda.Testes.E2E.ModuloCategoria;

namespace eAgenda.Testes.E2E.ModuloDespesa;

[TestClass]
public sealed class DespesaE2ETests : E2ETestsBase
{
    [TestMethod]
    public async Task DeveExibir_ListagemVazia_ParaUsuario_SemDespesas()
    {
        // Arrange
        await RegistrarEEntrarAsync(
            "despesa.listagem@teste.local",
            "Senha123!"
        );

        DespesaListarPage listarPage = new(Page, UrlBase);

        // Act
        await listarPage.IrParaAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);
        await Expect(listarPage.Titulo).ToBeVisibleAsync();
        await Expect(listarPage.EstadoVazio).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveCadastrar_Despesa_ComDadosValidos()
    {
        // Arrange
        await RegistrarEEntrarAsync(
            "despesa.cadastro@teste.local",
            "Senha123!"
        );

        await CadastrarCategoriaAsync("Alimentação");

        DespesaFormPage formPage = new(Page, UrlBase);
        DespesaListarPage listarPage = new(Page, UrlBase);

        // Act
        await formPage.IrParaCadastroAsync();

        await formPage.PreencherAsync(
            "Almoço",
            "2030-10-20",
            "50.00",
            "À Vista",
            "Alimentação"
        );

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);

        await Expect(
            listarPage.DescricaoDaDespesa("Almoço")
        ).ToBeVisibleAsync();

        await Expect(listarPage.EstadoVazio).Not.ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveEditar_Despesa_ComDadosValidos()
    {
        // Arrange
        await RegistrarEEntrarAsync(
            "despesa.edicao@teste.local",
            "Senha123!"
        );

        await CadastrarCategoriaAsync("Alimentação");

        await CadastrarDespesaAsync(
            "Almoço",
            "2030-10-20",
            "50.00",
            "À Vista",
            "Alimentação"
        );

        DespesaFormPage formPage = new(Page, UrlBase);
        DespesaListarPage listarPage = new(Page, UrlBase);

        // Act
        await listarPage.EditarAsync("Almoço");

        await formPage.PreencherAsync(
            "Jantar",
            "2030-10-21",
            "80.00",
            "Crédito",
            "Alimentação"
        );

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);

        await Expect(
            listarPage.DescricaoDaDespesa("Jantar")
        ).ToBeVisibleAsync();

        await Expect(
            listarPage.DescricaoDaDespesa("Almoço")
        ).Not.ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveExcluir_Despesa_SemVinculos()
    {
        // Arrange
        await RegistrarEEntrarAsync(
            "despesa.exclusao@teste.local",
            "Senha123!"
        );

        await CadastrarCategoriaAsync("Alimentação");

        await CadastrarDespesaAsync(
            "Almoço",
            "2030-10-20",
            "50.00",
            "À Vista",
            "Alimentação"
        );

        DespesaListarPage listarPage = new(Page, UrlBase);
        DespesaExcluirPage excluirPage = new(Page);

        // Act
        await listarPage.ExcluirAsync("Almoço");

        await Expect(Page).ToHaveURLAsync(
            new Regex(
                $"{Regex.Escape(UrlBase)}/Despesa/Excluir/.*"
            )
        );

        await Expect(
            excluirPage.MensagemConfirmacao
        ).ToBeVisibleAsync();

        await excluirPage.ConfirmarAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);

        await Expect(
            listarPage.DescricaoDaDespesa("Almoço")
        ).Not.ToBeVisibleAsync();

        await Expect(listarPage.EstadoVazio).ToBeVisibleAsync();
    }

    private async Task CadastrarCategoriaAsync(string titulo)
    {
        CategoriaFormPage formPage = new(Page, UrlBase);

        await formPage.IrParaCadastroAsync();
        await formPage.PreencherTituloAsync(titulo);
        await formPage.ConfirmarAsync();
    }

    private async Task CadastrarDespesaAsync(
        string descricao,
        string dataOcorrencia,
        string valor,
        string formaPagamento,
        string categoria)
    {
        DespesaFormPage formPage = new(Page, UrlBase);
        DespesaListarPage listarPage = new(Page, UrlBase);

        await formPage.IrParaCadastroAsync();

        await formPage.PreencherAsync(
            descricao,
            dataOcorrencia,
            valor,
            formaPagamento,
            categoria
        );

        await formPage.ConfirmarAsync();

        await Expect(
            listarPage.DescricaoDaDespesa(descricao)
        ).ToBeVisibleAsync();
    }
}