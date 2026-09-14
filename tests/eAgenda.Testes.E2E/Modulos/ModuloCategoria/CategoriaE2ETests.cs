using System.Text.RegularExpressions;
using eAgenda.Testes.E2E.Compartilhado;

namespace eAgenda.Testes.E2E.ModuloCategoria;

[TestClass]
public sealed class CategoriaE2ETests : E2ETestsBase
{
    [TestMethod]
    public async Task DeveExibir_ListagemVazia_ParaUsuario_SemCategorias()
    {
        // Arrange
        await RegistrarEEntrarAsync(
            "categoria.listagem@teste.local",
            "Senha123!"
        );

        CategoriaListarPage listarPage = new(Page, UrlBase);

        // Act
        await listarPage.IrParaAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);
        await Expect(listarPage.Titulo).ToBeVisibleAsync();
        await Expect(listarPage.EstadoVazio).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveCadastrar_Categoria_ComDadosValidos()
    {
        // Arrange
        await RegistrarEEntrarAsync(
            "categoria.cadastro@teste.local",
            "Senha123!"
        );

        CategoriaFormPage formPage = new(Page, UrlBase);
        CategoriaListarPage listarPage = new(Page, UrlBase);

        // Act
        await formPage.IrParaCadastroAsync();
        await formPage.PreencherTituloAsync("Alimentação");
        await formPage.ConfirmarAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);
        await Expect(
            listarPage.TituloDaCategoria("Alimentação")
        ).ToBeVisibleAsync();

        await Expect(listarPage.EstadoVazio).Not.ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveEditar_Categoria_ComDadosValidos()
    {
        // Arrange
        await RegistrarEEntrarAsync(
            "categoria.edicao@teste.local",
            "Senha123!"
        );

        await CadastrarCategoriaAsync("Alimentação");

        CategoriaFormPage formPage = new(Page, UrlBase);
        CategoriaListarPage listarPage = new(Page, UrlBase);

        // Act
        await listarPage.EditarAsync("Alimentação");
        await formPage.PreencherTituloAsync("Transporte");
        await formPage.ConfirmarAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);

        await Expect(
            listarPage.TituloDaCategoria("Transporte")
        ).ToBeVisibleAsync();

        await Expect(
            listarPage.TituloDaCategoria("Alimentação")
        ).Not.ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task DeveExcluir_Categoria_SemVinculos()
    {
        // Arrange
        await RegistrarEEntrarAsync(
            "categoria.exclusao@teste.local",
            "Senha123!"
        );

        await CadastrarCategoriaAsync("Alimentação");

        CategoriaListarPage listarPage = new(Page, UrlBase);
        CategoriaExcluirPage excluirPage = new(Page);

        // Act
        await listarPage.ExcluirAsync("Alimentação");

        await Expect(Page).ToHaveURLAsync(
            new Regex(
                $"{Regex.Escape(UrlBase)}/Categoria/Excluir/.*"
            )
        );

        await Expect(
            excluirPage.MensagemConfirmacao
        ).ToBeVisibleAsync();

        await excluirPage.ConfirmarAsync();

        // Assert
        await Expect(Page).ToHaveURLAsync(listarPage.Url);

        await Expect(
            listarPage.TituloDaCategoria("Alimentação")
        ).Not.ToBeVisibleAsync();

        await Expect(listarPage.EstadoVazio).ToBeVisibleAsync();
    }

    private async Task CadastrarCategoriaAsync(string titulo)
    {
        CategoriaFormPage formPage = new(Page, UrlBase);
        CategoriaListarPage listarPage = new(Page, UrlBase);

        await formPage.IrParaCadastroAsync();
        await formPage.PreencherTituloAsync(titulo);
        await formPage.ConfirmarAsync();

        await Expect(
            listarPage.TituloDaCategoria(titulo)
        ).ToBeVisibleAsync();
    }
}