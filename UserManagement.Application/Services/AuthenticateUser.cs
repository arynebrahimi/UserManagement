using UserManagement.Application.Interfaces;
using UserManagement.Application.Models;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Services;

public class AuthenticateUser : IAuthenticateUser
{
    private readonly IDb _db;
    private readonly IGetUserByUsername _getUserByUsername;
    private readonly IPasswordService _passwordService;

    public AuthenticateUser(
        IDb db,
        IGetUserByUsername getUserByUsername,
        IPasswordService passwordService)
    {
        _db = db;
        _getUserByUsername = getUserByUsername;
        _passwordService = passwordService;
    }

    public async Task<AuthenticatedUser?> AuthenticateAsync(
        string username,
        string password,
        CancellationToken cancellationToken = default)
    {
        var user = await _getUserByUsername.GetByUsernameAsync(
            username.Trim(),
            cancellationToken);

        if (user == null || string.IsNullOrEmpty(user.PasswordHash))
            return null;

        var result = _passwordService.VerifyPassword(
            user,
            user.PasswordHash,
            password);

        if (result == PasswordVerificationStatus.Failed)
            return null;

        if (result == PasswordVerificationStatus.SuccessRehashNeeded)
        {
            user.PasswordHash = _passwordService.HashPassword(
                user,
                password);

            _db.Update(user);

            await _db.SaveChangesAsync(cancellationToken);
        }

        return new AuthenticatedUser
        {
            ID = user.ID,
            UserName = user.UserName,
            Role = user.Role
        };
    }
}