using simplified_picpay.Models;

namespace simplified_picpay.Services.Abstractions
{
    public interface IAccountService
    {
        public bool VerifyDocument(Account account);
        public bool VerifyAccountType(Account account);
        public string PasswordHasher(Account account, string password);
        public bool CheckPassword(Account account, string hashedPassword, string providerPassword);
        public Task<bool> EnableAccount(Guid id, CancellationToken cancellationToken = default);
    }
}