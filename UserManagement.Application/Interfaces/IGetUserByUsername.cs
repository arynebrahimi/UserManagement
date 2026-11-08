using UserManagement.Domain.Entities;

namespace UserManagement.Application.Interfaces;

public interface IGetUserByUsername
{
    Task<User?> GetByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default);
}