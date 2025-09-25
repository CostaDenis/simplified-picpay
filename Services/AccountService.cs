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
        public bool VerifyDocument(CreateAccountDTO createAccountDTO)
        {
            var accountType = createAccountDTO.AccountType.ToString();
            var document = createAccountDTO.Document.ToString();

            if (accountType == EAccountType.Storekeeper.ToString() && document.Length != 14 ||
                accountType == EAccountType.User.ToString() && document.Length != 11)
                return false;

            return true;
        }

        public string PasswordHasher(Account account, string password)
        {
            var hasher = new PasswordHasher<Account>();
            var passwordHash = hasher.HashPassword(account, password);

            return passwordHash;
        }

        public string ConvertAccountType(Account account)
            => account.AccountType.ToString();

    }
}