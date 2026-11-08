using UserManagement.Domain.Entities;

namespace UserManagement.Application.Interfaces;

public interface IAddUser
{
    Task AddAsync(
        User user,
        CancellationToken cancellationToken = default);
}