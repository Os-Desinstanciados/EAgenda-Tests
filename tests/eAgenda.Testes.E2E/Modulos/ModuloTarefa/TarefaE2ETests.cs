using System.Text.RegularExpressions;
using eAgenda.Testes.E2E.Compartilhado;

namespace eAgenda.Testes.E2E.ModuloTarefa;

[TestClass]
public sealed class TarefaE2ETests : E2ETestsBase
{
    [TestMethod]
    public async Task DeveExibir_ListagemVazia_ParaUsuario_SemTarefas()
    {
        // Arrange
        await RegistrarEEntrarAsync(
            "tarefa.listagem@teste.local",
            "Senha123!"
        );

        TarefaListarPage listarPage = new(Page, UrlBase);

        // Act
        await listarPage.IrParaAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);
        await Expect(listarPage.EstadoVazio).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveCadastrar_Tarefa_ComDadosValidos()
    {
        // Arrange
        await RegistrarEEntrarAsync(
            "tarefa.cadastro@teste.local",
            "Senha123!"
        );

        TarefaFormPage formPage = new(Page, UrlBase);
        TarefaListarPage listarPage = new(Page, UrlBase);

        // Act
        await formPage.IrParaCadastroAsync();

        await formPage.PreencherAsync(
            "Estudar Playwright",
            "Alta"
        );

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);

        await Expect(
            listarPage.TituloDaTarefa("Estudar Playwright")
        ).ToBeVisibleAsync();

        await Expect(listarPage.EstadoVazio).Not.ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveEditar_Tarefa_ComDadosValidos()
    {
        // Arrange
        await RegistrarEEntrarAsync(
            "tarefa.edicao@teste.local",
            "Senha123!"
        );

        await CadastrarTarefaAsync(
            "Estudar Playwright",
            "Alta"
        );

        TarefaFormPage formPage = new(Page, UrlBase);
        TarefaListarPage listarPage = new(Page, UrlBase);

        // Act
        await listarPage.EditarAsync("Estudar Playwright");

        await formPage.PreencherAsync(
            "Estudar testes E2E",
            "Normal"
        );

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);

        await Expect(
            listarPage.TituloDaTarefa("Estudar testes E2E")
        ).ToBeVisibleAsync();

        await Expect(
            listarPage.TituloDaTarefa("Estudar Playwright")
        ).Not.ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveExcluir_Tarefa_SemVinculos()
    {
        // Arrange
        await RegistrarEEntrarAsync(
            "tarefa.exclusao@teste.local",
            "Senha123!"
        );

        await CadastrarTarefaAsync(
            "Estudar Playwright",
            "Alta"
        );

        TarefaListarPage listarPage = new(Page, UrlBase);
        TarefaExcluirPage excluirPage = new(Page);

        // Act
        await listarPage.ExcluirAsync("Estudar Playwright");

        await Expect(Page).ToHaveURLAsync(
            new Regex(
                $"{Regex.Escape(UrlBase)}/Tarefa/Excluir/.*"
            )
        );

        await Expect(
            excluirPage.MensagemConfirmacao
        ).ToBeVisibleAsync();

        await excluirPage.ConfirmarAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);

        await Expect(
            listarPage.TituloDaTarefa("Estudar Playwright")
        ).Not.ToBeVisibleAsync();

        await Expect(listarPage.EstadoVazio).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveAdicionarEConcluir_ItemDaTarefa()
    {
        // Arrange
        await RegistrarEEntrarAsync(
            "tarefa.itens@teste.local",
            "Senha123!"
        );

        await CadastrarTarefaAsync(
            "Estudar Playwright",
            "Alta"
        );

        TarefaListarPage listarPage = new(Page, UrlBase);
        TarefaGerenciarItensPage itensPage = new(Page);

        // Act
        await listarPage.GerenciarItensAsync("Estudar Playwright");

        await Expect(itensPage.EstadoVazio).ToBeVisibleAsync();

        await itensPage.AdicionarItemAsync("Criar testes E2E");

        // Assert
        await Expect(
            itensPage.Item("Criar testes E2E")
        ).ToBeVisibleAsync();

        await Expect(itensPage.EstadoVazio).Not.ToBeVisibleAsync();

        // Act
        await itensPage.ConcluirItemAsync("Criar testes E2E");

        // Assert
        await Expect(
            Page.GetByText("100%", new() { Exact = true }).First
        ).ToBeVisibleAsync();
    }

    private async Task CadastrarTarefaAsync(
        string titulo,
        string prioridade)
    {
        TarefaFormPage formPage = new(Page, UrlBase);
        TarefaListarPage listarPage = new(Page, UrlBase);

        await formPage.IrParaCadastroAsync();

        await formPage.PreencherAsync(
            titulo,
            prioridade
        );

        await formPage.ConfirmarAsync();

        await Expect(
            listarPage.TituloDaTarefa(titulo)
        ).ToBeVisibleAsync();
    }
}