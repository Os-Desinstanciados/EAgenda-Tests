using System.Text.RegularExpressions;
using eAgenda.Testes.E2E.Compartilhado;
using Microsoft.Playwright;

namespace eAgenda.Testes.E2E.ModuloContato;

[TestClass]
public sealed class ContatoE2ETests : E2ETestsBase
{
    [TestMethod]
    public async Task DeveExibir_ListagemVazia_ParaUsuario_SemContatos()
    {
        // Arrange
        await RegistrarEEntrarAsync("contato.listagem@teste.local", "Senha123!");

        ContatoListarPage listarPage = new(Page, UrlBase);

        // Act
        await listarPage.IrParaAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);
        await Expect(listarPage.Nome).ToBeVisibleAsync();
        await Expect(listarPage.EstadoVazio).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveCadastrar_Contato_ComDadosValidos()
    {
        // Arrange
        await RegistrarEEntrarAsync("contato.cadastro@teste.local", "Senha123!");

        ContatoFormPage formPage = new(Page, UrlBase);
        ContatoListarPage listarPage = new(Page, UrlBase);

        // Act
        await formPage.IrParaCadastroAsync();

        await formPage.PreencherAsync(
            "João Alves",
            "joao@teste.local",
            "(49) 99999-9999",
            "Desenvolvedor",
            "Academia"
        );

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);
        await Expect(listarPage.NomeDoContato("João Alves")).ToBeVisibleAsync();
        await Expect(listarPage.EstadoVazio).Not.ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveEditar_Contato_ComDadosValidos()
    {
        // Arrange
        await RegistrarEEntrarAsync("contato.edicao@teste.local", "Senha123!");

        await CadastrarContatoAsync(
            "João Alves",
            "joao@teste.local",
            "(49) 99999-9999",
            "Desenvolvedor",
            "Academia"
        );

        ContatoFormPage formPage = new(Page, UrlBase);
        ContatoListarPage listarPage = new(Page, UrlBase);

        // Act
        await listarPage.EditarAsync("João Alves");

        await formPage.PreencherAsync(
            "José Silva",
            "jose@teste.local",
            "(49) 88888-8888",
            "Analista",
            "Empresa Teste"
        );

        await formPage.ConfirmarAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);
        await Expect(listarPage.NomeDoContato("José Silva")).ToBeVisibleAsync();
        await Expect(listarPage.NomeDoContato("João Alves")).Not.ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveExcluir_Contato_SemVinculos()
    {
        // Arrange
        await RegistrarEEntrarAsync("contato.exclusao@teste.local", "Senha123!");

        await CadastrarContatoAsync(
            "João Alves",
            "joao@teste.local",
            "(49) 99999-9999",
            "Desenvolvedor",
            "Academia"
        );

        ContatoListarPage listarPage = new(Page, UrlBase);
        ContatoExcluirPage excluirPage = new(Page);

        // Act
        await listarPage.ExcluirAsync("João Alves");

        await Expect(Page).ToHaveURLAsync(
            new Regex($"{Regex.Escape(UrlBase)}/Contato/Excluir/.*")
        );

        await Expect(excluirPage.MensagemConfirmacao).ToBeVisibleAsync();

        await excluirPage.ConfirmarAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);
        await Expect(listarPage.NomeDoContato("João Alves")).Not.ToBeVisibleAsync();
        await Expect(listarPage.EstadoVazio).ToBeVisibleAsync();
    }

    private async Task CadastrarContatoAsync(
        string nome,
        string email,
        string telefone,
        string cargo,
        string empresa)
    {
        ContatoFormPage formPage = new(Page, UrlBase);
        ContatoListarPage listarPage = new(Page, UrlBase);

        await formPage.IrParaCadastroAsync();

        await formPage.PreencherAsync(
            nome,
            email,
            telefone,
            cargo,
            empresa
        );

        await formPage.ConfirmarAsync();

        await Expect(listarPage.NomeDoContato(nome)).ToBeVisibleAsync();
    }
}