using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Services;

public class RegisterUser : IRegisterUser
{
    private readonly IDb _db;
    private readonly IUsernameExists _usernameExists;
    private readonly IPasswordService _passwordService;

    public RegisterUser(
        IDb db,
        IUsernameExists usernameExists,
        IPasswordService passwordService)
    {
        _db = db;
        _usernameExists = usernameExists;
        _passwordService = passwordService;
    }

    public async Task<bool> RegisterAsync(
        string username,
        string fullName,
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        username = username.Trim();

        if (await _usernameExists.ExecuteAsync(
            username,
            cancellationToken: cancellationToken))
        {
            return false;
        }

        var user = new User
        {
            UserName = username,
            FullName = fullName.Trim(),
            Email = email.Trim(),
            CreatedAt = DateTime.UtcNow,
            Role = "User"
        };

        user.PasswordHash = _passwordService.HashPassword(
            user,
            password);

        _db.Add(user);

        await _db.SaveChangesAsync(cancellationToken);

        return true;
    }
}