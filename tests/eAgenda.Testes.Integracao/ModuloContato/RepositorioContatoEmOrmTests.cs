using eAgenda.Dominio.Modulos.ModuloContato;
using eAgenda.Infra.Compartilhado.Orm;
using eAgenda.Infra.Modulos.ModuloContato;
using eAgenda.Testes.Integracao.Compartilhado.Orm;

namespace eAgenda.Testes.Integracao.ModuloContato;

[TestClass]
public sealed class RepositorioContatoEmOrmTests : RepositorioEmOrmBaseTests
{
    private EAgendaDbContext dbContext = null!;
    private RepositorioContatoEmOrm repositorio = null!;

    [TestInitialize]
    public void InicializarRepositorio()
    {
        dbContext = CriarDbContext();

        repositorio = new RepositorioContatoEmOrm(dbContext);
    }

    [TestMethod]
    public void CadastrarESelecionarPorId_CarregaRelacionamentosDoContato()
    {
        // Arranjo
        Contato contato = new Contato(
            "Contato Teste",
            "contato@teste.com",
            "(47) 99999-9999",
            "Desenvolvedor",
            "Academia do Programador"
        );

        // Ação
        repositorio.Cadastrar(contato);
        dbContext.ChangeTracker.Clear();

        Contato? contatoSelecionado = repositorio.SelecionarPorId(contato.Id);

        // Asserção
        Assert.IsNotNull(contatoSelecionado);
        Assert.AreEqual("Contato Teste", contatoSelecionado.Nome);
        Assert.AreEqual("contato@teste.com", contatoSelecionado.Email);
        Assert.AreEqual("(47) 99999-9999", contatoSelecionado.Telefone);
        Assert.AreEqual("Desenvolvedor", contatoSelecionado.Cargo);
        Assert.AreEqual("Academia do Programador", contatoSelecionado.Empresa);
    }

    [TestMethod]
    public void Editar_AtualizaContatoExistente()
    {
        // Arranjo
        Contato contato = new Contato(
            "Contato Teste",
            "contato@teste.com",
            "(47) 99999-9999",
            "Desenvolvedor",
            "Academia do Programador"
        );

        repositorio.Cadastrar(contato);

        Contato contatoAtualizado = new Contato(
            "Contato Atualizado",
            "atualizado@teste.com",
            "(47) 98888-8888",
            "Analista",
            "Empresa Atualizada"
        );

        // Ação
        bool conseguiuEditar = repositorio.Editar(contato.Id, contatoAtualizado);

        dbContext.ChangeTracker.Clear();

        Contato? contatoSelecionado = repositorio.SelecionarPorId(contato.Id);

        // Asserção
        Assert.IsTrue(conseguiuEditar);
        Assert.IsNotNull(contatoSelecionado);
        Assert.AreEqual("Contato Atualizado", contatoSelecionado.Nome);
    }

    [TestMethod]
    public void Excluir_RemoveContatoExistente()
    {
        // Arranjo
        Contato contato = new Contato(
            "Contato Teste",
            "contato@teste.com",
            "(47) 99999-9999",
            "Desenvolvedor",
            "Academia do Programador"
        );

        repositorio.Cadastrar(contato);

        // Ação
        bool conseguiuExcluir = repositorio.Excluir(contato.Id);

        dbContext.ChangeTracker.Clear();

        Contato? contatoSelecionado = repositorio.SelecionarPorId(contato.Id);

        // Asserção
        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(contatoSelecionado);
    }

    [TestMethod]
    public void SelecionarTodos_RetornaContatosCadastrados()
    {
        // Arranjo
        Contato contato1 = new Contato(
            "Contato 1",
            "contato1@teste.com",
            "(47) 99999-1111",
            "Cargo 1",
            "Empresa 1"
        );

        Contato contato2 = new Contato(
            "Contato 2",
            "contato2@teste.com",
            "(47) 99999-2222",
            "Cargo 2",
            "Empresa 2"
        );

        Contato contato3 = new Contato(
            "Contato 3",
            "contato3@teste.com",
            "(47) 99999-3333",
            "Cargo 3",
            "Empresa 3"
        );

        repositorio.Cadastrar(contato1);
        repositorio.Cadastrar(contato2);
        repositorio.Cadastrar(contato3);

        dbContext.ChangeTracker.Clear();

        // Ação
        IList<Contato> contatos = repositorio.SelecionarTodos();

        // Asserção
        Assert.HasCount(3, contatos);
    }
}