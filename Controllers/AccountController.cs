using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using simplified_picpay.DTOs;
using simplified_picpay.Models;
using simplified_picpay.Repositories.Abstractions;
using simplified_picpay.Services.Abstractions;
using simplified_picpay.Views.ViewModels;

namespace simplified_picpay.Controllers
{
    [ApiController]
    [Route("accounts")]
    public class AccountController(IAccountRepository repository, IAccountService AccountService) : ControllerBase
    {
        private readonly IAccountRepository _repository = repository;
        private readonly IAccountService _accountService = AccountService;

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO,
                                    [FromServices] ITokenService _tokenService,
                                    CancellationToken cancellationToken)
        {
            var account = await _repository.LoginAsync(loginDTO.Email, cancellationToken);

            if (account == null)
                return Unauthorized(new ResultViewModel<string>("Acesso negado!"));

            if (!_accountService.CheckPassword(account, account.PasswordHash, loginDTO.Password))
                return Unauthorized(new ResultViewModel<string>("Acesso negado!"));

            var token = _tokenService.GenerateTokenJwt(account.Id, account.Email, account.AccountType);

            return Ok(new LoggedAccountViewModel
            {
                Id = account.Id,
                FullName = account.FullName,
                Email = account.Email,
                CurrentBalance = account.CurrentBalance,
                AccountType = account.AccountType,
                Document = account.Document,
                Token = token
            });

        }



        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAsync([FromBody] CreateAccountDTO createAccountDTO,
                                            CancellationToken cancellationToken)
        {

            var account = new Account
            {
                FullName = createAccountDTO.FullName,
                Email = createAccountDTO.Email,
                PasswordHash = createAccountDTO.Password,
                CurrentBalance = createAccountDTO.CurrentBalance,
                AccountType = createAccountDTO.AccountType,
                Document = createAccountDTO.Document
            };

            if (!_accountService.VerifyAccountType(account))
                return BadRequest(new ResultViewModel<string>("Só está disponível conta de User e Storekeeper!"));

            if (!_accountService.VerifyDocument(account))
                return BadRequest(new ResultViewModel<string>("Verifique o documento!"));

            var passwordHash = _accountService.PasswordHasher(account, account.PasswordHash);
            account.PasswordHash = passwordHash;

            try
            {
                await _repository.CreateAsync(account);
                return Ok(new ResultViewModel<Account>(account));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("Erro interno ao criar a conta"));
            }
        }
    }
}