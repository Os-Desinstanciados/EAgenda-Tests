
using eAgenda.Dominio.Modulos.ModuloContato;

namespace eAgenda.Testes.Unidade.Modulos.ModuloContato;

[TestClass]
public sealed class ContatoTestes
{
    [TestMethod]
    public void Validar_ComNomeVazio_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Contato contato = new Contato(
            string.Empty,
            "teste@hotmail.com",
            "(49) 9999-9999",
            null,
            "Empresa"
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = contato.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(2, erros);
        Assert.AreEqual(
            "O campo \"Nome\" deve ser preenchido.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_ComNomeCurto_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Contato contato = new Contato(
            new string('A', 1),
            "teste@hotmail.com",
            "(49) 9999-9999",
            null,
            "Empresa"
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = contato.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Nome\" deve conter entre 2 e 100 caracteres.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_ComNomeLongo_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Contato contato = new Contato(
            new string('A', 101),
            "teste@hotmail.com",
            "(49) 9999-9999",
            null,
            "Empresa"
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = contato.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Nome\" deve conter entre 2 e 100 caracteres.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_EmailVazio_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Contato contato = new Contato(
            "ContatoTeste",
            string.Empty,
            "(49) 9999-9999",
            null,
            "Empresa"
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = contato.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Email\" deve ser preenchido.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_EmailInvalido_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Contato contato = new Contato(
            "João",
            "emailinvalido.com",
            "(49) 9999-9999",
            null,
            "Empresa"
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = contato.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"E-mail\" deve conter um endereço de e-mail válido.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_TelefoneVazio_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Contato contato = new Contato(
            "João",
            "teste@hotmail.com",
            string.Empty,
            null,
            null
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = contato.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Telefone\" deve ser preenchido.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_CargoLongo_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Contato contato = new Contato(
            "João",
            "teste@hotmail.com",
            "(49) 9999-9999",
            new string('A', 101),
            null
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = contato.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Cargo\" deve conter no máximo 100 caracteres.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_EmpresaLonga_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Contato contato = new Contato(
            "João",
            "teste@hotmail.com",
            "(49) 9999-9999",
            null,
            new string('A', 101)
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = contato.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Empresa\" deve conter no máximo 100 caracteres.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_ContatoValido_DeveRetornarSemErros()
    {
        // Arranjo
        Contato contato = new(
            "João",
            "teste@hotmail.com",
            "(49) 99999-9999",
            "Desenvolvedor",
            "Empresa"
        );

        // Ação
        List<string> erros = contato.Validar();

        // Asserção
        Assert.HasCount(0, erros);
    }
}