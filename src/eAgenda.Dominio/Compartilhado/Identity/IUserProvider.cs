namespace eAgenda.Dominio.Compartilhado.Identity;

public interface IUserProvider
{
    Guid? Id { get; }
    bool EstaAutenticado { get; }
}