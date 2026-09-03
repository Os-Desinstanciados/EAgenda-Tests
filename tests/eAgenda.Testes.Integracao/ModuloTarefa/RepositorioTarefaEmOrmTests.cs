using eAgenda.Dominio.Modulos.ModuloTarefa;
using eAgenda.Infra.Compartilhado.Orm;
using eAgenda.Infra.Modulos.ModuloTarefa;
using eAgenda.Testes.Integracao.Compartilhado.Orm;

namespace eAgenda.Testes.Integracao.ModuloTarefa;

[TestClass]
public sealed class RepositorioTarefaEmOrmTests : RepositorioEmOrmBaseTests
{
    private EAgendaDbContext dbContext = null!;
    private RepositorioTarefaEmOrm repositorio = null!;

    [TestInitialize]
    public void InicializarRepositorio()
    {
        dbContext = CriarDbContext();

        repositorio = new RepositorioTarefaEmOrm(dbContext);
    }

    [TestMethod]
    public void CadastrarESelecionarPorId_CarregaRelacionamentosDaTarefa()
    {
        // Arranjo
        Tarefa tarefa = new Tarefa(
            "Estudar Testes",
            PrioridadeTarefa.Normal
        );

        // Ação
        repositorio.Cadastrar(tarefa);
        dbContext.ChangeTracker.Clear();

        Tarefa? tarefaSelecionada = repositorio.SelecionarPorId(tarefa.Id);

        // Asserção
        Assert.IsNotNull(tarefaSelecionada);
        Assert.AreEqual("Estudar Testes", tarefaSelecionada.Titulo);
        Assert.AreEqual(PrioridadeTarefa.Normal, tarefaSelecionada.Prioridade);
    }

    [TestMethod]
    public void Editar_AtualizaTarefaExistente()
    {
        // Arranjo
        Tarefa tarefa = new Tarefa(
            "Tarefa Original",
            PrioridadeTarefa.Normal
        );

        repositorio.Cadastrar(tarefa);

        Tarefa tarefaAtualizada = new Tarefa(
            "Tarefa Atualizada",
            PrioridadeTarefa.Alta
        );

        // Ação
        bool conseguiuEditar = repositorio.Editar(tarefa.Id, tarefaAtualizada);

        dbContext.ChangeTracker.Clear();

        Tarefa? tarefaSelecionada = repositorio.SelecionarPorId(tarefa.Id);

        // Asserção
        Assert.IsTrue(conseguiuEditar);
        Assert.IsNotNull(tarefaSelecionada);
        Assert.AreEqual("Tarefa Atualizada", tarefaSelecionada.Titulo);
        Assert.AreEqual(PrioridadeTarefa.Alta, tarefaSelecionada.Prioridade);
    }

    [TestMethod]
    public void Excluir_RemoveTarefaExistente()
    {
        // Arranjo
        Tarefa tarefa = new Tarefa(
            "Tarefa Teste",
            PrioridadeTarefa.Normal
        );

        repositorio.Cadastrar(tarefa);

        // Ação
        bool conseguiuExcluir = repositorio.Excluir(tarefa.Id);

        dbContext.ChangeTracker.Clear();

        Tarefa? tarefaSelecionada = repositorio.SelecionarPorId(tarefa.Id);

        // Asserção
        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(tarefaSelecionada);
    }

    [TestMethod]
    public void SelecionarTodos_RetornaTarefasCadastradas()
    {
        // Arranjo
        Tarefa tarefa1 = new Tarefa(
            "Tarefa 1",
            PrioridadeTarefa.Baixa
        );

        Tarefa tarefa2 = new Tarefa(
            "Tarefa 2",
            PrioridadeTarefa.Normal
        );

        Tarefa tarefa3 = new Tarefa(
            "Tarefa 3",
            PrioridadeTarefa.Alta
        );

        repositorio.Cadastrar(tarefa1);
        repositorio.Cadastrar(tarefa2);
        repositorio.Cadastrar(tarefa3);

        dbContext.ChangeTracker.Clear();

        // Ação
        List<Tarefa> tarefas = repositorio.SelecionarTodos();

        // Asserção
        Assert.HasCount(3, tarefas);
    }
}