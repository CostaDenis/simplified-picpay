using simplified_picpay.Enums;

namespace simplified_picpay.DTOs
{
    public class CreateAccountDTO
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public decimal CurrentBalance { get; set; }
        public AccountType AccountType { get; set; }
        public string Document { get; set; } = string.Empty;
    }
}