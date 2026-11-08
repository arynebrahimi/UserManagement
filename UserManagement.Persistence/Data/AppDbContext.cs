using Microsoft.EntityFrameworkCore;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;

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

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");

            entity.HasKey(user => user.ID);

            entity.Property(user => user.UserName)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(user => user.UserName)
                .IsUnique();

            entity.Property(user => user.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(user => user.FullName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(user => user.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(user => user.CreatedAt)
                .IsRequired();

            entity.Property(user => user.Role)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("User");
        });
    }
}