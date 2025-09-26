using Microsoft.AspNetCore.Identity;
using simplified_picpay.Enums;
using simplified_picpay.Models;
using simplified_picpay.Services.Abstractions;

namespace simplified_picpay.Services
{
    public class AccountService : IAccountService
    {
        public bool VerifyDocument(Account account)
        {
            var accountType = account.AccountType;
            var document = account.Document;

            if (accountType.ToLower() == EAccountType.Storekeeper.ToString().ToLower() && document.Length != 14 ||
                accountType.ToLower() == EAccountType.User.ToString().ToLower() && document.Length != 11)
                return false;

            return true;
        }

        public bool VerifyAccountType(Account account)
        {
            if (account.AccountType.ToLower() != EAccountType.Storekeeper.ToString().ToLower() &&
                account.AccountType.ToLower() != EAccountType.User.ToString().ToLower())
                return false;

            return true;
        }

        public string PasswordHasher(Account account, string password)
        {
            var hasher = new PasswordHasher<Account>();
            var passwordHash = hasher.HashPassword(account, password);

            return passwordHash;
        }

        public bool CheckPassword(Account account, string hashedPassword, string providerPassword)
        {
            var hasher = new PasswordHasher<Account>();
            var result = hasher.VerifyHashedPassword(account, hashedPassword, providerPassword);

            if (result == PasswordVerificationResult.Failed)
                return false;

            return true;
        }
    }
}