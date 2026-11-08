using UserManagement.Domain.Entities;

namespace UserManagement.Application.Interfaces;

public interface IDeleteUser
{
    Task DeleteAsync(
        User user,
        CancellationToken cancellationToken = default);
}