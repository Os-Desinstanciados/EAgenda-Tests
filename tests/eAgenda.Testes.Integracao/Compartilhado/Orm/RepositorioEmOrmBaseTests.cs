using eAgenda.Dominio.Modulos.ModuloCompromisso;
using eAgenda.Dominio.Modulos.ModuloContato;
using eAgenda.Infra.Compartilhado.Orm;
using eAgenda.Infra.Modulos.ModuloCompromisso;
using eAgenda.Infra.Modulos.ModuloContato;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.Testes.Integracao.Compartilhado.Orm;

public abstract class RepositorioEmOrmBaseTests
{
    protected EAgendaDbContext dbContext = null!;
    protected RepositorioContatoEmOrm repositorioContato = null!;
    protected RepositorioCompromissoEmOrm repositorioCompromisso = null!;

    [TestInitialize]
    public void InicializarContexto()
    {
        dbContext = CriarDbContext();
        repositorioContato = new RepositorioContatoEmOrm(dbContext);
        repositorioCompromisso = new RepositorioCompromissoEmOrm(dbContext);

        // Contato
        BuilderSetup.SetCreatePersistenceMethod<Contato>((contato) =>
        {
            repositorioContato.Cadastrar(contato);
            dbContext.ChangeTracker.Clear();
        });

        BuilderSetup.SetCreatePersistenceMethod<IList<Contato>>((contatos) =>
        {
            foreach (Contato c in contatos)
                repositorioContato.Cadastrar(c);
            
            dbContext.ChangeTracker.Clear();
            
        });

        // Comprimisso
        BuilderSetup.SetCreatePersistenceMethod<Compromisso>((compromisso) =>
        {
            repositorioCompromisso.Cadastrar(compromisso);
            dbContext.ChangeTracker.Clear();
        });

        BuilderSetup.SetCreatePersistenceMethod<IList<Compromisso>>((compromissos) =>
        {
            foreach (Compromisso c in compromissos)
                repositorioCompromisso.Cadastrar(c);
                
            dbContext.ChangeTracker.Clear();
        });
    }

    [TestCleanup]
    public void DescartarContexto()
    {
        dbContext.Dispose();
    }

    private static EAgendaDbContext CriarDbContext()
    {
        DbContextOptions<EAgendaDbContext> options =
            new DbContextOptionsBuilder<EAgendaDbContext>()
                .UseInMemoryDatabase($"eAgendaTestDB_Memory_{Guid.NewGuid()}")
                .Options;

        return new EAgendaDbContext(options);
    }
}