using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Services;

public class GetUserById : IGetUserById
{
    private readonly IDb _db;

    public GetUserById(IDb db)
    {
        _db = db;
    }

    public Task<User?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var query = _db.Query<User>()
            .Where(u => u.ID == id);

        return _db.FirstOrDefaultAsync(
            query,
            cancellationToken);
    }
}