using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using simplified_picpay.DTOs;
using simplified_picpay.Models;

namespace simplified_picpay.Repositories.Abstractions
{
    public interface IAccountRepository
    {
        Task<Account> CreateAsync(CreateAccountDTO createAccountDTO, CancellationToken cancellationToken = default);

    }
}