using eAgenda.Dominio.Compartilhado.Identity;

namespace eAgenda.Testes.Integracao.Identity;

public sealed class ProvedorDeUsuarioFake(Guid userId) : IUserProvider
{
    public Guid? Id => userId;

    public bool EstaAutenticado => true;
}
