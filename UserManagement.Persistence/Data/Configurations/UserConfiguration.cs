using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagement.Domain.Entities;

namespace UserManagement.Persistence.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
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
    }
}