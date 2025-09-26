using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using simplified_picpay.DTOs;
using simplified_picpay.Models;
using simplified_picpay.Repositories.Abstractions;
using simplified_picpay.Services.Abstractions;
using simplified_picpay.Views.ViewModels;

namespace simplified_picpay.Controllers
{
    [ApiController]
    [Authorize(Roles = "user, storekeeper")]
    [Route("accounts")]
    public class AccountController(IAccountRepository repository,
    IAccountService AccountService, ITokenService tokenService) : ControllerBase
    {
        private readonly IAccountRepository _repository = repository;
        private readonly IAccountService _accountService = AccountService;
        private readonly ITokenService _tokenService = tokenService;

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO,
                                    CancellationToken cancellationToken)
        {
            var account = await _repository.LoginAsync(loginDTO.Email, cancellationToken);

            if (account == null)
                return Unauthorized(new ResultViewModel<string>("Acesso negado!"));

            if (!_accountService.CheckPassword(account, account.PasswordHash, loginDTO.Password))
                return Unauthorized(new ResultViewModel<string>("Acesso negado!"));

            if (account.IsActive == false)
            {
                if (!await _accountService.EnableAccount(account.Id, cancellationToken))
                    return StatusCode(500, new ResultViewModel<string>("Erro interno ao reativar sua conta!"));

                return Ok(new ResultViewModel<string>(data: "Conta reativada!"));
            }

            var token = _tokenService.GenerateTokenJwt(account.Id, account.Email, account.AccountType);

            return Ok(new ResultViewModel<LoggedAccountViewModel>(new LoggedAccountViewModel
            {
                Id = account.Id,
                FullName = account.FullName,
                Email = account.Email,
                CurrentBalance = account.CurrentBalance,
                AccountType = account.AccountType,
                Document = account.Document,
                Token = token
            }));

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
                AccountType = createAccountDTO.AccountType.ToLower(),
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
                return Ok(new ResultViewModel<Account>(await _repository.CreateAsync(account, cancellationToken)));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("Erro interno ao criar a conta"));
            }
        }

        [HttpPut]
        public IActionResult Update([FromBody] UpdateAccountDTO updateAccountDTO)
        {
            var id = _tokenService.GetAccounId(this.HttpContext);
            var account = new Account
            {
                Id = id,
                Email = updateAccountDTO.Email,
                PasswordHash = updateAccountDTO.Password
            };

            var passwordHash = _accountService.PasswordHasher(account, account.PasswordHash);
            account.PasswordHash = passwordHash;

            try
            {
                return Ok(_repository.Update(account));
            }
            catch (DbUpdateException)
            {
                return Conflict(new ResultViewModel<string>("Email já resgitrado na aplicação!"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("Erro interno ao atualizar a conta"));
            }
        }

        [HttpPut]
        [Route("disable-account")]
        public async Task<IActionResult> DisableAccount(CancellationToken cancellationToken)
        {
            var id = _tokenService.GetAccounId(this.HttpContext);

            try
            {
                await _repository.DisableAccountAsync(id, cancellationToken);
                return Ok(new ResultViewModel<string>(data: "Conta desativada"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("Erro interno ao desativar a conta"));
            }
        }
    }
}