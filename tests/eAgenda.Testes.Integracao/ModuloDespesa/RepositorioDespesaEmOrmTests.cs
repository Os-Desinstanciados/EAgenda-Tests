using eAgenda.Dominio.Modulos.ModuloCategoria;
using eAgenda.Dominio.Modulos.ModuloDespesa;
using eAgenda.Infra.Compartilhado.Orm;
using eAgenda.Infra.Modulos.ModuloDespesa;
using eAgenda.Testes.Integracao.Compartilhado.Orm;

namespace eAgenda.Testes.Integracao.ModuloDespesa;

[TestClass]
public sealed class RepositorioDespesaEmOrmTests : RepositorioEmOrmBaseTests
{
    private EAgendaDbContext dbContext = null!;
    private RepositorioDespesaEmOrm repositorio = null!;

    [TestInitialize]
    public void InicializarRepositorio()
    {
        dbContext = CriarDbContext();

        repositorio = new RepositorioDespesaEmOrm(dbContext);
    }

    [TestMethod]
    public void CadastrarESelecionarPorId_CarregaRelacionamentosDaDespesa()
    {
        // Arranjo
        Categoria categoria = new Categoria("Alimentação");

        Despesa despesa = new Despesa(
            "Compras do mercado",
            DateTime.Today,
            150.00m,
            FormaPagamento.Debito,
            [categoria]
        );

        // Ação
        repositorio.Cadastrar(despesa);
        dbContext.ChangeTracker.Clear();

        Despesa? despesaSelecionada =
            repositorio.SelecionarPorId(despesa.Id);

        // Asserção
        Assert.IsNotNull(despesaSelecionada);
        Assert.AreEqual("Compras do mercado", despesaSelecionada.Descricao);
        Assert.AreEqual(150.00m, despesaSelecionada.Valor);
        Assert.AreEqual(FormaPagamento.Debito, despesaSelecionada.FormaPagamento);
        Assert.HasCount(1, despesaSelecionada.Categorias);
    }

    [TestMethod]
    public void Editar_AtualizaDespesaExistente()
    {
        // Arranjo
        Categoria categoria = new Categoria("Alimentação");

        Despesa despesa = new Despesa(
            "Compras do mercado",
            DateTime.Today,
            150.00m,
            FormaPagamento.Debito,
            [categoria]
        );

        repositorio.Cadastrar(despesa);

        Despesa despesaAtualizada = new Despesa(
            "Compras atualizadas",
            DateTime.Today,
            200.00m,
            FormaPagamento.Credito,
            [categoria]
        );

        // Ação
        bool conseguiuEditar =
            repositorio.Editar(despesa.Id, despesaAtualizada);

        dbContext.ChangeTracker.Clear();

        Despesa? despesaSelecionada =
            repositorio.SelecionarPorId(despesa.Id);

        // Asserção
        Assert.IsTrue(conseguiuEditar);
        Assert.IsNotNull(despesaSelecionada);
        Assert.AreEqual("Compras atualizadas", despesaSelecionada.Descricao);
        Assert.AreEqual(200.00m, despesaSelecionada.Valor);
        Assert.AreEqual(FormaPagamento.Credito, despesaSelecionada.FormaPagamento);
    }

    [TestMethod]
    public void Excluir_RemoveDespesaExistente()
    {
        // Arranjo
        Categoria categoria = new Categoria("Alimentação");

        Despesa despesa = new Despesa(
            "Compras do mercado",
            DateTime.Today,
            150.00m,
            FormaPagamento.Debito,
            [categoria]
        );

        repositorio.Cadastrar(despesa);

        // Ação
        bool conseguiuExcluir = repositorio.Excluir(despesa.Id);

        dbContext.ChangeTracker.Clear();

        Despesa? despesaSelecionada =
            repositorio.SelecionarPorId(despesa.Id);

        // Asserção
        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(despesaSelecionada);
    }

    [TestMethod]
    public void SelecionarTodos_RetornaDespesasCadastradas()
    {
        // Arranjo
        Categoria categoria = new Categoria("Alimentação");

        Despesa despesa1 = new Despesa(
            "Despesa 1",
            DateTime.Today,
            100.00m,
            FormaPagamento.AVista,
            [categoria]
        );

        Despesa despesa2 = new Despesa(
            "Despesa 2",
            DateTime.Today,
            200.00m,
            FormaPagamento.Credito,
            [categoria]
        );

        Despesa despesa3 = new Despesa(
            "Despesa 3",
            DateTime.Today,
            300.00m,
            FormaPagamento.Debito,
            [categoria]
        );

        repositorio.Cadastrar(despesa1);
        repositorio.Cadastrar(despesa2);
        repositorio.Cadastrar(despesa3);

        dbContext.ChangeTracker.Clear();

        // Ação
        List<Despesa> despesas = repositorio.SelecionarTodos();

        // Asserção
        Assert.HasCount(3, despesas);
    }
}