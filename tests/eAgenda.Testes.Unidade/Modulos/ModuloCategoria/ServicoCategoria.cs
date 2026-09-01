using eAgenda.Aplicacao.Modulos.ModuloCategoria;
using eAgenda.Dominio.Modulos.ModuloCategoria;
using eAgenda.Dominio.Modulos.ModuloDespesa;
using FluentResults;
using Moq;

namespace eAgenda.Testes.Unidade.Modulos.ModuloCategoria;

[TestClass]
public sealed class ServicoCategoriaTests
{
    [TestMethod]
    public void Cadastrar_DadosValidos_PersisteCategoria()
    {
        // Arranjo
        Mock<IRepositorioCategoria> repositorioCategoria = new();
        Mock<IRepositorioDespesa> repositorioDespesa = new();

        repositorioCategoria.Setup(r => r.SelecionarTodos()).Returns([]);

        Categoria? categoriaCadastrada = null;

        repositorioCategoria
            .Setup(r => r.Cadastrar(It.IsAny<Categoria>()))
            .Callback<Categoria>(categoria => categoriaCadastrada = categoria);

        ServicoCategoria servicoCategoria = new ServicoCategoria(
            repositorioCategoria.Object,
            repositorioDespesa.Object
        );

        // Ação
        Result resultado = servicoCategoria.Cadastrar(
            new CadastrarCategoriaDto("Alimentação")
        );

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(categoriaCadastrada);
        Assert.AreEqual("Alimentação", categoriaCadastrada.Titulo);
        repositorioCategoria.Verify(r => r.Cadastrar(It.IsAny<Categoria>()), Times.Once);
    }

    [TestMethod]
    public void Cadastrar_TituloDuplicado_RetornaFalha()
    {
        // Arranjo
        Mock<IRepositorioCategoria> repositorioCategoria = new();
        Mock<IRepositorioDespesa> repositorioDespesa = new();

        Categoria categoriaExistente = new Categoria("Alimentação");

        repositorioCategoria.Setup(r => r.SelecionarTodos()).Returns([categoriaExistente]);

        ServicoCategoria servicoCategoria = new ServicoCategoria(
            repositorioCategoria.Object,
            repositorioDespesa.Object
        );

        // Ação
        Result resultado = servicoCategoria.Cadastrar(
            new CadastrarCategoriaDto("Alimentação")
        );

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.Contains("Já existe uma categoria com este título.", resultado.Errors.Single().Message);
        repositorioCategoria.Verify(r => r.Cadastrar(It.IsAny<Categoria>()), Times.Never);
    }

    [TestMethod]
    public void Editar_DadosValidos_EditaCategoria()
    {
        // Arranjo
        Mock<IRepositorioCategoria> repositorioCategoria = new();
        Mock<IRepositorioDespesa> repositorioDespesa = new();

        Guid categoriaId = Guid.NewGuid();

        repositorioCategoria.Setup(r => r.SelecionarTodos()).Returns([]);

        Categoria? categoriaEditada = null;

        repositorioCategoria
            .Setup(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Categoria>()))
            .Callback<Guid, Categoria>((id, categoria) => categoriaEditada = categoria)
            .Returns(true);

        ServicoCategoria servicoCategoria = new ServicoCategoria(
            repositorioCategoria.Object,
            repositorioDespesa.Object
        );

        // Ação
        Result resultado = servicoCategoria.Editar(
            new EditarCategoriaDto(
                categoriaId,
                "Transporte"
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(categoriaEditada);
        Assert.AreEqual("Transporte", categoriaEditada.Titulo);
        repositorioCategoria.Verify(r => r.Editar(categoriaId, It.IsAny<Categoria>()), Times.Once);
    }

    [TestMethod]
    public void Editar_TituloDuplicado_RetornaFalha()
    {
        // Arranjo
        Mock<IRepositorioCategoria> repositorioCategoria = new();
        Mock<IRepositorioDespesa> repositorioDespesa = new();

        Guid categoriaId = Guid.NewGuid();

        Categoria categoriaExistente = new Categoria("Alimentação");

        repositorioCategoria.Setup(r => r.SelecionarTodos()).Returns([categoriaExistente]);

        ServicoCategoria servicoCategoria = new ServicoCategoria(
            repositorioCategoria.Object,
            repositorioDespesa.Object
        );

        // Ação
        Result resultado = servicoCategoria.Editar(
            new EditarCategoriaDto(
                categoriaId,
                "Alimentação"
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.Contains("Já existe uma categoria com este título.", resultado.Errors.Single().Message);
        repositorioCategoria.Verify(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Categoria>()), Times.Never);
    }

    [TestMethod]
    public void Excluir_CategoriaSemDespesasVinculadas_ExcluiCategoria()
    {
        // Arranjo
        Mock<IRepositorioCategoria> repositorioCategoria = new();
        Mock<IRepositorioDespesa> repositorioDespesa = new();

        Categoria categoria = new Categoria("Alimentação");

        repositorioCategoria.Setup(r => r.SelecionarPorId(categoria.Id)).Returns(categoria);
        repositorioDespesa.Setup(r => r.SelecionarTodos()).Returns([]);

        ServicoCategoria servicoCategoria = new ServicoCategoria(
            repositorioCategoria.Object,
            repositorioDespesa.Object
        );

        // Ação
        Result resultado = servicoCategoria.Excluir(categoria.Id);

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        repositorioCategoria.Verify(r => r.Excluir(categoria.Id), Times.Once);
    }

    [TestMethod]
    public void Excluir_CategoriaComDespesaVinculada_RetornaFalha()
    {
        // Arranjo
        Mock<IRepositorioCategoria> repositorioCategoria = new();
        Mock<IRepositorioDespesa> repositorioDespesa = new();

        Categoria categoria = new Categoria("Alimentação");

        Despesa despesa = new Despesa(
            "Compra do mês",
            new DateTime(2026, 08, 31),
            100,
            FormaPagamento.AVista,
            new List<Categoria> { categoria }
        );

        repositorioCategoria.Setup(r => r.SelecionarPorId(categoria.Id)).Returns(categoria);
        repositorioDespesa.Setup(r => r.SelecionarTodos()).Returns([despesa]);

        ServicoCategoria servicoCategoria = new ServicoCategoria(
            repositorioCategoria.Object,
            repositorioDespesa.Object
        );

        // Ação
        Result resultado = servicoCategoria.Excluir(categoria.Id);

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.Contains("Não é possível excluir esta categoria, pois ela possui despesas vinculadas.", resultado.Errors.Single().Message);
        repositorioCategoria.Verify(r => r.Excluir(It.IsAny<Guid>()), Times.Never);
    }

    [TestMethod]
    public void SelecionarTodos_ComCategoriasCadastradas_RetornaCategorias()
    {
        // Arranjo
        Mock<IRepositorioCategoria> repositorioCategoria = new();
        Mock<IRepositorioDespesa> repositorioDespesa = new();

        Categoria categoriaAlimentacao = new Categoria("Alimentação");
        Categoria categoriaTransporte = new Categoria("Transporte");

        repositorioCategoria
            .Setup(r => r.SelecionarTodos())
            .Returns([categoriaAlimentacao, categoriaTransporte]);

        ServicoCategoria servicoCategoria = new ServicoCategoria(
            repositorioCategoria.Object,
            repositorioDespesa.Object
        );

        // Ação
        List<ListarCategoriasDto> categorias = servicoCategoria.SelecionarTodos();

        // Asserção
        Assert.HasCount(2, categorias);
        Assert.AreEqual("Alimentação", categorias[0].Titulo);
        Assert.AreEqual("Transporte", categorias[1].Titulo);
    }
}