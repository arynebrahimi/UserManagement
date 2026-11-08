using UserManagement.Domain.Entities;

namespace UserManagement.Application.Interfaces;

public interface IUpdateUser
{
    Task UpdateAsync(
        User user,
        CancellationToken cancellationToken = default);
}