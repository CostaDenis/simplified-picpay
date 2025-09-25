using simplified_picpay.Enums;

namespace simplified_picpay.Models
{
    public class Account
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public decimal CurrentBalance { get; set; }
        public string AccountType { get; set; } = string.Empty;
        public string Document { get; set; } = string.Empty;
        public List<Transaction>? Payments { get; set; }
        public List<Transaction> Receipts { get; set; } = new();
        public bool IsActive { get; set; } = true;
    }
}