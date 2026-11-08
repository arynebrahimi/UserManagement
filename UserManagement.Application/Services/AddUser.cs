using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Services;

public class AddUser : IAddUser
{
    private readonly IDb _db;

    public AddUser(IDb db)
    {
        _db = db;
    }

    public async Task AddAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        _db.Add(user);
        await _db.SaveChangesAsync(cancellationToken);
    }
}