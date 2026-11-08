
using Microsoft.AspNetCore.Identity;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Web.Services;

public class PasswordService : IPasswordService
{
    private readonly IPasswordHasher<User> _passwordHasher;

    public PasswordService(
        IPasswordHasher<User> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public string HashPassword(User user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    public PasswordVerificationStatus VerifyPassword(
        User user,
        string hashedPassword,
        string providedPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(
            user,
            hashedPassword,
            providedPassword);

        return result switch
        {
            PasswordVerificationResult.Success =>
                PasswordVerificationStatus.Success,

            PasswordVerificationResult.SuccessRehashNeeded =>
                PasswordVerificationStatus.SuccessRehashNeeded,

            _ => PasswordVerificationStatus.Failed
        };
    }
}
