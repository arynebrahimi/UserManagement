using UserManagement.Domain.Entities;

namespace UserManagement.Application.Interfaces;

public interface IGetUserById
{
    Task<User?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);
}