
namespace simplified_picpay.Views.ViewModels
{
    public class LoggedAccountViewModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string PublicId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal CurrentBalance { get; set; }
        public string AccountType { get; set; } = string.Empty;
        public string Document { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;

    }
}