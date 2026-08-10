using eAgenda.Dominio.Modulos.ModuloCompromisso;
using eAgenda.Infra.Modulos.ModuloCompromisso;
using eAgenda.Testes.Integracao.Compartilhado.Orm;
using FizzWare.NBuilder;

namespace eAgenda.Testes.Integracao.ModuloCompromisso;

[TestClass]
public sealed class RepositorioCompromissoEmOrmTests : RepositorioEmOrmBaseTests
{
    [TestMethod]
    public void CadastrarESelecionarPorId_CarregaRelacionamentosDoCompromisso()
    {
        // Arranjo
        Compromisso compromisso = Builder<Compromisso>
            .CreateNew()
            .With(c => c.HoraInicio = TimeSpan.FromHours(20))
            .With(c => c.HoraTermino = TimeSpan.FromHours(23))
            .With(c => c.Tipo = TipoCompromisso.Presencial)
            .Build();

        RepositorioCompromissoEmOrm repositorio = new RepositorioCompromissoEmOrm(dbContext);

        // Ação
        repositorio.Cadastrar(compromisso);
        dbContext.ChangeTracker.Clear();

        Compromisso? compromissoSelecionado = repositorio.SelecionarPorId(compromisso.Id);

        //Asserção
        Assert.IsNotNull(compromisso);
        Assert.AreEqual("Assunto1", compromisso.Assunto);
        Assert.AreEqual(TimeSpan.FromHours(20), compromisso.HoraInicio);
        Assert.AreEqual(TimeSpan.FromHours(23), compromisso.HoraTermino);
        Assert.AreEqual(TipoCompromisso.Presencial, compromisso.Tipo);
        Assert.AreEqual("Local1", compromisso.Local);
        Assert.AreEqual("Link1", compromisso.Link);
        Assert.IsNull(compromisso.Contato);
    }

    [TestMethod]
    public void Editar_AtualizaCompromissoExistente()
    {
        // Arranjo
        Compromisso compromisso = Builder<Compromisso>
            .CreateNew()
            .Persist();

        Compromisso compromissoAtualizado = Builder<Compromisso>
            .CreateNew()
            .With(c => c.Assunto = "AssuntoAtualizado")
            .Build();

        RepositorioCompromissoEmOrm repositorio = new RepositorioCompromissoEmOrm(dbContext);

        // Ação
        bool conseguiuEditar = repositorioCompromisso.Editar(compromisso.Id, compromissoAtualizado);
        dbContext.ChangeTracker.Clear();

        Compromisso? compromissoSelecionado = repositorio.SelecionarPorId(compromisso.Id);

        // Asserção
        Assert.IsTrue(conseguiuEditar);
        Assert.IsNotNull(compromissoSelecionado);
        Assert.AreEqual("AssuntoAtualizado", compromissoAtualizado.Assunto);
    }

    [TestMethod]
    public void Excluir_RemoveCompromissoExistente()
    {
        // Arranjo
        Compromisso compromisso = Builder<Compromisso>
            .CreateNew()
            .Persist();

        // Ação
        bool conseguiuExcluir = repositorioCompromisso.Excluir(compromisso.Id);
        dbContext.ChangeTracker.Clear();

        Compromisso? compromissoSelecionado = repositorioCompromisso.SelecionarPorId(compromisso.Id);

        // Asserção
        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(compromissoSelecionado);
    }

    [TestMethod]
    public void SelecionarTodos_RetornaCompromissosCadastrados()
    {
        // Arranjo // Ação
        IList<Compromisso> compromisso = Builder<Compromisso>
            .CreateListOfSize(3)
            .Persist();

        dbContext.ChangeTracker.Clear();

        // Asserção
        Assert.HasCount(3, repositorioCompromisso.SelecionarTodos());
    }

}