
using eAgenda.Dominio.Modulos.ModuloCategoria;

namespace eAgenda.Testes.Unidade.Modulos.ModuloCategoria;

[TestClass]
public sealed class CategoriaTestes
{
    [TestMethod]
    public void Validar_ComNomeVazio_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Categoria categoria = new Categoria(
            string.Empty
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = categoria.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(2, erros);
        Assert.AreEqual(
            "O campo \"Título\" deve ser preenchido.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_ComNomeCurto_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Categoria categoria = new Categoria(
            new string('A', 1)
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = categoria.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Título\" deve conter entre 2 e 100 caracteres.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_ComNomeLongo_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Categoria categoria = new Categoria(
            new string('A', 101)
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = categoria.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Nome\" deve conter entre 2 e 100 caracteres.",
            erros.First()
        );
    }
    
}