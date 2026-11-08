namespace UserManagement.Application.Interfaces;

public interface IRegisterUser
{
    Task<bool> RegisterAsync(
        string username,
        string fullName,
        string email,
        string password,
        CancellationToken cancellationToken = default);
}