
using eAgenda.Dominio.Modulos.ModuloCategoria;
using eAgenda.Dominio.Modulos.ModuloDespesa;

namespace eAgenda.Testes.Unidade.Modulos.ModuloDespesa;

[TestClass]
public sealed class DespesaTestes
{
    //Arranjo [Configura dados do teste]
    //Ação [Executa a ação sob teste]
    //Asserção [Checa o resultado comparado com o esperado]

    [TestMethod]
    public void Validar_ComDescricaoVazia_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Categoria categoria = new Categoria("Limpeza");

        Despesa despesa = new Despesa(
            string.Empty,
            new DateTime(2026, 08, 08),            
            100,
            0,
            new List<Categoria> { categoria }            
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = despesa.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(2, erros);
        Assert.AreEqual(
            "O campo \"Descrição\" deve ser preenchido.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_ComDescricaoCurta_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Categoria categoria = new Categoria("Limpeza");

        Despesa despesa = new Despesa(
            new string('A', 1),
            new DateTime(2026, 08, 08),            
            100,
            0,
            new List<Categoria> { categoria }
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = despesa.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Descrição\" deve conter entre 2 e 100 caracteres.",
            erros.First()
        );

    }

    [TestMethod]
    public void Validar_ComDescricaoLonga_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Categoria categoria = new Categoria("Limpeza");

        Despesa despesa = new Despesa(
            new string('A', 101),
            new DateTime(2026, 08, 08),            
            100,
            0,
            new List<Categoria> { categoria }
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = despesa.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Descrição\" deve conter entre 2 e 100 caracteres.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_ComDataVazia_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Categoria categoria = new Categoria("Limpeza");

        Despesa despesa = new Despesa(
            "Despesa",
            new DateTime (default),
            100,
            0,
            new List<Categoria> { categoria }
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = despesa.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Data de Ocorrência\" deve ser preenchido.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_ComValorIgualMenorAZero_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Categoria categoria = new Categoria("Limpeza");

        Despesa despesa = new Despesa(
            "Despesa",
            new DateTime(2023, 03, 08),
            0,
            0,
            new List<Categoria> { categoria }
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = despesa.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Valor\" deve ser maior que zero.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_ComFormaDePagamentoVazia_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        Categoria categoria = new Categoria("Limpeza");

        Despesa despesa = new Despesa(
            "Despesa",
            new DateTime(2023, 03, 08),
            100,
            (FormaPagamento)999,
            new List<Categoria> { categoria }
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = despesa.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Forma de Pagamento\" deve ser preenchido.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_ComNenhumaCategoria_DeveRetornarErro()
    {
        //Arranjo [Configura dados do teste]
        

        Despesa despesa = new Despesa(
            "Despesa",
            new DateTime(2023, 03, 08),
            100,
            (FormaPagamento)0, //Cast is redundant.
            new List<Categoria> { }
        );

        //Ação [Executa a ação sob teste]
        List<string> erros = despesa.Validar();

        //Asserção [Checa o resultado comparado com o esperado]
        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "Selecione ao menos uma categoria.",
            erros.First()
        );
    }    

}