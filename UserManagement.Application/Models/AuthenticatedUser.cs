namespace UserManagement.Application.Models;

public class AuthenticatedUser
{
    public int ID { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
}