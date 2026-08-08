
using eAgenda.Dominio.Modulos.ModuloCompromisso;

namespace eAgenda.Testes.Unidade.Modulos.ModuloCompromisso;

[TestClass]
public sealed class CompromissoTestes
{
    //Arranjo [Configura dados do teste]
    //Ação [Executa a ação sob teste]
    //Asserção [Checa o resultado comparado com o esperado]

    [TestMethod]
    public void Validar_ComAssuntoVazio_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Compromisso compromisso = new Compromisso(
            string.Empty,
            new DateTime(2026, 08, 08),
            TimeSpan.FromHours(14),
            TimeSpan.FromHours(15, 5),
            TipoCompromisso.Presencial,
            "Salão de Festas",
            null,
            null
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = compromisso.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Assunto\" deve ser preenchido.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_ComAssuntoCurto_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Compromisso compromisso = new Compromisso(
            new string('A', 1),
            new DateTime(2023, 03, 08),
            TimeSpan.FromHours(14),
            TimeSpan.FromHours(15, 5),
            TipoCompromisso.Presencial,
            "Salão de Festas",
            null,
            null
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = compromisso.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Assunto\" deve conter entre 2 e 100 caracteres.",
            erros.First()
        );

    }

    [TestMethod]
    public void Validar_ComAssuntoLongo_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Compromisso compromisso = new Compromisso(
            new string('A', 101),
            new DateTime(2023, 03, 08),
            TimeSpan.FromHours(14),
            TimeSpan.FromHours(15, 5),
            TipoCompromisso.Presencial,
            "Salão de Festas",
            null,
            null
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = compromisso.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Assunto\" deve conter entre 2 e 100 caracteres.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_ComDataVazia_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Compromisso compromisso = new Compromisso(
            "Compromisso",
            new DateTime (default),
            TimeSpan.FromHours (14),
            TimeSpan.FromHours (15, 5),
            TipoCompromisso.Remoto,
            null,
            "google.com",
            null
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = compromisso.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Data de Ocorrência\" deve ser preenchido.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_ComHoraInicioVazio_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Compromisso compromisso = new Compromisso(
            "Compromisso",
            new DateTime(2023, 03, 08),
            TimeSpan.FromHours(default),
            TimeSpan.FromHours(15, 5),
            TipoCompromisso.Presencial,
            "Salão",
            null,
            null
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = compromisso.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Hora de Início\" deve ser preenchido.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_ComHoraTerminoVazio_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Compromisso compromisso = new Compromisso(
            "Compromisso",
            new DateTime(2023, 03, 08),
            TimeSpan.FromHours(15, 5),
            TimeSpan.FromHours(default),
            TipoCompromisso.Presencial,
            "Salão",
            null,
            null
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = compromisso.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Hora de Término\" deve ser preenchido.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_ComHoraAnteriorAoInicio_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Compromisso compromisso = new Compromisso(
            "Compromisso",
            new DateTime(2023, 03, 08),
            TimeSpan.FromHours(15.5),
            TimeSpan.FromHours(12),
            TipoCompromisso.Presencial,
            "Salão",
            null,
            null
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = compromisso.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "A hora de término deve ser posterior à hora de início.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_TipoPresencialComLocalVazio_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Compromisso compromisso = new Compromisso(
            "Compromisso",
            new DateTime(2023, 03, 08),
            TimeSpan.FromHours(14),
            TimeSpan.FromHours(15, 5),
            TipoCompromisso.Presencial,
            null,
            null,
            null
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = compromisso.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Local\" deve ser preenchido para compromissos presenciais.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_TipoRemotoComLinkVazio_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Compromisso compromisso = new Compromisso(
            "Compromisso",
            new DateTime(2023, 03, 08),
            TimeSpan.FromHours(14),
            TimeSpan.FromHours(15, 5),
            TipoCompromisso.Remoto,
            null,
            null,
            null
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = compromisso.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Link\" deve ser preenchido para compromissos remotos.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_ComTipoPresencialELocalLongo_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Compromisso compromisso = new Compromisso(
            "Compromisso",
            new DateTime(2023, 03, 08),
            TimeSpan.FromHours(14),
            TimeSpan.FromHours(15, 5),
            TipoCompromisso.Presencial,
            new string('A', 256),
            null,
            null
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = compromisso.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Local\" deve conter no máximo 255 caracteres.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_ComTipoRemotoELinkLongo_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Compromisso compromisso = new Compromisso(
            "Compromisso",
            new DateTime(2023, 03, 08),
            TimeSpan.FromHours(14),
            TimeSpan.FromHours(15, 5),
            TipoCompromisso.Remoto,
            null,
            new string('A', 501),
            null
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = compromisso.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Link\" deve conter no máximo 500 caracteres.",
            erros.First()
        );
    }

}