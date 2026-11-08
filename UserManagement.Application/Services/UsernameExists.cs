using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Services;

public class UsernameExists : IUsernameExists
{
    private readonly IDb _db;

    public UsernameExists(IDb db)
    {
        _db = db;
    }

    public Task<bool> ExecuteAsync(
        string username,
        int? exceptUserId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _db.Query<User>()
            .Where(u => u.UserName == username);

        if (exceptUserId.HasValue)
        {
            query = query.Where(
                u => u.ID != exceptUserId.Value);
        }

        return _db.AnyAsync(query, cancellationToken);
    }
}