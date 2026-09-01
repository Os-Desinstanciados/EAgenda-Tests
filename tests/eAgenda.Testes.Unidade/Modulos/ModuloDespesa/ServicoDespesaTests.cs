using eAgenda.Aplicacao.Modulos.ModuloDespesa;
using eAgenda.Dominio.Modulos.ModuloCategoria;
using eAgenda.Dominio.Modulos.ModuloDespesa;
using FluentResults;
using Moq;

namespace eAgenda.Testes.Unidade.Modulos.ModuloDespesa;

[TestClass]
public sealed class ServicoDespesaTests
{
    [TestMethod]
    public void Editar_AlterandoValorECategoria_PersisteDespesa()
    {
        // Arranjo
        Mock<IRepositorioDespesa> repositorioDespesa = new();
        Mock<IRepositorioCategoria> repositorioCategoria = new();

        Categoria alimentacao = new Categoria("Alimentação");
        Categoria perfumes = new Categoria("Perfumes");

        repositorioCategoria
            .Setup(r => r.SelecionarTodos())
            .Returns([alimentacao, perfumes]);

        Guid despesaId = Guid.NewGuid();

        Despesa? despesaEditada = null;

        repositorioDespesa
            .Setup(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Despesa>()))
            .Callback<Guid, Despesa>((id, despesa) => despesaEditada = despesa)
            .Returns(true);

        ServicoDespesa servicoDespesa = new ServicoDespesa(
            repositorioDespesa.Object,
            repositorioCategoria.Object
        );

        // Ação
        Result resultado = servicoDespesa.Editar(
            new EditarDespesaDto(
                despesaId,
                "Casa",
                new DateTime(2008, 09, 22),
                50,
                FormaPagamento.AVista,
                new List<Guid> { alimentacao.Id, perfumes.Id }
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(despesaEditada);
        Assert.AreEqual(50, despesaEditada.Valor);
        Assert.HasCount(2, despesaEditada.Categorias);

        repositorioDespesa.Verify(r => r.Editar(despesaId, It.IsAny<Despesa>()), Times.Once);
    }

    [TestMethod]
    public void Editar_SemCategorias_RetornaFalha()
    {
        // Arranjo
        Mock<IRepositorioDespesa> repositorioDespesa = new();
        Mock<IRepositorioCategoria> repositorioCategoria = new();

        EditarDespesaDto dto = new EditarDespesaDto(
            Guid.NewGuid(),
            "Casa",
            new DateTime(2026, 8, 31),
            50,
            FormaPagamento.AVista,
            new List<Guid>()
        );

        ServicoDespesa servicoDespesa = new ServicoDespesa(
            repositorioDespesa.Object,
            repositorioCategoria.Object
        );

        // Acao
        Result conseguiuEditar = servicoDespesa.Editar(dto);

        // Assercao
        Assert.IsTrue(conseguiuEditar.IsFailed);
        repositorioDespesa.Verify(r => r
            .Editar(It.IsAny<Guid>(),
            It.IsAny<Despesa>()), Times.Never);
    }

    [TestMethod]
    public void Visualizar_DespesaComCategoriaVinculada_RetorDados()
    {
        // Arranjo
        Mock<IRepositorioDespesa> repositorioDespesa = new();
        Mock<IRepositorioCategoria> repositorioCategoria = new();

        Guid despesaId = Guid.NewGuid();

        List<Categoria> categorias = new List<Categoria>
        {
            new Categoria("Alimentação"),
            new Categoria("Casa")
        };

        Despesa despesa = new Despesa(
            "Compras do mês",
            new DateTime(2026, 8, 31),
            150,
            FormaPagamento.AVista,
            categorias
        );

        repositorioDespesa
            .Setup(r => r.SelecionarPorId(despesaId))
            .Returns(despesa);


        ServicoDespesa servicoDespesa = new ServicoDespesa(
            repositorioDespesa.Object,
            repositorioCategoria.Object
        );

        // Acao
        Result<DetalhesDespesaDto> conseguiuVisualizar = servicoDespesa.SelecionarPorId(despesaId);

        // Assercao
        Assert.IsTrue(conseguiuVisualizar.IsSuccess);
        Assert.HasCount(2, conseguiuVisualizar.Value.Categorias);
    }

    [TestMethod]
    public void Listar_TodasDespesas_RetornaDespesasCadastradas()
    {
        // Arranjo
        Mock<IRepositorioDespesa> repositorioDespesa = new();
        Mock<IRepositorioCategoria> repositorioCategoria = new();

        Guid despesaId = Guid.NewGuid();

        List<Categoria> categorias = new List<Categoria>
        {
            new Categoria("Alimentação"),
            new Categoria("Casa")
        };

        Despesa despesa = new Despesa(
            "Compras do mês",
            new DateTime(2026, 8, 31),
            150,
            FormaPagamento.AVista,
            categorias
        );

        Despesa despesaExtra = new Despesa(
            "Perfume presente",
            new DateTime(2026, 9, 3),
            10,
            FormaPagamento.AVista,
            categorias
        );

        repositorioDespesa
            .Setup(r => r.SelecionarTodos())
            .Returns([despesa, despesaExtra]);


        ServicoDespesa servicoDespesa = new ServicoDespesa(
            repositorioDespesa.Object,
            repositorioCategoria.Object
        );

        // Acao
        List<ListarDespesasDto> despesas = servicoDespesa.SelecionarTodos();

        // Assercao
        Assert.HasCount(2, despesas);
    }

    [TestMethod]
    public void Excluir_Despesa_DeveExcluirComVinculosCategorias()
    {
        // Arranjo
        Mock<IRepositorioDespesa> repositorioDespesa = new();
        Mock<IRepositorioCategoria> repositorioCategoria = new();

        Guid despesaId = Guid.NewGuid();
        List<Categoria> categorias = new List<Categoria>
        {
            new Categoria("Alimentação"),
            new Categoria("Casa")
        };

        Despesa despesa = new Despesa(
            "Compras do mês",
            new DateTime(2026, 8, 31),
            150,
            FormaPagamento.AVista,
            categorias
        );

        repositorioDespesa
            .Setup(r => r.SelecionarPorId(despesaId))
            .Returns(despesa);
        
        ServicoDespesa servicoDespesa = new ServicoDespesa(
            repositorioDespesa.Object,
            repositorioCategoria.Object
        );

        // Acao
        Result conseguiuExcluir = servicoDespesa.Excluir(despesaId);

        // Assercao
        Assert.IsTrue(conseguiuExcluir.IsSuccess);
        repositorioDespesa.Verify(r => r.Excluir(despesaId), Times.Once);
    }


    /*
    
    */
}