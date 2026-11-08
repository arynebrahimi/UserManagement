namespace UserManagement.Application.Interfaces;

public interface IUsernameExists
{
    Task<bool> ExecuteAsync(
        string username,
        int? exceptUserId = null,
        CancellationToken cancellationToken = default);
}