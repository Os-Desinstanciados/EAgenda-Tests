using eAgenda.Dominio.Modulos.ModuloTarefa;
using eAgenda.Infra.Modulos.ModuloTarefa;
using eAgenda.Testes.Integracao.Compartilhado.Orm;
using FizzWare.NBuilder;

namespace eAgenda.Testes.Integracao.ModuloTarefa;

[TestClass]
public sealed class RepositorioTarefaEmOrmTests : RepositorioEmOrmBaseTests
{
    [TestMethod]
    public void CadastrarESelecionarPorId_CarregaRelacionamentosDaTarefa()
    {
        // Arranjo
        Tarefa tarefa = Builder<Tarefa>
            .CreateNew()
            .With(t => t.Titulo = "Estudar Testes")
            .With(t => t.Prioridade = PrioridadeTarefa.Normal)
            .Build();

        RepositorioTarefaEmOrm repositorio = new RepositorioTarefaEmOrm(dbContext);

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
        Tarefa tarefa = Builder<Tarefa>
            .CreateNew()
            .Persist();

        Tarefa tarefaAtualizada = Builder<Tarefa>
            .CreateNew()
            .With(t => t.Titulo = "Tarefa Atualizada")
            .With(t => t.Prioridade = PrioridadeTarefa.Alta)
            .Build();

        // Ação
        bool conseguiuEditar = repositorioTarefa.Editar(tarefa.Id, tarefaAtualizada);
        dbContext.ChangeTracker.Clear();

        Tarefa? tarefaSelecionada = repositorioTarefa.SelecionarPorId(tarefa.Id);

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
        Tarefa tarefa = Builder<Tarefa>
            .CreateNew()
            .Persist();

        // Ação
        bool conseguiuExcluir = repositorioTarefa.Excluir(tarefa.Id);
        dbContext.ChangeTracker.Clear();

        Tarefa? tarefaSelecionada = repositorioTarefa.SelecionarPorId(tarefa.Id);

        // Asserção
        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(tarefaSelecionada);
    }

    [TestMethod]
    public void SelecionarTodos_RetornaTarefasCadastradas()
    {
        // Arranjo // Ação
        IList<Tarefa> tarefas = Builder<Tarefa>
            .CreateListOfSize(3)
            .Persist();

        dbContext.ChangeTracker.Clear();

        // Asserção
        Assert.HasCount(3, repositorioTarefa.SelecionarTodos());
    }
}