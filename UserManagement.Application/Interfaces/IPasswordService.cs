
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Interfaces;

public interface IPasswordService
{
    string HashPassword(User user, string password);

    PasswordVerificationStatus VerifyPassword(
        User user,
        string hashedPassword,
        string providedPassword);
}

public enum PasswordVerificationStatus
{
    Failed,
    Success,
    SuccessRehashNeeded
}
