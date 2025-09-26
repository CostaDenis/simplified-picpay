using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using simplified_picpay.DTOs;
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

            if (accountType == EAccountType.Storekeeper.ToString() && document.Length != 14 ||
                accountType == EAccountType.User.ToString() && document.Length != 11)
                return false;

            return true;
        }

        public bool VerifyAccountType(Account account)
        {
            if (account.AccountType.ToUpper() != EAccountType.Storekeeper.ToString().ToUpper() &&
                account.AccountType.ToUpper() != EAccountType.User.ToString().ToUpper())
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