using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using simplified_picpay.Data;
using simplified_picpay.DTOs;
using simplified_picpay.Enums;
using simplified_picpay.Models;
using simplified_picpay.Repositories.Abstractions;
using simplified_picpay.Services.Abstractions;

namespace simplified_picpay.Repositories
{
    public class AccountRepository(AppDbContext context) : IAccountRepository
    {
        private readonly AppDbContext _context = context;


        public async Task<Account> CreateAsync(Account account, CancellationToken cancellationToken = default)
        {
            // var account = new Account
            // {
            //     FullName = createAccountDTO.FullName,
            //     Email = createAccountDTO.Email,
            //     PasswordHash = createAccountDTO.Password,
            //     CurrentBalance = createAccountDTO.CurrentBalance,
            //     AccountType = createAccountDTO.AccountType,
            //     Document = createAccountDTO.Document
            // };

            await _context.Accounts.AddAsync(account, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return account;
        }


    }
}