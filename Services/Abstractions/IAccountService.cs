using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using simplified_picpay.DTOs;
using simplified_picpay.Models;

namespace simplified_picpay.Services.Abstractions
{
    public interface IAccountService
    {
        public bool VerifyDocument(Account account);
        public bool VerifyAccountType(Account account);
        public string PasswordHasher(Account account, string password);
        public bool CheckPassword(Account account, string hashedPassword, string providerPassword);
    }
}