using eAgenda.Dominio.Modulos.ModuloContato;
using eAgenda.Infra.Compartilhado.Orm;
using eAgenda.Infra.Modulos.ModuloContato;
using eAgenda.Testes.Integracao.Compartilhado.Orm;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.Testes.Integracao.ModuloContato;

[TestClass]
public sealed class RepositorioContatoEmOrmTests : RepositorioEmOrmBaseTests
{
    [TestMethod]
    public void CadastrarESelecionarPorId_CarregaRelacionamentosDoContato()
    {
        // Arranjo
        Contato contato = CriaContatoValido();
        
        RepositorioContatoEmOrm repositorio = new RepositorioContatoEmOrm(dbContext);

        // Ação
        repositorio.Cadastrar(contato);
        dbContext.ChangeTracker.Clear();

        Contato? contatoSelecionado = repositorio.SelecionarPorId(contato.Id);

        //Asserção
        Assert.IsNotNull(contatoSelecionado);
        Assert.AreEqual("Contato Teste", contatoSelecionado.Nome);
        Assert.AreEqual("teste@hotmail.com", contatoSelecionado.Email);
        Assert.AreEqual("(49) 9999-9999", contatoSelecionado.Telefone);
        Assert.AreEqual("Administrador", contatoSelecionado.Cargo);
        Assert.AreEqual("Empresa", contatoSelecionado.Empresa);

    }

    [TestMethod]
    public void Editar_AtualizaContatoExistente()
    {
        // Arranjo
        Contato contato = CriaContatoValido();

        RepositorioContatoEmOrm repositorio = new RepositorioContatoEmOrm(dbContext);

        repositorio.Cadastrar(contato);
        
        Contato contatoAtualizado = new Contato(
            "Contato Editado",
            "testeEditar@hotmail.com",
            "(49) 9999-4444",
            "Assistente",
            "Mercado"
        );

        // Ação
        bool conseguiuEditar = repositorio.Editar(contato.Id, contatoAtualizado);
        dbContext.ChangeTracker.Clear();

        // Asserção
        Assert.IsTrue(conseguiuEditar);
        Assert.AreEqual(
            "Contato Editado",
            repositorio.SelecionarPorId(contato.Id)!.Nome
        );
    }

    [TestMethod]
    public void Excluir_RemoveContatoExistente()
    {
        // Arranjo
        Contato contato = CriaContatoValido();

        RepositorioContatoEmOrm repositorio = new RepositorioContatoEmOrm(dbContext);

        repositorio.Cadastrar(contato);

        // Ação
        bool conseguiuExcluir = repositorio.Excluir(contato.Id);
        dbContext.ChangeTracker.Clear();

        // Asserção
        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(repositorio.SelecionarPorId(contato.Id));
    }

    [TestMethod]
    public void SelecionarTodos_RetornaContatosCadastrados()
    {
        // Arranjo
        Contato contato = CriaContatoValido();

        RepositorioContatoEmOrm repositorio = new RepositorioContatoEmOrm(dbContext);

        repositorio.Cadastrar(contato);

        // Ação
        List<Contato> contatos = repositorio.SelecionarTodos();
        dbContext.ChangeTracker.Clear();

        // Asserção
        Assert.IsNotEmpty(contatos);
    }

    private Contato CriaContatoValido()
    {
        Contato contato = new Contato(
            "Contato Teste",
            "teste@hotmail.com",
            "(49) 9999-9999",
            "Administrador",
            "Empresa"
        );

        return contato;
    }

}