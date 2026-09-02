using System.Reflection;
using eAgenda.Dominio.Compartilhado.Identity;
using eAgenda.Dominio.Modulos.ModuloCategoria;
using eAgenda.Dominio.Modulos.ModuloCompromisso;
using eAgenda.Dominio.Modulos.ModuloContato;
using eAgenda.Dominio.Modulos.ModuloDespesa;
using eAgenda.Dominio.Modulos.ModuloInstituicao;
using eAgenda.Dominio.Modulos.ModuloTarefa;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.Infra.Compartilhado.Orm;

public sealed class EAgendaDbContext(
    DbContextOptions<EAgendaDbContext> options,
    IUserProvider? userProvider = null
    ) : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Contato> Contatos => Set<Contato>();
    public DbSet<Compromisso> Compromissos => Set<Compromisso>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Despesa> Despesas => Set<Despesa>();
    public DbSet<ItemTarefa> ItensTarefa => Set<ItemTarefa>();
    public DbSet<Tarefa> Tarefas => Set<Tarefa>();
    public DbSet<Instituicao> Instituicoes => Set<Instituicao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        Guid? userId = userProvider?.Id;

        Assembly assembly = typeof(EAgendaDbContext).Assembly;

        modelBuilder.ApplyConfigurationsFromAssembly(assembly);

        modelBuilder.Entity<Contato>()
            .HasQueryFilter(c => userProvider == null || c.UserId == userProvider.Id);

        modelBuilder.Entity<Compromisso>()
            .HasQueryFilter(c => userProvider == null || c.UserId == userProvider.Id);

        modelBuilder.Entity<Categoria>()
            .HasQueryFilter(c => userProvider == null || c.UserId == userProvider.Id);

        modelBuilder.Entity<Despesa>()
            .HasQueryFilter(d => userProvider == null || d.UserId == userProvider.Id);

        modelBuilder.Entity<ItemTarefa>()
            .HasQueryFilter(i => userProvider == null || i.UserId == userProvider.Id);

        modelBuilder.Entity<Tarefa>()
            .HasQueryFilter(d => userProvider == null || d.UserId == userProvider.Id);

    }

    public override int SaveChanges()
    {
        Guid? userId = userProvider?.Id;

        if (!userId.HasValue)
        {
            throw new UnauthorizedAccessException(
                "Não é possível salvar entidades da instituição sem estar autenticado."
            );
        }

        foreach (var entry in ChangeTracker.Entries<IEntidadeUsuario>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Entity.UserId == Guid.Empty)
                    {
                        entry.Property(nameof(IEntidadeUsuario.UserId)).CurrentValue = userId.Value;
                    }
                    else if (entry.Entity.UserId != userId.Value)
                    {
                        throw new UnauthorizedAccessException(
                            "Tentativa de criar entidade para outra instituição."
                        );
                    }

                    break;

                case EntityState.Modified:
                    Guid idOriginalInstituicao = entry
                        .Property(nameof(IEntidadeUsuario.UserId))
                        .OriginalValue is Guid idOriginal
                        ? idOriginal
                        : Guid.Empty;

                    Guid idAtualInstituicao = entry
                        .Property(nameof(IEntidadeUsuario.UserId))
                        .CurrentValue is Guid idAtual
                        ? idAtual
                        : Guid.Empty;

                    if (idOriginalInstituicao != idAtualInstituicao)
                    {
                        throw new UnauthorizedAccessException(
                              "Não é permitido alterar a instituição de uma entidade."
                          );
                    }

                    if (idAtualInstituicao != userId.Value)
                    {
                        throw new UnauthorizedAccessException(
                            "Tentativa de modificar entidade de outra instituição."
                        );
                    }

                    break;

                case EntityState.Deleted:
                    Guid instituicaoOriginal = entry
                        .Property(nameof(IEntidadeUsuario.UserId))
                        .OriginalValue is Guid original
                        ? original
                        : Guid.Empty;

                    if (instituicaoOriginal != userId.Value)
                    {
                        throw new UnauthorizedAccessException(
                            "Tentativa de excluir entidade de outra instituicao."
                        );
                    }

                    break;

            }
        }

        return base.SaveChanges();
    }
}
