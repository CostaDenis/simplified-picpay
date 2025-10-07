

namespace simplified_picpay.DTOs.Account
{
    public class UpdateAccountDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}