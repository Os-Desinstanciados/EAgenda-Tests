using eAgenda.Dominio.Modulos.ModuloTarefa;

namespace eAgenda.Testes.Unidade.Modulos.ModuloTarefa;

[TestClass]
public sealed class TarefaTests
{
    [TestMethod]
    public void Validar_ComAssuntoVazio_DeveRetornaErros()
    {
        //Arranjo [Configura dados do teste]
        Tarefa tarefa = new Tarefa(
            string.Empty,
            PrioridadeTarefa.Normal
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = tarefa.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Título\" deve ser preenchudo.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_ComAssuntoCurto_DeveRetornaErros()
    {
        //Arranjo [Configura dados do teste]
        Tarefa tarefa = new Tarefa(
            new string('A',1),
            PrioridadeTarefa.Normal
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = tarefa.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Título\" deve conter entre 2 e 100 caracteres.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_ComAssuntoLongo_DeveRetornaErros()
    {
        //Arranjo [Configura dados do teste]
        Tarefa tarefa = new Tarefa(
            new string('A',101),
            PrioridadeTarefa.Normal
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = tarefa.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Título\" deve conter entre 2 e 100 caracteres.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_ComPrioridadeVazia_DeveRetornaErros()
    {
        //Arranjo [Configura dados do teste]
        Tarefa tarefa = new Tarefa(
            "Estudar Testes de Unidade",
            (PrioridadeTarefa)99
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = tarefa.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Prioridade\" deve ser preenchido.",
            erros.First()
        );
    }
}