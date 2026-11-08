using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Services;

public class UpdateUser : IUpdateUser
{
    private readonly IDb _db;

    public UpdateUser(IDb db)
    {
        _db = db;
    }

    public async Task UpdateAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        _db.Update(user);
        await _db.SaveChangesAsync(cancellationToken);
    }
}