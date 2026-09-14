using eAgenda.Aplicacao.Modulos.ModuloTarefa;
using eAgenda.Dominio.Modulos.ModuloTarefa;
using FluentResults;
using Moq;

namespace eAgenda.Testes.Unidade.Modulos.ModuloTarefa;

[TestClass]
public sealed class ServicoTarefaTests
{
    [TestMethod]
    public void Cadastrar_TarefaValida_PersisteTarefa()
    {
        //Arranjo
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        repositorioTarefa.Setup(r => r.SelecionarTodos()).Returns([]);

        Tarefa? tarefaCadastrada = null;

        repositorioTarefa
            .Setup(r => r.Cadastrar(It.IsAny<Tarefa>()))
            .Callback<Tarefa>(tarefa => tarefaCadastrada = tarefa);

        ServicoTarefa servicoTarefa = new ServicoTarefa(
            repositorioTarefa.Object
        );

        //Ação
        Result resultado = servicoTarefa.Cadastrar(
            new CadastrarTarefaDto(
                "Estudar Testes Unitarios",
                PrioridadeTarefa.Normal
            )
        );

        //Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(tarefaCadastrada);
        Assert.AreEqual("Estudar Testes Unitarios", tarefaCadastrada.Titulo);
        Assert.AreEqual(PrioridadeTarefa.Normal, tarefaCadastrada.Prioridade);
    }

    [TestMethod]
    public void Editar_ComTarefaVazia_RetornaFalha()
    {
        //Arranjo
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        Guid tarefaId = Guid.NewGuid();

        repositorioTarefa
            .Setup(r => r.SelecionarPorId(tarefaId))
            .Returns((Tarefa?)null);

        ServicoTarefa servicoTarefa = new ServicoTarefa(
            repositorioTarefa.Object
        );

        // Ação
        Result resultado = servicoTarefa.Editar(
            new EditarTarefaDto(
                tarefaId,
                "Estudar Testes Unitários",
                PrioridadeTarefa.Normal
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.Contains("Tarefa não encontrada", resultado.Errors.Single().Message);

        repositorioTarefa.Verify(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Tarefa>()), Times.Never);
    }

    [TestMethod]
    public void Excluir_ComTarefaVazia_RetornaFalha()
    {
        //Arranjo
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        Guid tarefaId = Guid.NewGuid();

        repositorioTarefa
            .Setup(r => r.SelecionarPorId(tarefaId))
            .Returns((Tarefa?)null);

        ServicoTarefa servicoTarefa = new ServicoTarefa(
            repositorioTarefa.Object
        );

        // Ação
        Result resultado = servicoTarefa.Excluir(tarefaId);

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.Contains("Tarefa não encontrada", resultado.Errors.Single().Message);

        repositorioTarefa.Verify(r => r.Excluir(It.IsAny<Guid>()), Times.Never);
    }

    [TestMethod]
    public void AdicionarItem_ComDadosValidos_AdicionaItem()
    {
        // Arranjo
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        Tarefa tarefa = new Tarefa(
            "Estudar Testes Unitários",
            PrioridadeTarefa.Normal
        );

        repositorioTarefa
            .Setup(r => r.SelecionarPorId(tarefa.Id))
            .Returns(tarefa);

        repositorioTarefa
            .Setup(r => r.Editar(tarefa.Id, It.IsAny<Tarefa>()))
            .Returns(true);

        ServicoTarefa servicoTarefa = new ServicoTarefa(
            repositorioTarefa.Object
        );

        // Ação
        Result resultado = servicoTarefa.AdicionarItem(
            new AdicionarItemTarefaDto(
                tarefa.Id,
                "Estudar Mock Tests"
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.HasCount(1, tarefa.Itens);
        Assert.AreEqual("Estudar Mock Tests", tarefa.Itens.First().Titulo);

        repositorioTarefa.Verify(r => r.Editar(tarefa.Id, tarefa), Times.Once);
    }

    [TestMethod]
    public void AdicionarItem_ComTarefaVazia_RetornaFalha()
    {
        // Arranjo
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        Guid tarefaId = Guid.NewGuid();

        repositorioTarefa
            .Setup(r => r.SelecionarPorId(tarefaId))
            .Returns((Tarefa?)null);

        ServicoTarefa servicoTarefa = new ServicoTarefa(
            repositorioTarefa.Object
        );

        // Ação
        Result resultado = servicoTarefa.AdicionarItem(
            new AdicionarItemTarefaDto(
                tarefaId,
                "Estudar Mock Tests"
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.Contains("Tarefa não encontrada", resultado.Errors.Single().Message);

        repositorioTarefa.Verify(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Tarefa>()), Times.Never);
    }

    [TestMethod]
    public void AlterarConclusaoItem_ComItemValido_ConcluiItem()
    {
        // Arranjo
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        Tarefa tarefa = new Tarefa(
            "Finalizar trabalho",
            PrioridadeTarefa.Alta
        );

        ItemTarefa item = new ItemTarefa("Revisar documentação");
        tarefa.AdicionarItem(item);

        repositorioTarefa
            .Setup(r => r.SelecionarPorId(tarefa.Id))
            .Returns(tarefa);

        repositorioTarefa
            .Setup(r => r.Editar(tarefa.Id, It.IsAny<Tarefa>()))
            .Returns(true);

        ServicoTarefa servicoTarefa = new ServicoTarefa(
            repositorioTarefa.Object
        );

        // Ação
        Result resultado = servicoTarefa.AlterarConclusaoItem(
            new AlterarConclusaoItemTarefaDto(
                tarefa.Id,
                item.Id,
                true
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsTrue(item.Concluido);

        repositorioTarefa.Verify(r => r.Editar(tarefa.Id, tarefa), Times.Once);
    }

    [TestMethod]
    public void AlterarConclusaoItem_ComItemInvalido_RetornaFalha()
    {
        // Arranjo
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        Tarefa tarefa = new Tarefa(
            "Finalizar trabalho",
            PrioridadeTarefa.Alta
        );

        Guid itemIdInvalido = Guid.NewGuid();

        repositorioTarefa
            .Setup(r => r.SelecionarPorId(tarefa.Id))
            .Returns(tarefa);

        ServicoTarefa servicoTarefa = new ServicoTarefa(
            repositorioTarefa.Object
        );

        // Ação
        Result resultado = servicoTarefa.AlterarConclusaoItem(
            new AlterarConclusaoItemTarefaDto(
                tarefa.Id,
                itemIdInvalido,
                true
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.Contains("Item de tarefa não encontrado", resultado.Errors.Single().Message);

        repositorioTarefa.Verify(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Tarefa>()), Times.Never);
    }

    [TestMethod]
    public void RemoverItem_ComItemValido_RemoveItem()
    {
        // Arranjo
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        Tarefa tarefa = new Tarefa(
            "Organizar projeto",
            PrioridadeTarefa.Normal
        );

        ItemTarefa item = new ItemTarefa("Remover arquivos antigos");
        tarefa.AdicionarItem(item);

        repositorioTarefa
            .Setup(r => r.SelecionarPorId(tarefa.Id))
            .Returns(tarefa);

        repositorioTarefa
            .Setup(r => r.Editar(tarefa.Id, It.IsAny<Tarefa>()))
            .Returns(true);

        ServicoTarefa servicoTarefa = new ServicoTarefa(
            repositorioTarefa.Object
        );

        // Ação
        Result resultado = servicoTarefa.RemoverItem(
            new RemoverItemTarefaDto(
                tarefa.Id,
                item.Id
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.HasCount(0, tarefa.Itens);

        repositorioTarefa.Verify(r => r.Editar(tarefa.Id, tarefa), Times.Once);
    }

    [TestMethod]
    public void RemoverItem_ComItemInexistente_RetornaFalha()
    {
        // Arranjo
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        Tarefa tarefa = new Tarefa(
            "Organizar projeto",
            PrioridadeTarefa.Normal
        );

        Guid itemIdInexistente = Guid.NewGuid();

        repositorioTarefa
            .Setup(r => r.SelecionarPorId(tarefa.Id))
            .Returns(tarefa);

        ServicoTarefa servicoTarefa = new ServicoTarefa(
            repositorioTarefa.Object
        );

        // Ação
        Result resultado = servicoTarefa.RemoverItem(
            new RemoverItemTarefaDto(
                tarefa.Id,
                itemIdInexistente
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.Contains("Item de tarefa não encontrado", resultado.Errors.Single().Message);

        repositorioTarefa.Verify(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Tarefa>()), Times.Never);
    }

    [TestMethod]
    public void AlterarConclusao_TarefaSemItens_AlteraConclusao()
    {
        // Arranjo
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        Tarefa tarefa = new Tarefa(
            "Organizar documentos",
            PrioridadeTarefa.Normal
        );

        repositorioTarefa
            .Setup(r => r.SelecionarPorId(tarefa.Id))
            .Returns(tarefa);

        repositorioTarefa
            .Setup(r => r.Editar(tarefa.Id, It.IsAny<Tarefa>()))
            .Returns(true);

        ServicoTarefa servicoTarefa = new ServicoTarefa(
            repositorioTarefa.Object
        );

        // Ação
        Result resultado = servicoTarefa.AlterarConclusao(
            new AlterarConclusaoTarefaDto(
                tarefa.Id,
                true
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsTrue(tarefa.Concluida);

        repositorioTarefa.Verify(r => r.Editar(tarefa.Id, tarefa), Times.Once);
    }

    [TestMethod]
    public void AlterarConclusao_TarefaComItens_RetornaFalha()
    {
        // Arranjo
        Mock<IRepositorioTarefa> repositorioTarefa = new();

        Tarefa tarefa = new Tarefa(
            "Organizar documentos",
            PrioridadeTarefa.Normal
        );

        ItemTarefa item = new ItemTarefa("Revisar documentos");
        tarefa.AdicionarItem(item);

        repositorioTarefa
            .Setup(r => r.SelecionarPorId(tarefa.Id))
            .Returns(tarefa);

        ServicoTarefa servicoTarefa = new ServicoTarefa(
            repositorioTarefa.Object
        );

        // Ação
        Result resultado = servicoTarefa.AlterarConclusao(
            new AlterarConclusaoTarefaDto(
                tarefa.Id,
                true
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.Contains("A conclusão desta tarefa deve ser controlada pelos itens cadastrados.", resultado.Errors.Single().Message);

        repositorioTarefa.Verify(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Tarefa>()), Times.Never);
    }
}