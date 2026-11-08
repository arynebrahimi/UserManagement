using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Services;

public class DeleteUser : IDeleteUser
{
    private readonly IDb _db;

    public DeleteUser(IDb db)
    {
        _db = db;
    }

    public async Task DeleteAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        _db.Delete(user);
        await _db.SaveChangesAsync(cancellationToken);
    }
}