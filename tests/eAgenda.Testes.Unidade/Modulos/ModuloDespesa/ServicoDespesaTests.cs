using eAgenda.Aplicacao.Modulos.ModuloDespesa;
using eAgenda.Dominio.Modulos.ModuloDespesa;
using eAgenda.Dominio.Modulos.ModuloCategoria;
using FluentResults;
using Moq;

namespace eAgenda.Testes.Unidade.Modulos.ModuloDespesa;

[TestClass]
public sealed class ServicoDespesaTests
{
    [TestMethod]
    public void Cadastrar_DadosValidosPersisteDespesa()
    {
        // Arranjo
        Mock<IRepositorioDespesa> repositorioDespesa = new Mock<IRepositorioDespesa>();
        Mock<IRepositorioCategoria> repositorioCategoria = new Mock<IRepositorioCategoria>();

        Guid categoriaId = Guid.NewGuid();
        Categoria categoria = new Categoria("Categ Teste") { Id = categoriaId };        

        repositorioDespesa.Setup(r => r.SelecionarTodos()).Returns([]);

        Despesa? despesaCadastrada = null;

        repositorioDespesa
            .Setup(r => r.Cadastrar(It.IsAny<Despesa>()))
            .Callback<Despesa>(despesa => despesaCadastrada = despesa);

        repositorioCategoria
            .Setup(r => r.SelecionarPorId(categoria.Id))
            .Returns(categoria);

        
        ServicoDespesa servicoDespesa = new ServicoDespesa(
            repositorioDespesa.Object,
            repositorioCategoria.Object
        );

        // Ação
        Result resultado = servicoDespesa.Cadastrar(
            new CadastrarDespesaDto(
            "Despesa Teste",
            new DateTime(2026, 08, 08),
            100m,
            FormaPagamento.AVista,
            new List<Guid> { categoria.Id }
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(despesaCadastrada);
        Assert.AreEqual("Despesa Teste", despesaCadastrada.Descricao);
        Assert.AreEqual(new DateTime(2026, 08, 08), despesaCadastrada.DataOcorrencia);        
        Assert.AreEqual(100m, despesaCadastrada.Valor);         
        Assert.AreEqual(FormaPagamento.AVista, despesaCadastrada.FormaPagamento);               
        Assert.AreEqual(categoria, despesaCadastrada.Categorias[0]);         
        repositorioDespesa.Verify(r => r.Cadastrar(It.IsAny<Despesa>()), Times.Once);
    }    

    [TestMethod]
    public void Editar_DadosValidos_EditaDespesa()
    {
        // Arranjo
        Mock<IRepositorioDespesa> repositorioDespesa = new();
        Mock<IRepositorioCategoria> repositorioCategoria = new();

        Guid categoriaId = Guid.NewGuid();
        Categoria categoria2 = new Categoria("Categ Teste2") { Id = categoriaId };

        repositorioDespesa
            .Setup(r => r.SelecionarTodos())
            .Returns([]);

        Despesa? despesaEditada = null;

        repositorioDespesa
            .Setup(r => r.Editar(It.IsAny<Guid>(), It.IsAny<Despesa>()))
            .Callback<Guid, Despesa>((id, despesa) => despesaEditada = despesa)
            .Returns(true);
        
        repositorioCategoria
            .Setup(r => r.SelecionarPorId(categoria2.Id))
            .Returns(categoria2);

        ServicoDespesa servicoDespesa = new ServicoDespesa(
            repositorioDespesa.Object,
            repositorioCategoria.Object
        );

        Guid despesaId = Guid.NewGuid();

        // Ação
        Result resultado = servicoDespesa.Editar(
            new EditarDespesaDto(
                despesaId,
                "Despesa Editada",
                new DateTime(2026, 08, 06),
                200m,
                FormaPagamento.Credito,
                new List<Guid> { categoria2.Id }
            )
        );

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        Assert.IsNotNull(despesaEditada);
        Assert.AreEqual("Despesa Editada", despesaEditada.Descricao);
        Assert.AreEqual(new DateTime(2026, 08, 06), despesaEditada.DataOcorrencia);        
        Assert.AreEqual(200m, despesaEditada.Valor);         
        Assert.AreEqual(FormaPagamento.Credito, despesaEditada.FormaPagamento);               
        Assert.AreEqual(categoria2, despesaEditada.Categorias[0]);       
        repositorioDespesa.Verify(r => r.Editar(despesaId, It.IsAny<Despesa>()), Times.Once);
    }     

    [TestMethod]
    public void Excluir_Despesa_ExcluiDespesa()
    {
        // Arranjo
       
        Categoria categoria = new Categoria("Categ Teste");        

        Despesa despesa = new Despesa(
            "Despesa Teste",
            new DateTime(2026, 08, 08),
            100m,
            FormaPagamento.AVista,
            new List<Categoria> { categoria }
        );

        Mock<IRepositorioDespesa> repositorioDespesa = new();
        Mock<IRepositorioCategoria> repositorioCategoria = new();

        repositorioDespesa
            .Setup(d => d.SelecionarPorId(despesa.Id))
            .Returns(despesa);

        repositorioCategoria
            .Setup(c => c.SelecionarTodos())
            .Returns([]);

        ServicoDespesa servicoDespesa = new ServicoDespesa(
            repositorioDespesa.Object,
            repositorioCategoria.Object
        );

        // Ação
        Result resultado = servicoDespesa.Excluir(despesa.Id);

        // Asserção
        Assert.IsTrue(resultado.IsSuccess);
        repositorioDespesa.Verify(r => r.Excluir(despesa.Id), Times.Once);
    }
    
}