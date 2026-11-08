using UserManagement.Domain.Entities;

namespace UserManagement.Application.Interfaces;

public interface IGetAllUsers
{
    Task<List<User>> GetAllAsync(
        CancellationToken cancellationToken = default);
}