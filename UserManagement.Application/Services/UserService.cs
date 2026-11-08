using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Services;

public class UserService : IUserService
{
    private readonly IDb _db;

    public UserService(IDb db)
    {
        _db = db;
    }

    public Task<List<User>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var query = _db.Query<User>()
            .OrderBy(u => u.ID);

        return _db.ToListAsync(query, cancellationToken);
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

    public Task<bool> UsernameExistsAsync(
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

    public async Task AddAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        _db.Add(user);

        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        _db.Update(user);

        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        _db.Delete(user);

        await _db.SaveChangesAsync(cancellationToken);
    }
}