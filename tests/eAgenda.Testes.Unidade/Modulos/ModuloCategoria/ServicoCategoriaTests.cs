using eAgenda.Aplicacao.Modulos.ModuloCategoria;
using eAgenda.Dominio.Modulos.ModuloDespesa;
using eAgenda.Dominio.Modulos.ModuloCategoria;
using FluentResults;
using Moq;

namespace eAgenda.Testes.Unidade.Modulos.ModuloCategoria;

[TestClass]
public sealed class ServicoCategoriaTests
{
    [TestMethod]
    public void Cadastrar_DadosValidosPersisteCategoria()
    {
        // Arranjo
        Mock<IRepositorioCategoria> repositorioCategoria = new Mock<IRepositorioCategoria>();
        Mock<IRepositorioDespesa> repositorioDespesa = new Mock<IRepositorioDespesa>();

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
            new CadastrarCategoriaDto(
            "Categoria Teste"            
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(categoriaCadastrada);
        Assert.AreEqual("Categoria Teste", categoriaCadastrada.Titulo);        
        repositorioCategoria.Verify(r => r.Cadastrar(It.IsAny<Categoria>()), Times.Once);
    }    

    [TestMethod]
    public void Editar_DadosValidos_EditaCategoria()
    {
        // Arranjo
        Mock<IRepositorioCategoria> repositorioCategoria = new();
        Mock<IRepositorioDespesa> repositorioDespesa = new();

        repositorioCategoria
            .Setup(r => r.SelecionarTodos())
            .Returns([]);

        Categoria? categoriaEditada = null;

        repositorioCategoria
            .Setup(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Categoria>()))
            .Callback<Guid, Categoria>((id, categoria) => categoriaEditada = categoria)
            .Returns(true);

        ServicoCategoria servicoCategoria = new ServicoCategoria(
            repositorioCategoria.Object,
            repositorioDespesa.Object
        );

        Guid categoriaId = Guid.NewGuid();

        // Ação
        Result resultado = servicoCategoria.Editar(
            new EditarCategoriaDto(
                categoriaId,
                "Categoria Editado"
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(categoriaEditada);
        Assert.AreEqual("Categoria Editado", categoriaEditada.Titulo);        
        repositorioCategoria.Verify(r => r.Editar(categoriaId, It.IsAny<Categoria>()), Times.Once);
    }     

    [TestMethod]
    public void Excluir_CategoriaSemDespesasVinculadas_ExcluiCategoria()
    {
        // Arranjo
        Categoria categoria = new Categoria(
            "Categ Teste"
        );

        Mock<IRepositorioCategoria> repositorioCategoria = new();
        Mock<IRepositorioDespesa> repositorioDespesa = new();

        repositorioCategoria
            .Setup(c => c.SelecionarPorId(categoria.Id))
            .Returns(categoria);

        repositorioDespesa
            .Setup(c => c.SelecionarTodos())
            .Returns([]);

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
    public void Excluir_CategoriaComDespesasVinculadas_RetornaFalha()
    {
        // Arranjo
        Categoria categoria = new Categoria(
            "Categ Teste"
        );

        Mock<IRepositorioCategoria> repositorioCategoria = new();
        Mock<IRepositorioDespesa> repositorioDespesa = new();

        repositorioCategoria
            .Setup(c => c.SelecionarPorId(categoria.Id))
            .Returns(categoria);

        repositorioDespesa
            .Setup(c => c.SelecionarTodos())
            .Returns([new Despesa(
                "TesteDeDespesa",
                new DateTime(2026, 12, 08),
                100,
                0,                
                new List<Categoria> { categoria }
            )]);

        ServicoCategoria servicoCategoria = new ServicoCategoria(
            repositorioCategoria.Object,
            repositorioDespesa.Object
        );

        // Ação
        Result resultado = servicoCategoria.Excluir(categoria.Id);

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        repositorioCategoria.Verify(r => r.Excluir(categoria.Id), Times.Never);

    }
}