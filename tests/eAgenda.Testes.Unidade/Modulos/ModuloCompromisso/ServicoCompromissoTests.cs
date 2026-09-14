using eAgenda.Aplicacao.Modulos.ModuloCompromisso;
using eAgenda.Dominio.Modulos.ModuloCompromisso;
using eAgenda.Dominio.Modulos.ModuloContato;
using FluentResults;
using Moq;

namespace eAgenda.Testes.Unidade.Modulos.ModuloCompromisso;

[TestClass]
public sealed class ServicoCompromissoTests
{
    [TestMethod]
    public void Cadastrar_DadosValidos_PersisteCompromisso()
    {
        // Arranjo
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();
        Mock<IRepositorioContato> repositorioContato = new();

        repositorioCompromisso
            .Setup(r => r.SelecionarTodos())
            .Returns([]);

        Compromisso? compromissoCadastrado = null;

        repositorioCompromisso
            .Setup(r => r.Cadastrar(It.IsAny<Compromisso>()))
            .Callback<Compromisso>(compromisso =>
                compromissoCadastrado = compromisso);

        ServicoCompromisso servicoCompromisso = new ServicoCompromisso(
            repositorioCompromisso.Object,
            repositorioContato.Object
        );

        // Ação
        Result resultado = servicoCompromisso.Cadastrar(
            new CadastrarCompromissoDto(
                "Reunião de Projeto",
                new DateTime(2026, 08, 15),
                TimeSpan.FromHours(14),
                TimeSpan.FromHours(15),
                TipoCompromisso.Presencial,
                "Sala de Reuniões",
                null,
                null
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(compromissoCadastrado);
        Assert.AreEqual("Reunião de Projeto", compromissoCadastrado.Assunto);
        Assert.AreEqual(new DateTime(2026, 08, 15), compromissoCadastrado.DataOcorrencia);
        Assert.AreEqual(TimeSpan.FromHours(14), compromissoCadastrado.HoraInicio);
        Assert.AreEqual(TimeSpan.FromHours(15), compromissoCadastrado.HoraTermino);
        Assert.AreEqual(TipoCompromisso.Presencial, compromissoCadastrado.Tipo);
        Assert.AreEqual("Sala de Reuniões", compromissoCadastrado.Local);
        Assert.IsNull(compromissoCadastrado.Link);
        Assert.IsNull(compromissoCadastrado.Contato);
        repositorioCompromisso.Verify(r => r.Cadastrar(It.IsAny<Compromisso>()), Times.Once);
    }

    [TestMethod]
    public void Cadastrar_HorarioConflitante_RetornaFalha()
    {
        // Arranjo
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();
        Mock<IRepositorioContato> repositorioContato = new();

        Compromisso compromissoExistente = new Compromisso(
            "Compromisso Existente",
            new DateTime(2026, 08, 15),
            TimeSpan.FromHours(14),
            TimeSpan.FromHours(16),
            TipoCompromisso.Presencial,
            "Sala de Reuniões",
            null,
            null
        );

        repositorioCompromisso
            .Setup(r => r.SelecionarTodos())
            .Returns([compromissoExistente]);

        ServicoCompromisso servicoCompromisso = new ServicoCompromisso(
            repositorioCompromisso.Object,
            repositorioContato.Object
        );

        // Ação
        Result resultado = servicoCompromisso.Cadastrar(
            new CadastrarCompromissoDto(
                "Novo Compromisso",
                new DateTime(2026, 08, 15),
                TimeSpan.FromHours(15),
                TimeSpan.FromHours(17),
                TipoCompromisso.Presencial,
                "Escritório",
                null,
                null
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.Contains("Já existe um compromisso cadastrado neste intervalo de horário.", resultado.Errors.Single().Message);
        repositorioCompromisso.Verify(r => r.Cadastrar(It.IsAny<Compromisso>()), Times.Never);
    }

    [TestMethod]
    public void Cadastrar_ComContatoInexistente_RetornaFalha()
    {
        // Arranjo
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();
        Mock<IRepositorioContato> repositorioContato = new();

        Guid contatoId = Guid.NewGuid();

        repositorioContato
            .Setup(r => r.SelecionarPorId(contatoId))
            .Returns((Contato?)null);

        ServicoCompromisso servicoCompromisso = new ServicoCompromisso(
            repositorioCompromisso.Object,
            repositorioContato.Object
        );

        // Ação
        Result resultado = servicoCompromisso.Cadastrar(
            new CadastrarCompromissoDto(
                "Reunião de Projeto",
                new DateTime(2026, 08, 15),
                TimeSpan.FromHours(14),
                TimeSpan.FromHours(15),
                TipoCompromisso.Presencial,
                "Sala de Reuniões",
                null,
                contatoId
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.AreEqual("ContatoId", resultado.Errors.Single().Metadata["Campo"]);
        Assert.Contains("Selecione um contato válido.", resultado.Errors.Single().Message);
        repositorioCompromisso.Verify(r => r.Cadastrar(It.IsAny<Compromisso>()), Times.Never);
    }

    [TestMethod]
    public void Editar_DadosValidos_EditaCompromisso()
    {
        // Arranjo
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();
        Mock<IRepositorioContato> repositorioContato = new();

        repositorioCompromisso
            .Setup(r => r.SelecionarTodos())
            .Returns([]);

        Compromisso? compromissoEditado = null;

        repositorioCompromisso
            .Setup(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Compromisso>()))
            .Callback<Guid, Compromisso>((id, compromisso) =>
                compromissoEditado = compromisso)
            .Returns(true);

        ServicoCompromisso servicoCompromisso = new ServicoCompromisso(
            repositorioCompromisso.Object,
            repositorioContato.Object
        );

        Guid compromissoId = Guid.NewGuid();

        // Ação
        Result resultado = servicoCompromisso.Editar(
            new EditarCompromissoDto(
                compromissoId,
                "Compromisso Editado",
                new DateTime(2026, 08, 20),
                TimeSpan.FromHours(16),
                TimeSpan.FromHours(18),
                TipoCompromisso.Presencial,
                "Auditório",
                null,
                null
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(compromissoEditado);
        Assert.AreEqual("Compromisso Editado", compromissoEditado.Assunto);
        Assert.AreEqual(new DateTime(2026, 08, 20), compromissoEditado.DataOcorrencia);
        Assert.AreEqual(TimeSpan.FromHours(16), compromissoEditado.HoraInicio);
        Assert.AreEqual(TimeSpan.FromHours(18), compromissoEditado.HoraTermino);
        Assert.AreEqual(TipoCompromisso.Presencial, compromissoEditado.Tipo);
        Assert.AreEqual("Auditório", compromissoEditado.Local);
        Assert.IsNull(compromissoEditado.Link);
        Assert.IsNull(compromissoEditado.Contato);

        repositorioCompromisso.Verify(r => r.Editar(compromissoId, It.IsAny<Compromisso>()), Times.Once);
    }

    [TestMethod]
    public void Editar_HorarioConflito_RetornaFalha()
    {
        // Arranjo
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();
        Mock<IRepositorioContato> repositorioContato = new();

        Guid compromissoId = Guid.NewGuid();

        Compromisso compromissoExistente = new Compromisso(
            "Compromisso Existente",
            new DateTime(2026, 08, 20),
            TimeSpan.FromHours(14),
            TimeSpan.FromHours(17),
            TipoCompromisso.Presencial,
            "Sala de Reuniões",
            null,
            null
        );

        repositorioCompromisso
            .Setup(r => r.SelecionarTodos())
            .Returns([compromissoExistente]);

        ServicoCompromisso servicoCompromisso = new ServicoCompromisso(
            repositorioCompromisso.Object,
            repositorioContato.Object
        );

        // Ação
        Result resultado = servicoCompromisso.Editar(
            new EditarCompromissoDto(
                compromissoId,
                "Compromisso Editado",
                new DateTime(2026, 08, 20),
                TimeSpan.FromHours(16),
                TimeSpan.FromHours(18),
                TipoCompromisso.Presencial,
                "Auditório",
                null,
                null
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsFailed);
        Assert.Contains("Já existe um compromisso cadastrado neste intervalo de horário.", resultado.Errors.Single().Message);
        repositorioCompromisso.Verify(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Compromisso>()), Times.Never);
    }

    [TestMethod]
    public void Excluir_CompromissoExistente_ExcluiCompromisso()
    {
        // Arranjo
        Mock<IRepositorioCompromisso> repositorioCompromisso = new();
        Mock<IRepositorioContato> repositorioContato = new();

        Compromisso compromisso = new Compromisso(
            "Reunião de Projeto",
            new DateTime(2026, 08, 20),
            TimeSpan.FromHours(14),
            TimeSpan.FromHours(15),
            TipoCompromisso.Presencial,
            "Sala de Reuniões",
            null,
            null
        );

        repositorioCompromisso
            .Setup(r => r.SelecionarPorId(compromisso.Id))
            .Returns(compromisso);

        ServicoCompromisso servicoCompromisso = new ServicoCompromisso(
            repositorioCompromisso.Object,
            repositorioContato.Object
        );

        // Ação
        Result resultado = servicoCompromisso.Excluir(compromisso.Id);

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        repositorioCompromisso.Verify(r => r.Excluir(compromisso.Id), Times.Once);
    }
}