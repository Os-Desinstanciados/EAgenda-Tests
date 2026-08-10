using eAgenda.Aplicacao.Modulos.ModuloContato;
using eAgenda.Dominio.Modulos.ModuloCompromisso;
using eAgenda.Dominio.Modulos.ModuloContato;
using FluentResults;
using Moq;

namespace eAgenda.Testes.Unidade.Modulos.ModuloContato;

[TestClass]
public sealed class ServicoContatoTests
{
    [TestMethod]
    public void Cadastrar_DadosValidosPersisteContato()
    {
        // Arranjo
        Mock<IRepositorioContato> repositorioContato = new Mock<IRepositorioContato>();
        Mock<IRepositorioCompromisso> repositorioCompromisso = new Mock<IRepositorioCompromisso>();

        repositorioContato.Setup(r => r.SelecionarTodos()).Returns([]);

        Contato? contatoCadastrado = null;

        repositorioContato
            .Setup(r => r.Cadastrar(It.IsAny<Contato>()))
            .Callback<Contato>(contato => contatoCadastrado = contato);

        ServicoContato servicoContato = new ServicoContato(
            repositorioContato.Object,
            repositorioCompromisso.Object
        );

        // Ação
        Result resultado = servicoContato.Cadastrar(
            new CadastrarContatoDto(
            "Contato Teste",
            "teste@hotmail.com",
            "(49) 9999-9999",
            "Administrador",
            "Empresa"
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(contatoCadastrado);
        Assert.AreEqual("Contato Teste", contatoCadastrado.Nome);
        Assert.AreEqual("teste@hotmail.com", contatoCadastrado.Email);
        Assert.AreEqual("(49) 9999-9999", contatoCadastrado.Telefone);
        Assert.AreEqual("Administrador", contatoCadastrado.Cargo);
        Assert.AreEqual("Empresa", contatoCadastrado.Empresa);

        repositorioContato.Verify(r => r.Cadastrar(It.IsAny<Contato>()), Times.Once);
    }

    [TestMethod]
    public void Cadastrar_EmailDuplicado_RetornaFalha()
    {
        // Arranjo
        Mock<IRepositorioContato> repositorioContato = new Mock<IRepositorioContato>();
        Mock<IRepositorioCompromisso> repositorioCompromisso = new Mock<IRepositorioCompromisso>();

        repositorioContato
            .Setup(r => r.SelecionarTodos())
            .Returns([new Contato(
                "Junior Teste",
                "teste@hotmail.com",
                "(49) 9999-4444",
                "Administrador",
                "Empresa"
            )]);

        ServicoContato servicoContato = new ServicoContato(
            repositorioContato.Object,
            repositorioCompromisso.Object
        );

        // Ação
        Result resultado = servicoContato.Cadastrar(
            new CadastrarContatoDto(
                "Junior Teste",
                "teste@hotmail.com",
                "(49) 9999-9999",
                "Administrador",
                "Empresa"
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual("Email", resultado.Errors.Single().Metadata["Campo"]);
        Assert.Contains("Já existe", resultado.Errors.Single().Message);

        repositorioContato.Verify(r => r.Cadastrar(It.IsAny<Contato>()), Times.Never);
    }
}