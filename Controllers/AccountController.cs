using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using simplified_picpay.DTOs;
using simplified_picpay.Models;
using simplified_picpay.Repositories.Abstractions;
using simplified_picpay.Services.Abstractions;

namespace simplified_picpay.Controllers
{
    [ApiController]
    [Route("accounts")]
    public class AccountController(IAccountRepository repository, IAccountService service) : ControllerBase
    {
        private readonly IAccountRepository _repository = repository;
        private readonly IAccountService _service = service;

        [HttpPost]
        [AllowAnonymous]
        public Task<Account> CreateAsync(CancellationToken cancellationToken,
                                        [FromBody] CreateAccountDTO createAccountDTO)
        {
            var account = new Account
            {
                FullName = createAccountDTO.FullName,
                Email = createAccountDTO.Email,
                CurrentBalance = createAccountDTO.CurrentBalance,
                AccountType = createAccountDTO.AccountType,
                Document = createAccountDTO.Document
            };

            var passwordHash = _service.PasswordHasher(account, account.PasswordHash);
            account.PasswordHash = passwordHash;

            try
            {
                return _repository.CreateAsync(account, cancellationToken);
            }
            catch
            {
                throw new Exception("Erro");
            }
        }
    }
}