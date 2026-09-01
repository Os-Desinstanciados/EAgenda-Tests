using eAgenda.Dominio.Modulos.ModuloCategoria;

namespace eAgenda.Testes.Unidade.Modulos.ModuloCategoria;

[TestClass]
public sealed class CategoriaTests
{
    [TestMethod]
    public void Validar_ComTituloVazio_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Categoria categoria = new Categoria();

        //Ação [Executa a ação sob teste]
        List<string> erros = categoria.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Título\" deve ser preenchido.",
            erros.First()
        );
    }
    
    [TestMethod]
    public void Validar_ComTituloCurto_DeveRetornarErro()
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
    public void Validar_ComTituloLongo_DeveRetornarErro()
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
            "O campo \"Título\" deve conter entre 2 e 100 caracteres.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_CategoriaValida_DeveRetornarSucesso()
    {
        //Arranjo [Configura dados do teste]
        Categoria categoria = new Categoria(
            "Casa & Banho"
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = categoria.Validar();
        
        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(0, erros);
    }

}