using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using simplified_picpay.Data;
using simplified_picpay.DTOs;
using simplified_picpay.Enums;
using simplified_picpay.Models;
using simplified_picpay.Repositories.Abstractions;

namespace simplified_picpay.Repositories
{
    public class AccountRepository(AppDbContext context) : IAccountRepository
    {
        private readonly AppDbContext _context = context;

        public Task<Account> CreateAsync(CreateAccountDTO createAccountDTO, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}