using eAgenda.Dominio.Modulos.ModuloContato;
using eAgenda.Infra.Modulos.ModuloContato;
using eAgenda.Testes.Integracao.Compartilhado.Orm;
using FizzWare.NBuilder;

namespace eAgenda.Testes.Integracao.ModuloContato;

[TestClass]
public sealed class RepositorioContatoEmOrmTests : RepositorioEmOrmBaseTests
{
    [TestMethod]
    public void CadastrarESelecionarPorId_CarregaRelacionamentosDoContato()
    {
        // Arranjo
        Contato contato = Builder<Contato>
            .CreateNew()
            .Build();

        RepositorioContatoEmOrm repositorio = new RepositorioContatoEmOrm(dbContext);

        // Ação
        repositorio.Cadastrar(contato);
        dbContext.ChangeTracker.Clear();

        Contato? contatoSelecionado = repositorio.SelecionarPorId(contato.Id);

        //Asserção
        Assert.IsNotNull(contatoSelecionado);
        Assert.AreEqual("Nome1", contatoSelecionado.Nome);
        Assert.AreEqual("Email1", contatoSelecionado.Email);
        Assert.AreEqual("Telefone1", contatoSelecionado.Telefone);
        Assert.AreEqual("Cargo1", contatoSelecionado.Cargo);
        Assert.AreEqual("Empresa1", contatoSelecionado.Empresa);
    }

    [TestMethod]
    public void Editar_AtualizaContatoExistente()
    {
        // Arranjo
        Contato contato = Builder<Contato>
            .CreateNew()
            .Persist();

        Contato contatoAtualizado = Builder<Contato>
            .CreateNew()
            .With(c => c.Nome = "ContatoAtualizado")
            .Build();

        RepositorioContatoEmOrm repositorio = new RepositorioContatoEmOrm(dbContext);

        // Ação
        bool conseguiuEditar = repositorioContato.Editar(contato.Id, contatoAtualizado);
        dbContext.ChangeTracker.Clear();

        Contato? contatoSelecionado = repositorioContato.SelecionarPorId(contato.Id);

        // Asserção
        Assert.IsTrue(conseguiuEditar);
        Assert.IsNotNull(contatoSelecionado);
        Assert.AreEqual("ContatoAtualizado", contatoAtualizado.Nome);
    }

    [TestMethod]
    public void Excluir_RemoveContatoExistente()
    {
        // Arranjo
        Contato contato = Builder<Contato>
            .CreateNew()
            .Persist();

        // Ação
        bool conseguiuExcluir = repositorioContato.Excluir(contato.Id);
        dbContext.ChangeTracker.Clear();

        Contato? contatoSelecionado = repositorioContato.SelecionarPorId(contato.Id);

        // Asserção
        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(contatoSelecionado);
    }

    [TestMethod]
    public void SelecionarTodos_RetornaContatosCadastrados()
    {
        // Arranjo // Ação
        IList<Contato> contato = Builder<Contato>
            .CreateListOfSize(3)
            .Persist();

        dbContext.ChangeTracker.Clear();
        
        // Asserção
        Assert.HasCount(3, repositorioContato.SelecionarTodos());
    }
}