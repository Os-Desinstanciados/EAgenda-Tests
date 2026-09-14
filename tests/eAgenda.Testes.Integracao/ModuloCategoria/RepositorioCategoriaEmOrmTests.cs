using eAgenda.Dominio.Modulos.ModuloCategoria;
using eAgenda.Infra.Compartilhado.Orm;
using eAgenda.Infra.Modulos.ModuloCategoria;
using eAgenda.Testes.Integracao.Compartilhado.Orm;

namespace eAgenda.Testes.Integracao.ModuloCategoria;

[TestClass]
public sealed class RepositorioCategoriaEmOrmTests : RepositorioEmOrmBaseTests
{
    private EAgendaDbContext dbContext = null!;
    private RepositorioCategoriaEmOrm repositorio = null!;

    [TestInitialize]
    public void InicializarRepositorio()
    {
        dbContext = CriarDbContext();

        repositorio = new RepositorioCategoriaEmOrm(dbContext);
    }

    [TestMethod]
    public void CadastrarESelecionarPorId_CarregaCategoria()
    {
        // Arranjo
        Categoria categoria = new Categoria("Alimentação");

        // Ação
        repositorio.Cadastrar(categoria);
        dbContext.ChangeTracker.Clear();

        Categoria? categoriaSelecionada =
            repositorio.SelecionarPorId(categoria.Id);

        // Asserção
        Assert.IsNotNull(categoriaSelecionada);
        Assert.AreEqual("Alimentação", categoriaSelecionada.Titulo);
    }

    [TestMethod]
    public void Editar_AtualizaCategoriaExistente()
    {
        // Arranjo
        Categoria categoria = new Categoria("Alimentação");

        repositorio.Cadastrar(categoria);

        Categoria categoriaAtualizada = new Categoria("Transporte");

        // Ação
        bool conseguiuEditar =
            repositorio.Editar(categoria.Id, categoriaAtualizada);

        dbContext.ChangeTracker.Clear();

        Categoria? categoriaSelecionada =
            repositorio.SelecionarPorId(categoria.Id);

        // Asserção
        Assert.IsTrue(conseguiuEditar);
        Assert.IsNotNull(categoriaSelecionada);
        Assert.AreEqual("Transporte", categoriaSelecionada.Titulo);
    }

    [TestMethod]
    public void Excluir_RemoveCategoriaExistente()
    {
        // Arranjo
        Categoria categoria = new Categoria("Alimentação");

        repositorio.Cadastrar(categoria);

        // Ação
        bool conseguiuExcluir = repositorio.Excluir(categoria.Id);

        dbContext.ChangeTracker.Clear();

        Categoria? categoriaSelecionada =
            repositorio.SelecionarPorId(categoria.Id);

        // Asserção
        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(categoriaSelecionada);
    }

    [TestMethod]
    public void SelecionarTodos_RetornaCategoriasCadastradas()
    {
        // Arranjo
        Categoria categoria1 = new Categoria("Alimentação");
        Categoria categoria2 = new Categoria("Transporte");
        Categoria categoria3 = new Categoria("Lazer");

        repositorio.Cadastrar(categoria1);
        repositorio.Cadastrar(categoria2);
        repositorio.Cadastrar(categoria3);

        dbContext.ChangeTracker.Clear();

        // Ação
        List<Categoria> categorias = repositorio.SelecionarTodos();

        // Asserção
        Assert.HasCount(3, categorias);
    }
}