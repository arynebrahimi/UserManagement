using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Services;

public class GetUserByUsername : IGetUserByUsername
{
    private readonly IDb _db;

    public GetUserByUsername(IDb db)
    {
        _db = db;
    }

    public Task<User?> GetByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default)
    {
        var query = _db.Query<User>()
            .Where(u => u.UserName == username);

        return _db.FirstOrDefaultAsync(
            query,
            cancellationToken);
    }
}