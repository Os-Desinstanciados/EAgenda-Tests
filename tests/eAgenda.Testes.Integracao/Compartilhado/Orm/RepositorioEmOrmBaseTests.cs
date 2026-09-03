using eAgenda.Infra.Compartilhado.Orm;
using eAgenda.Testes.Integracao.Identity;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.Testes.Integracao.Compartilhado.Orm;

public abstract class RepositorioEmOrmBaseTests
{
    protected EAgendaDbContext CriarDbContext()
    {
        DbContextOptions<EAgendaDbContext> options =
            new DbContextOptionsBuilder<EAgendaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

        return new EAgendaDbContext(
            options,
            new ProvedorDeUsuarioFake(Guid.NewGuid())
        );
    }
}