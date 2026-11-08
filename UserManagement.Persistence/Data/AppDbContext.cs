using Microsoft.EntityFrameworkCore;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;
using UserManagement.Persistence.Data.Configurations;

namespace UserManagement.Persistence.Data;

public class AppDbContext : DbContext, IDb
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Generic Query
    public IQueryable<T> Query<T>() where T : class
    {
        return Set<T>().AsNoTracking();
    }

    // Generic Add
    public void Add<T>(T entity) where T : class
    {
        Set<T>().Add(entity);
    }

    // Generic Update
    public void Update<T>(T entity) where T : class
    {
        Set<T>().Update(entity);
    }

    // Generic Delete
    public void Delete<T>(T entity) where T : class
    {
        Set<T>().Remove(entity);
    }

    // Generic DeleteById
    public async Task<bool> DeleteByIdAsync<T>(
        int id,
        CancellationToken cancellationToken = default)
        where T : class
    {
        var entity = await Set<T>().FindAsync(
            new object[] { id },
            cancellationToken);

        if (entity == null)
            return false;

        Set<T>().Remove(entity);

        return true;
    }
    public Task<List<T>> ToListAsync<T>(
    IQueryable<T> query,
    CancellationToken cancellationToken = default)
    where T : class
    {
        return EntityFrameworkQueryableExtensions
            .ToListAsync(query, cancellationToken);
    }

    public Task<T?> FirstOrDefaultAsync<T>(
        IQueryable<T> query,
        CancellationToken cancellationToken = default)
        where T : class
    {
        return EntityFrameworkQueryableExtensions
            .FirstOrDefaultAsync(query, cancellationToken);
    }

    public Task<bool> AnyAsync<T>(
        IQueryable<T> query,
        CancellationToken cancellationToken = default)
        where T : class
    {
        return EntityFrameworkQueryableExtensions
            .AnyAsync(query, cancellationToken);
    }

    protected override void OnModelCreating(
    ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}