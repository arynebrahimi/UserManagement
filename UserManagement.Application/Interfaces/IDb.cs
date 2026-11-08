namespace UserManagement.Application.Interfaces;

public interface IDb
{
    IQueryable<T> Query<T>() where T : class;

    void Add<T>(T entity) where T : class;

    void Update<T>(T entity) where T : class;

    void Delete<T>(T entity) where T : class;

    Task<bool> DeleteByIdAsync<T>(
        int id,
        CancellationToken cancellationToken = default)
        where T : class;

    Task<List<T>> ToListAsync<T>(
        IQueryable<T> query,
        CancellationToken cancellationToken = default)
        where T : class;

    Task<T?> FirstOrDefaultAsync<T>(
        IQueryable<T> query,
        CancellationToken cancellationToken = default)
        where T : class;

    Task<bool> AnyAsync<T>(
        IQueryable<T> query,
        CancellationToken cancellationToken = default)
        where T : class;

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}