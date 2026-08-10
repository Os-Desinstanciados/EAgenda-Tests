using eAgenda.Dominio.Modulos.ModuloCompromisso;
using eAgenda.Infra.Modulos.ModuloCompromisso;
using eAgenda.Testes.Integracao.Compartilhado.Orm;

namespace eAgenda.Testes.Integracao.ModuloCompromisso;

[TestClass]
public sealed class RepositorioCompromissoEmOrmTests : RepositorioEmOrmBaseTests
{
    [TestMethod]
    public void CadastrarESelecionarPorId_CarregaRelacionamentosDoCompromisso()
    {
        // Arranjo
        Compromisso compromisso = CriarComprimissoValido();

        RepositorioCompromissoEmOrm repositorio = new RepositorioCompromissoEmOrm(dbContext);

        // Ação
        repositorio.Cadastrar(compromisso);
        dbContext.ChangeTracker.Clear();

        //Asserção
        Assert.IsNotNull(compromisso);
        Assert.AreEqual("Show", compromisso.Assunto);
        Assert.AreEqual(new DateTime(2023, 03, 08), compromisso.DataOcorrencia);
        Assert.AreEqual(TimeSpan.FromHours(20), compromisso.HoraInicio);
        Assert.AreEqual(TimeSpan.FromHours(23), compromisso.HoraTermino);
        Assert.AreEqual(TipoCompromisso.Presencial, compromisso.Tipo);
        Assert.AreEqual("Belo Horizonte", compromisso.Local);
        Assert.AreEqual("instagram.com/davidguetta", compromisso.Link);
        Assert.IsNull(compromisso.Contato);
    }

    [TestMethod]
    public void Editar_AtualizaCompromissoExistente()
    {
        // Arranjo
        Compromisso compromisso = CriarComprimissoValido();

        RepositorioCompromissoEmOrm repositorio = new RepositorioCompromissoEmOrm(dbContext);

        repositorio.Cadastrar(compromisso);

        Compromisso compromissoAtualizado = new Compromisso(
            "Teatro",
            new DateTime(2023, 03, 08),
            TimeSpan.FromHours(16),
            TimeSpan.FromHours(19),
            TipoCompromisso.Presencial,
            "Teatro Marajoara",
            "instagram.com/teatromarajuara",
            null
        );

        // Ação
        bool conseguiuEditar = repositorio.Editar(compromisso.Id, compromissoAtualizado);
        dbContext.ChangeTracker.Clear();

        // Asserção
        Assert.IsTrue(conseguiuEditar);
        Assert.AreEqual("Teatro",
            repositorio.SelecionarPorId(compromisso.Id)!.Assunto
        );
    }

    [TestMethod]
    public void Excluir_RemoveCompromissoExistente()
    {
        // Arranjo
        Compromisso compromisso = CriarComprimissoValido();

        RepositorioCompromissoEmOrm repositorio = new RepositorioCompromissoEmOrm(dbContext);

        repositorio.Cadastrar(compromisso);

        // Ação
        bool conseguiuExcluir = repositorio.Excluir(compromisso.Id);
        dbContext.ChangeTracker.Clear();

        // Asserção
        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(repositorio.SelecionarPorId(compromisso.Id));
    }

    [TestMethod]
    public void SelecionarTodos_RetornaCompromissosCadastrados()
    {
        // Arranjo
        Compromisso compromisso = CriarComprimissoValido();

        RepositorioCompromissoEmOrm repositorio = new RepositorioCompromissoEmOrm(dbContext);

        repositorio.Cadastrar(compromisso);

        // Ação
        List<Compromisso> compromissos = repositorio.SelecionarTodos();
        dbContext.ChangeTracker.Clear();

        // Asserção
        Assert.IsNotEmpty(compromissos);
    }

    private Compromisso CriarComprimissoValido()
    {
        Compromisso compromisso = new Compromisso(
            "Show",
            new DateTime(2023, 03, 08),
            TimeSpan.FromHours(20),
            TimeSpan.FromHours(23),
            TipoCompromisso.Presencial,
            "Belo Horizonte",
            "instagram.com/davidguetta",
            null
        );

        return compromisso;
    }
}