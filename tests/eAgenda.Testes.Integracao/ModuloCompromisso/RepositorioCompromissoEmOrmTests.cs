using eAgenda.Dominio.Modulos.ModuloCompromisso;
using eAgenda.Infra.Compartilhado.Orm;
using eAgenda.Infra.Modulos.ModuloCompromisso;
using eAgenda.Testes.Integracao.Compartilhado.Orm;

namespace eAgenda.Testes.Integracao.ModuloCompromisso;

[TestClass]
public sealed class RepositorioCompromissoEmOrmTests : RepositorioEmOrmBaseTests
{
    private EAgendaDbContext dbContext = null!;
    private RepositorioCompromissoEmOrm repositorio = null!;

    [TestInitialize]
    public void InicializarRepositorio()
    {
        dbContext = CriarDbContext();

        repositorio = new RepositorioCompromissoEmOrm(dbContext);
    }

    [TestMethod]
    public void CadastrarESelecionarPorId_CarregaRelacionamentosDoCompromisso()
    {
        // Arranjo
        Compromisso compromisso = new Compromisso(
            "Compromisso Teste",
            DateTime.Today.AddDays(1),
            TimeSpan.FromHours(20),
            TimeSpan.FromHours(23),
            TipoCompromisso.Presencial,
            "Local Teste",
            null,
            null
        );

        // Ação
        repositorio.Cadastrar(compromisso);
        dbContext.ChangeTracker.Clear();

        Compromisso? compromissoSelecionado =
            repositorio.SelecionarPorId(compromisso.Id);

        // Asserção
        Assert.IsNotNull(compromissoSelecionado);
        Assert.AreEqual("Compromisso Teste", compromissoSelecionado.Assunto);
        Assert.AreEqual(TimeSpan.FromHours(20), compromissoSelecionado.HoraInicio);
        Assert.AreEqual(TimeSpan.FromHours(23), compromissoSelecionado.HoraTermino);
        Assert.AreEqual(TipoCompromisso.Presencial, compromissoSelecionado.Tipo);
        Assert.AreEqual("Local Teste", compromissoSelecionado.Local);
        Assert.IsNull(compromissoSelecionado.Link);
        Assert.IsNull(compromissoSelecionado.Contato);
    }

    [TestMethod]
    public void Editar_AtualizaCompromissoExistente()
    {
        // Arranjo
        Compromisso compromisso = new Compromisso(
            "Compromisso Teste",
            DateTime.Today.AddDays(1),
            TimeSpan.FromHours(20),
            TimeSpan.FromHours(23),
            TipoCompromisso.Presencial,
            "Local Teste",
            null,
            null
        );

        repositorio.Cadastrar(compromisso);

        Compromisso compromissoAtualizado = new Compromisso(
            "Assunto Atualizado",
            DateTime.Today.AddDays(2),
            TimeSpan.FromHours(18),
            TimeSpan.FromHours(21),
            TipoCompromisso.Presencial,
            "Local Atualizado",
            null,
            null
        );

        // Ação
        bool conseguiuEditar =
            repositorio.Editar(compromisso.Id, compromissoAtualizado);

        dbContext.ChangeTracker.Clear();

        Compromisso? compromissoSelecionado =
            repositorio.SelecionarPorId(compromisso.Id);

        // Asserção
        Assert.IsTrue(conseguiuEditar);
        Assert.IsNotNull(compromissoSelecionado);
        Assert.AreEqual("Assunto Atualizado", compromissoSelecionado.Assunto);
    }

    [TestMethod]
    public void Excluir_RemoveCompromissoExistente()
    {
        // Arranjo
        Compromisso compromisso = new Compromisso(
            "Compromisso Teste",
            DateTime.Today.AddDays(1),
            TimeSpan.FromHours(20),
            TimeSpan.FromHours(23),
            TipoCompromisso.Presencial,
            "Local Teste",
            null,
            null
        );

        repositorio.Cadastrar(compromisso);

        // Ação
        bool conseguiuExcluir = repositorio.Excluir(compromisso.Id);

        dbContext.ChangeTracker.Clear();

        Compromisso? compromissoSelecionado =
            repositorio.SelecionarPorId(compromisso.Id);

        // Asserção
        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(compromissoSelecionado);
    }

    [TestMethod]
    public void SelecionarTodos_RetornaCompromissosCadastrados()
    {
        // Arranjo
        Compromisso compromisso1 = new Compromisso(
            "Compromisso 1",
            DateTime.Today.AddDays(1),
            TimeSpan.FromHours(8),
            TimeSpan.FromHours(9),
            TipoCompromisso.Presencial,
            "Local 1",
            null,
            null
        );

        Compromisso compromisso2 = new Compromisso(
            "Compromisso 2",
            DateTime.Today.AddDays(2),
            TimeSpan.FromHours(10),
            TimeSpan.FromHours(11),
            TipoCompromisso.Presencial,
            "Local 2",
            null,
            null
        );

        Compromisso compromisso3 = new Compromisso(
            "Compromisso 3",
            DateTime.Today.AddDays(3),
            TimeSpan.FromHours(14),
            TimeSpan.FromHours(15),
            TipoCompromisso.Presencial,
            "Local 3",
            null,
            null
        );

        repositorio.Cadastrar(compromisso1);
        repositorio.Cadastrar(compromisso2);
        repositorio.Cadastrar(compromisso3);

        dbContext.ChangeTracker.Clear();

        // Ação
        List<Compromisso> compromissos = repositorio.SelecionarTodos();

        // Asserção
        Assert.HasCount(3, compromissos);
    }
}