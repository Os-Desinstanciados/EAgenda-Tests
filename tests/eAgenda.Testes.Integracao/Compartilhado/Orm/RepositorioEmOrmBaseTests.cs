using eAgenda.Dominio.Compartilhado;
using eAgenda.Infra.Compartilhado.Orm;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.Testes.Integracao.Compartilhado.Orm;

public abstract class RepositorioEmOrmBaseTests
{
    protected EAgendaDbContext dbContext = null!;

    [TestInitialize]
    public void InicializarContexto()
    {
        dbContext = CriarDbContext();
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
                .UseInMemoryDatabase("eAgendaTestDB_Memory")
                .Options;

        return new EAgendaDbContext(options);
    }
}