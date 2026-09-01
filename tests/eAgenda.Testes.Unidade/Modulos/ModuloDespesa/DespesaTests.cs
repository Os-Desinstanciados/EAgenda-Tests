using eAgenda.Dominio.Modulos.ModuloCategoria;
using eAgenda.Dominio.Modulos.ModuloDespesa;

namespace eAgenda.Testes.Unidade.Modulos.ModuloDespesa;

[TestClass]
public sealed class DespesaTests
{
    [TestMethod]
    public void Validar_ComDescricaoVazio_DeveRetornarErro()
    {
        List<Categoria> categorias = new List<Categoria>
        {
            new Categoria("Alimentação")
        };

        Despesa despesa = new Despesa(
            null,
            new DateTime(2023, 03, 08),
            12,
            FormaPagamento.AVista,
            categorias
        );

        List<string> erros = despesa.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Descrição\" deve ser preenchido.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_ComValorVazio_DeveRetornarErro()
    {
        List<Categoria> categorias = new List<Categoria>
        {
            new Categoria("Alimentação")
        };
        
        Despesa despesa = new Despesa(
            "Compra Mês",
            new DateTime(2023, 03, 08),
            0,
            FormaPagamento.AVista,
            categorias
        );

        List<string> erros = despesa.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Valor\" deve ser maior que zero.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_ComFormaPagamenmtoVazia_DeveRetornarErro()
    {
        List<Categoria> categorias = new List<Categoria>
        {
            new Categoria("Alimentação")
        };
        
        Despesa despesa = new Despesa(
            "Compra Mês",
            new DateTime(2023, 03, 08),
            10,
            (FormaPagamento)99,
            categorias
        );

        List<string> erros = despesa.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "O campo \"Forma de Pagamento\" deve ser preenchido.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_ComCategoriaVazia_DeveRetornarErro()
    {
        Despesa despesa = new Despesa(
            "Compra Mês",
            new DateTime(2023, 03, 08),
            12,
            FormaPagamento.AVista,
            new List<Categoria>()
        );

        List<string> erros = despesa.Validar();

        Assert.HasCount(1, erros);
        Assert.AreEqual(
            "Selecione ao menos uma categoria.",
            erros.First()
        );
    }

    [TestMethod]
    public void Validar_DespesaValida_DeveRetornarSucesso()
    {
        List<Categoria> categorias = new List<Categoria>
        {
            new Categoria("Alimentação")
        };
        
        Despesa despesa = new Despesa(
            "Compra Mês",
            new DateTime(2023, 03, 08),
            10,
            FormaPagamento.AVista,
            categorias
        );

        List<string> erros = despesa.Validar();

        Assert.HasCount(0, erros);
    }

}