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

    [TestMethod]
    public void Cadastrar_TelefoneDuplicado_RetornaFalha()
    {
        // Arranjo
        Mock<IRepositorioContato> repositorioContato = new Mock<IRepositorioContato>();
        Mock<IRepositorioCompromisso> repositorioCompromisso = new Mock<IRepositorioCompromisso>();

        repositorioContato
            .Setup(r => r.SelecionarTodos())
            .Returns([new Contato(
                "Junior Teste",
                "teste@hotmail.com",
                "(49) 9999-9999",
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
                "teste1@hotmail.com",
                "(49) 9999-9999",
                "Administrador",
                "Empresa"
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual("Telefone", resultado.Errors.Single().Metadata["Campo"]);
        Assert.Contains("Já existe", resultado.Errors.Single().Message);
        repositorioContato.Verify(r => r.Cadastrar(It.IsAny<Contato>()), Times.Never);
    }

    [TestMethod]
    public void Editar_DadosValidos_EditaContato()
    {
        // Arranjo
        Mock<IRepositorioContato> repositorioContato = new();
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();

        repositorioContato
            .Setup(r => r.SelecionarTodos())
            .Returns([]);

        Contato? contatoEditado = null;

        repositorioContato
            .Setup(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Contato>()))
            .Callback<Guid, Contato>((id, contato) => contatoEditado = contato)
            .Returns(true);

        ServicoContato servicoContato = new ServicoContato(
            repositorioContato.Object,
            repositorioCompromisso.Object
        );

        Guid contatoId = Guid.NewGuid();

        // Ação
        Result resultado = servicoContato.Editar(
            new EditarContatoDto(
                contatoId,
                "Contato Editado",
                "editado@hotmail.com",
                "(49) 9999-4444",
                "Assistente",
                "Mercado"
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(contatoEditado);
        Assert.AreEqual("Contato Editado", contatoEditado.Nome);
        Assert.AreEqual("editado@hotmail.com", contatoEditado.Email);
        Assert.AreEqual("(49) 9999-4444", contatoEditado.Telefone);
        Assert.AreEqual("Assistente", contatoEditado.Cargo);
        Assert.AreEqual("Mercado", contatoEditado.Empresa);
        repositorioContato.Verify(r => r.Editar(contatoId, It.IsAny<Contato>()), Times.Once);
    }

    [TestMethod]
    public void Editar_EmailDuplicado_RetornaFalha()
    {
        // Arranjo
        Mock<IRepositorioContato> repositorioContato = new();
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();

        Guid contatoId = Guid.NewGuid();

        repositorioContato
            .Setup(r => r.SelecionarTodos())
            .Returns([
                new Contato(
                "Outro Contato",
                "duplicado@hotmail.com",
                "(49) 9999-4444",
                "Administrador",
                "Empresa"
            )
            ]);

        ServicoContato servicoContato = new ServicoContato(
            repositorioContato.Object,
            repositorioCompromisso.Object
        );

        // Ação
        Result resultado = servicoContato.Editar(
            new EditarContatoDto(
                contatoId,
                "Contato Editado",
                "duplicado@hotmail.com",
                "(49) 9999-9999",
                "Assistente",
                "Empresa"
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual("Email", resultado.Errors.Single().Metadata["Campo"]);
        Assert.Contains("Já existe", resultado.Errors.Single().Message);
        repositorioContato.Verify(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Contato>()), Times.Never);
    }

    [TestMethod]
    public void Editar_TelefoneDuplicado_RetornaFalha()
    {
        // Arranjo
        Mock<IRepositorioContato> repositorioContato = new();
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();

        Guid contatoId = Guid.NewGuid();

        repositorioContato
            .Setup(r => r.SelecionarTodos())
            .Returns([
                new Contato(
                "Outro Contato",
                "outro@hotmail.com",
                "(49) 9999-4444",
                "Administrador",
                "Empresa"
            )
            ]);

        ServicoContato servicoContato = new ServicoContato(
            repositorioContato.Object,
            repositorioCompromisso.Object
        );

        // Ação
        Result resultado = servicoContato.Editar(
            new EditarContatoDto(
                contatoId,
                "Contato Editado",
                "editado@hotmail.com",
                "(49) 9999-4444",
                "Assistente",
                "Empresa"
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual("Telefone", resultado.Errors.Single().Metadata["Campo"]);
        Assert.Contains("Já existe", resultado.Errors.Single().Message);
        repositorioContato.Verify(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Contato>()), Times.Never);
    }

    [TestMethod]
    public void Excluir_ContatoSemCompromissosVinculados_ExcluiContato()
    {
        // Arranjo
        Contato contato = new Contato(
            "Junior Teste",
            "teste@hotmail.com",
            "(49) 9999-4444",
            "Administrador",
            "Empresa"
        );

        Mock<IRepositorioContato> repositorioContato = new();
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();

        repositorioContato
            .Setup(c => c.SelecionarPorId(contato.Id))
            .Returns(contato);

        repositorioCompromisso
            .Setup(c => c.SelecionarTodos())
            .Returns([]);

        ServicoContato servicoContato = new ServicoContato(
            repositorioContato.Object,
            repositorioCompromisso.Object
        );

        // Ação
        Result resultado = servicoContato.Excluir(contato.Id);

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        repositorioContato.Verify(r => r.Excluir(contato.Id), Times.Once);
    }

    [TestMethod]
    public void Excluir_ContatoComCompromissosVinculados_RetornaFalha()
    {
        // Arranjo
        Contato contato = new Contato(
            "Junior Teste",
            "teste@hotmail.com",
            "(49) 9999-4444",
            "Administrador",
            "Empresa"
        );

        Mock<IRepositorioContato> repositorioContato = new();
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();

        repositorioContato
            .Setup(c => c.SelecionarPorId(contato.Id))
            .Returns(contato);

        repositorioCompromisso
            .Setup(c => c.SelecionarTodos())
            .Returns([new Compromisso(
                "TesteDeCompromisso",
                new DateTime(2026, 12, 08),
                TimeSpan.FromHours(14),
                TimeSpan.FromHours(15),
                TipoCompromisso.Presencial,
                "Salão de Festas",
                null,
                contato
            )]);

        ServicoContato servicoContato = new ServicoContato(
            repositorioContato.Object,
            repositorioCompromisso.Object
        );

        // Ação
        Result resultado = servicoContato.Excluir(contato.Id);

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        repositorioContato.Verify(r => r.Excluir(contato.Id), Times.Never);

    }
}