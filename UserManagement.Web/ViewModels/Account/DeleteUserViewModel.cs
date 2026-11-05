
namespace UserManagement.Web.ViewModels.Account
{
    public class DeleteUserViewModel
    {
        public int ID { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}