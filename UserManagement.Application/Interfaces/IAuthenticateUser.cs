namespace UserManagement.Application.Interfaces;

using UserManagement.Application.Models;

public interface IAuthenticateUser
{
    Task<AuthenticatedUser?> AuthenticateAsync(
        string username,
        string password,
        CancellationToken cancellationToken = default);
}